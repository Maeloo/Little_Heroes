namespace LittleHeroes {
	using UnityEngine;
    using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;

	public class Enemy : Photon.MonoBehaviour {

		[SerializeField]
		protected float _speed;

        [SerializeField]
        protected float _minDistance;

		[SerializeField]
		protected float _life;

        [SerializeField]
        protected float _point;

		[SerializeField]
		protected GameObject _bulletPrefab;

		[SerializeField]
		protected GameObject _explosionPrefab;

        [SerializeField]
        protected GameObject _pointPrefab;


		public float FireRate;
		public float FireForce;
        public float FireAngleRange;

		protected Vector3			_lastPositionDetected;

		protected Vector3			_realPosition;
        protected Vector3           _realScale;
		protected Quaternion		_realRotation;

		protected bool				_isAgressive;

		protected float				_lastShot;

		protected List<GameObject>	_bulletPool;

		protected Rigidbody2D		_selfBody;
        
        protected GameObject        _gameCanvas;


		protected void Start ( ) {
			Init ( );
		}


		protected void Init ( ) {
			Log.i ( "Initialization" );

			_realPosition	= transform.position;
			_realRotation	= transform.rotation;
            _realScale      = transform.localScale;
			_isAgressive	= false;
			_lastShot		= Time.time;

			GameObject ammoStock = GameObject.Find ( "ammo_stock" );

			_bulletPool = new List<GameObject> ( );
			for ( int i = 0; i < 100; ++i ) {
				GameObject bullet = ( GameObject ) Instantiate ( _bulletPrefab );
				bullet.SetActive ( false );

				if ( ammoStock )
					bullet.transform.parent = ammoStock.transform;

				_bulletPool.Add ( bullet );
			}

			_selfBody   = GetComponent<Rigidbody2D> ( );            

			//gameObject.SetActive ( false );
		}


		public void OnPhotonPlayerConnected ( PhotonPlayer newcomer ) {			
			Debug.Log ( "Player Connected " + newcomer.name );
			if ( PhotonNetwork.isMasterClient ) {
				photonView.RPC (
				"syncOnConnection",
				PhotonTargets.Others,
				new object[] { transform.position, transform.localScale } );
			}
		}


		[RPC]
		public void syncOnConnection ( Vector3 position, Vector3 scale ) {
			_realPosition		 = position;
            _realScale           = scale;

			transform.position	 = position;
            transform.localScale = scale;
		}


		protected void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.position );
				stream.SendNext ( transform.rotation );
                stream.SendNext ( transform.localScale );
			}
			else {
				//Network player, receive data
				_realPosition   = ( Vector3 ) stream.ReceiveNext ( );
				_realRotation   = ( Quaternion ) stream.ReceiveNext ( );
                _realScale      = ( Vector3 ) stream.ReceiveNext ( );
			}
		}


		[RPC]
		public void activeEnemy ( Vector2 newPos ) {
			Log.i ( "ActiveEnemy" );
			transform.position	= newPos;
			_realPosition		= newPos;

			//gameObject.SetActive ( true );
		}


		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.position      = Vector3.Lerp ( transform.position, _realPosition, Time.deltaTime );
				transform.rotation      = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime );
                transform.localScale    = Vector3.Lerp ( transform.localScale, _realScale, Time.deltaTime );
			}
			else {
				if ( _isAgressive ) {
					stabilize ( );

					if ( Time.time - _lastShot > FireRate ) {
						shootBullet ( );
					}
				}
				else if ( !_isAgressive ) {
					lookForPlayers ( );
				}
			}
		}


		virtual protected void lookForPlayers ( ) {
			Vector3		heading		= Spaceship.i.transform.position - transform.position;
			float		distance	= heading.magnitude;

            if ( distance < _minDistance )
                return;

			Vector3		direction	= heading / distance;
			Quaternion	rotation	= Quaternion.LookRotation ( Vector3.forward, direction );

			transform.rotation = Quaternion.Lerp ( transform.rotation, rotation, Time.deltaTime );

			Vector3 force			= direction * _speed;

			_selfBody.AddForce ( force );
		}


		virtual protected void stabilize ( ) { 
			_selfBody.velocity = Vector3.Lerp( _selfBody.velocity, Vector3.zero, Time.deltaTime);
		}


		protected void shootBullet ( ) {
			GameObject bullet =  _bulletPool[0];
			_bulletPool.RemoveAt ( 0 );
			_bulletPool.Add ( bullet );

			bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			bullet.transform.position = transform.position;

			bullet.SetActive ( true );

			if (!PhotonNetwork.isMasterClient) {
				SoundManager.getInstance ().playShootLaserEnemy ();
			}

			Vector3 heading		= ( _lastPositionDetected - bullet.transform.position );
			float distance		= heading.magnitude;
			Vector3 direction	= heading / distance;
            direction = Quaternion.Euler ( 0f, 0f, Random.Range ( -FireAngleRange, FireAngleRange ) ) * direction;
			
            Vector3 force		= direction * FireForce;
			bullet.GetComponent<Rigidbody2D>().AddForce ( force );

			gameObject.GetPhotonView ( ).RPC (
				"onBulletShot",
				PhotonTargets.Others,
				new object[] { direction } );

			_lastShot = Time.time;
			_isAgressive = false;
		}


		[RPC]
		public void onBulletShot ( Vector3 direction ) {
			if ( _bulletPool== null )
				return;

			GameObject bullet =  _bulletPool[0];
			_bulletPool.RemoveAt ( 0 );
			_bulletPool.Add ( bullet );

			if (!PhotonNetwork.isMasterClient) {
				SoundManager.getInstance ().playShootLaserEnemy ();
			}

			bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			bullet.transform.position = transform.position;

			bullet.SetActive ( true );

			bullet.GetComponent<Rigidbody2D>().AddForce ( direction * FireForce );
		}


		protected void OnTriggerEnter2D ( Collider2D collider ) {
			PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet> ( );

			if ( bullet ) {
				_life -= bullet.damage;
				bullet.destroy ( true );

				if ( PhotonNetwork.isMasterClient && _life <= 0 ) {
                    GameMaster.instance.onEnemyKilled ( _point );

					photonView.RPC (
						"onEnemyDeath",
						PhotonTargets.All,
						new object[] { } );

					foreach ( Player player in FindObjectsOfType<Player> ( ) ) {
						player.photonView.RPC (
							"notifyEnemyKilled",
							PhotonTargets.Others,
							new object[] { } );
					}
				}
			}
		}


		public void sendTargetCoord ( Vector3 targetPosition ) {
			_lastPositionDetected   = targetPosition;
			_isAgressive            = true;
		}


		[RPC]
		protected void onEnemyDeath ( ) {
            if ( _gameCanvas == null )
                _gameCanvas = GameObject.Find ( "GameCanvas" );

            GameObject point = NGUITools.AddChild ( _gameCanvas, _pointPrefab );
            point.transform.position = transform.position;
            point.GetComponent<DeathPoint> ( ).init ( );
            point.GetComponent<Text> ( ).text = "+" + _point;

			if (!PhotonNetwork.isMasterClient) {
				SoundManager.getInstance ().playSoundExplosion ();
			}

			GameObject.Instantiate ( _explosionPrefab, transform.position, Quaternion.identity );
            transform.position = new Vector3 ( 9999f, 9999f, 9999f );
			//gameObject.SetActive ( false );
		}
	}
}
