namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class Dephasor : Module {

        public float	    FireRate; // in s
        public float	    FireForce;
        public float        FireAngleRange;
        public BulletType   Type;

        private float				_lastShot;
        private Transform			_gun;

        private List<GameObject>	_localPool;

        private Animator            _shootAnim;
        private bool                _isAnimPlaying;


        protected void Start ( ) {
            Init ( );
        }


        protected void Init ( ) {
            Log.i ( "Initialization" );

            _lastShot       = Time.time;
            _gun            = transform.FindChild ( "gun_cannon" );
            _initialOrder   = _baseModule.sortingOrder;
            _shootAnim      = _animModule.GetComponent<Animator> ( );
            _isAnimPlaying  = false;
            
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


        protected void shootBullet ( ) {
            if ( !_isAnimPlaying ) {
                _shootAnim.StopPlayback ( );
                _shootAnim.Play ( 0 );

                _isAnimPlaying = true;
            }

            if ( _localPool == null ) {
                //_localPool = MunitionManager.i.getBullets ( GameClient.i.myAvatar.power );
                _localPool = MunitionManager.i.getLasers ( Type );
            }

			if (!PhotonNetwork.isMasterClient) {
				//Sound
				SoundManager.getInstance ().playShootDephasor ();
			}


            GameObject bullet           = _localPool[0];
            bullet.SetActive ( true );

            _localPool.RemoveAt ( 0 );
            _localPool.Add ( bullet );

            bullet.GetComponent<Rigidbody2D> ( ).velocity = Vector3.zero;
            bullet.transform.parent     = transform;

            Vector3 inputPosition		= InputController.i.primeTouchRealWorldPosition;
            bullet.transform.position   = _gun.position;

            Vector3 heading		        = ( bullet.transform.position - inputPosition );
            float distance		        = heading.magnitude;
            
            Vector3 direction	        = heading / distance;
            direction                   = Quaternion.Euler ( 0f, 0f, Random.Range ( -FireAngleRange, FireAngleRange ) ) * direction;
            bullet.transform.rotation   = Quaternion.LookRotation ( Vector3.forward, direction );
            
            Vector3 force		        = direction * FireForce;
            bullet.GetComponent<Rigidbody2D> ( ).AddForce ( force );

            bullet.transform.parent     = MunitionManager.i.transform;

            gameObject.GetPhotonView ( ).RPC (
                "onBulletShot",
                PhotonTargets.Others,
                new object[] { direction, BulletTypeClass.typeToString ( GameClient.i.myAvatar.power ) } );

            _lastShot = Time.time;
        }


        [RPC]
        public void onBulletShot ( Vector3 direction, string type ) {
            if ( !_isAnimPlaying ) {
                _shootAnim.StopPlayback ( );
                _shootAnim.Play ( 0 );

                _isAnimPlaying = true;
            }

            if ( _localPool == null ) {
                //_localPool = MunitionManager.i.getBullets ( BulletTypeClass.stringToType ( type ) );
                _localPool = MunitionManager.i.getLasers ( Type );
            }

			if (!PhotonNetwork.isMasterClient) {
				//Sound
				SoundManager.getInstance ().playShootDephasor ();
			}

            //transform.rotation = Quaternion.LookRotation ( Vector3.forward, -direction );

            GameObject bullet =  _localPool[0];
            _localPool.RemoveAt ( 0 );
            _localPool.Add ( bullet );

            bullet.GetComponent<Rigidbody2D> ( ).velocity = Vector3.zero;
            bullet.transform.parent     = transform;

            bullet.transform.position   = _gun.position;
            bullet.transform.rotation   = Quaternion.LookRotation ( Vector3.forward, direction );

            bullet.transform.parent     = MunitionManager.i.transform;

            bullet.SetActive ( true );

            bullet.GetComponent<Rigidbody2D> ( ).AddForce ( direction * FireForce );
        }
    }
}
