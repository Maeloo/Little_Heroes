namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Reactor : Module {

		public float Acceleration;
		public float Smooth;

		private float	    _force;
		private bool    	_off;

		private GameObject  _reactorParticles;
		private Vector3     _velocity = Vector3.zero;
        private Animator    _shootAnim;
        private bool        _isAnimPlaying;

		public Vector3 getPropulsion ( ) {
			if ( _controller.isModuleWorking ( ) ) {
				_velocity = -transform.up.normalized * _force;
				return _velocity;
			}
			else {
				_velocity = Vector3.Lerp ( _velocity, Vector3.zero, Time.deltaTime );
				return _velocity;
			}
		}


		protected void Start ( ) {
			Init ( );
		}


		protected void Init ( ) {
			Log.i ( "Initialization" );

			_reactorParticles = transform.FindChild ( "reactor_particles_v2" ).gameObject;
			_reactorParticles.SetActive ( false );

			_force	        = 0f;
			_off	        = true;
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


		[RPC]
		public void startEngine ( bool start ) {
            SoundManager.getInstance ( ).playReactorLoop ( start );

            if ( !start )
                stopAnim ( );

			_reactorParticles.SetActive ( start );
			_off = true;
		}


		protected void Update ( ) {
			base.Update ( );

			if ( _off ) {
				stabilize ( );
			}
		}


		void stabilize ( ) {
			_force = Mathf.Lerp ( _force, 0f, Time.deltaTime );
		}


		public override void onPlayerInput ( Vector3 inputPosition ) {
			_off = false;

			transform.rotation = Quaternion.LookRotation ( Vector3.forward, inputPosition - transform.position );

			if ( !_reactorParticles.activeSelf ) {
				GetComponent<PhotonView> ( ).RPC (
					"startEngine",
					PhotonTargets.All,
					new object[] { true } );
			}

			_force = Mathf.Lerp ( _force, Acceleration, Time.deltaTime * Smooth );
		}


        public override void notifyTakeover ( ) {
            _baseModule.sortingOrder = 10;

            _shootAnim.StopPlayback ( );
            _shootAnim.Play ( 0 ); 
        }


        public override void notifyRelease ( ) {
            _baseModule.sortingOrder = _initialOrder;
        }


		public override void onPlayerInputEnd ( ) {
            stopAnim ( );

			GetComponent<PhotonView> ( ).RPC (
					"startEngine",
					PhotonTargets.All,
					new object[] { false } );
		}


		// [DEPRECATED]

		/*protected void Update ( ) {
			//base.Update ( );

			if ( InputController.instance.touchCount > 0 && _controller.isModuleWorking ( ) ) {
				if ( !_reactorParticles.activeSelf )
					_reactorParticles.SetActive ( true );

				_force = Mathf.Lerp ( _force, Acceleration, Time.deltaTime * Smooth );			
			}
			else {
				_reactorParticles.SetActive ( false );
				_force = 0;
			}
		}*/


	}
}
