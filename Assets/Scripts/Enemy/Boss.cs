namespace LittleHeroes {
	using UnityEngine;
    using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;

	public class Boss : Enemy {

		protected void Start ( ) {
			Init ( );
		}


		protected void Init ( ) {
			Log.i ( "Initialization" );

			_realPosition	= transform.position;
			_realRotation	= transform.rotation;
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

			_selfBody = GetComponent<Rigidbody2D> ( );
		}


		protected override void lookForPlayers ( ) {
			// TODO
		}


		protected void OnTriggerEnter2D ( Collider2D collider ) {
			PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet> ( );

			if ( bullet ) {
				_life -= bullet.damage;
				bullet.destroy ( true );

				if ( PhotonNetwork.isMasterClient && _life <= 0 ) {
					photonView.RPC (
						"onBossDeath",
						PhotonTargets.All,
						new object[] { } );

					foreach ( Player player in FindObjectsOfType<Player> ( ) ) {
						player.photonView.RPC (
							"notifyBossKilled",
							PhotonTargets.All,
							new object[] { } );
					}
				}
			}
		}


		[RPC]
		public void activeBoss ( Vector2 newPos ) {
			transform.position = newPos;

			_realPosition = newPos;

			//transform.parent.gameObject.SetActive ( true );
		}


		[RPC]
		protected void onBossDeath ( ) {
            float x =  transform.position.x;
            float y =  transform.position.y;

            Vector2 rangeX = new Vector2 ( x - 2f, x + 2f );
            Vector2 rangeY = new Vector2 ( y - 2f, y + 2f );

            Vector3 newPos1 =  MathUtils.randomBetweenRange ( rangeX, rangeY );
            Vector3 newPos2 =  MathUtils.randomBetweenRange ( rangeX, rangeY );
            Vector3 newPos3 =  MathUtils.randomBetweenRange ( rangeX, rangeY );

            GameObject.Instantiate ( _explosionPrefab, newPos1, Quaternion.identity );
            GameObject.Instantiate ( _explosionPrefab, newPos2, Quaternion.identity );
            GameObject.Instantiate ( _explosionPrefab, newPos3, Quaternion.identity );

            if ( _gameCanvas == null )
                _gameCanvas = GameObject.Find ( "GameCanvas" );

            GameObject point = NGUITools.AddChild ( _gameCanvas, _pointPrefab );
            point.transform.position = transform.position;
            point.GetComponent<DeathPoint> ( ).init ( );
            point.GetComponent<Text> ( ).text = "+" + _point;

            transform.parent.localPosition = new Vector3 ( 9999f, 9999f, 9999f );
			//transform.parent.gameObject.SetActive ( false );
		}


	}
}
