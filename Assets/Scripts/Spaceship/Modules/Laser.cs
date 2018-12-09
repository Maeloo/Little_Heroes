namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class Laser : Module {

		public float	    FireRate; // in s
		public float	    FireForce;
        public BulletType   Type;

		private float				_lastShot;
		private Transform			_gun1;
		private Transform			_gun2;

		private List<GameObject>	_localPool;

        private Animator            _shootAnim;
        private bool                _isAnimPlaying;


		protected void Start ( ) {
			Init ( );
		}


		protected void Init ( ) {
			Log.i ( "Initialization" );

			_lastShot	    = Time.time;
			_gun1		    = transform.FindChild ( "gun_cannon_laser1" );
			_gun2		    = transform.FindChild ( "gun_cannon_laser2" );
            _initialOrder   = _baseModule.sortingOrder;
            _isAnimPlaying  = false;

            _shootAnim = _animModule.GetComponent<Animator> ( );

            stopAnim ( );
		}


        [RPC]
        public void stopAnim ( ) {
            _isAnimPlaying = false;

            _shootAnim.Play ( 0 );
            _shootAnim.StartPlayback ( );
        }


        public override void onPlayerInputEnd ( ) {
            stopAnim ( );

            photonView.RPC (
                "stopAnim",
                PhotonTargets.Others,
                new object[] { } );
        }


		public override void onPlayerInput ( Vector3 inputPosition ) {
			//transform.rotation = Quaternion.LookRotation ( Vector3.forward, inputPosition - transform.position );
			base.onPlayerInput ( inputPosition );

			if ( Time.time - _lastShot > FireRate ) {
				shootBullet ( );
			}
		}


        public override void notifyTakeover ( ) {
            _baseModule.sortingOrder = 10;
        }


        public override void notifyRelease ( ) {
            _baseModule.sortingOrder = _initialOrder;
        }


		private int gunAnim = 1;
		protected void shootBullet ( ) {
            if ( !_isAnimPlaying ) {
                _shootAnim.StopPlayback ( );
                _shootAnim.Play ( 0 );

                _isAnimPlaying = true;
            }            

			if ( _localPool == null ) {
				//_localPool = MunitionManager.i.getLasers ( GameClient.i.myAvatar.power  );
                _localPool = MunitionManager.i.getLasers ( Type );
			}



			GameObject bullet =  _localPool[0];
			_localPool.RemoveAt ( 0 );
			_localPool.Add ( bullet );

			bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			bullet.transform.parent = transform;

			Vector3 inputPosition		= InputController.i.primeTouchRealWorldPosition;
			bullet.transform.position	= gunAnim % 2 == 1 ? _gun1.position : _gun2.position;
			gunAnim++;

			if (!PhotonNetwork.isMasterClient) {
				if (gunAnim % 2 == 1) {
					//Sound
					SoundManager.getInstance ().playShootLaser ();
				}
			}

			bullet.transform.rotation	= Quaternion.LookRotation ( Vector3.forward, inputPosition - bullet.transform.position );
			bullet.SetActive ( true );

			Vector3 heading		= ( bullet.transform.position - inputPosition );
			float distance		= heading.magnitude;
			Vector3 direction	= heading / distance;
			Vector3 force		= direction * FireForce;

			bullet.GetComponent<Rigidbody2D> ( ).AddForce ( force );
			bullet.transform.parent = MunitionManager.i.transform;

			gameObject.GetPhotonView ( ).RPC (
				"onLaserShot",
				PhotonTargets.Others,
				new object[] { direction, BulletTypeClass.typeToString ( GameClient.i.myAvatar.power ) } );

			_lastShot = Time.time;
		}


		[RPC]
		public void onLaserShot ( Vector3 direction, string type ) {
            if ( !_isAnimPlaying ) {
                _shootAnim.StopPlayback ( );
                _shootAnim.Play ( 0 );

                _isAnimPlaying = true;
            }

			if ( _localPool == null ) {
				//_localPool = MunitionManager.i.getLasers ( BulletTypeClass.stringToType ( type ) );
                _localPool = MunitionManager.i.getLasers ( Type );
			}


			//transform.rotation = Quaternion.LookRotation ( Vector3.forward, -direction );

			GameObject bullet =  _localPool[0];
			_localPool.RemoveAt ( 0 );
			_localPool.Add ( bullet );

			bullet.GetComponent<Rigidbody2D> ( ).velocity = Vector3.zero;
			bullet.transform.parent		= transform;
			bullet.transform.position	= gunAnim%2 == 1 ? _gun1.position : _gun2.position;
			gunAnim++;

			if (!PhotonNetwork.isMasterClient) {
				if (gunAnim % 2 == 1) {
					//Sound
					SoundManager.getInstance ().playShootLaser ();
				}
			}

			bullet.transform.rotation	= Quaternion.LookRotation ( Vector3.forward, direction );
			bullet.transform.parent		= MunitionManager.i.transform;
			bullet.SetActive ( true );
			bullet.GetComponent<Rigidbody2D> ( ).AddForce ( direction * FireForce );
		}

	}
}
