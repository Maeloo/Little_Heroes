namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Graviton : Module {

		[SerializeField]
		private Transform _anchor;

		public float _attractionForce;
		public float _repulsionForce;

		private GravitonBeam _beam;

		private GravitonTarget _currentTarget;
		public	GravitonTarget currentTarget {
			get { return _currentTarget; }
		}


		void Start ( ) {
			_beam           = GetComponentInChildren<GravitonBeam> ( );
            _initialOrder   = _baseModule.sortingOrder;
		}

		
		public override void onPlayerInput ( Vector3 inputPosition ) {
			_realLocalRotation = Quaternion.LookRotation ( Vector3.forward, inputPosition - transform.position );

			if ( _beam.currentTarget != null && _currentTarget != _beam.currentTarget ) {
				_currentTarget = _beam.currentTarget;

				_currentTarget.photonView.RequestOwnership ( );
			}
		}


		public override void onPlayerInputEnd ( ) {
			if ( _currentTarget != null ) {
				Vector3 inputPosition	= InputController.i.primeTouchRealWorldPosition;
				Vector3 heading			= ( _currentTarget.transform.position - inputPosition );
				float distance			= heading.magnitude;
				Vector3 direction		= heading / distance;
				Vector3 force			= direction * _repulsionForce;

				_currentTarget.unlock_ ( );
				//_currentTarget.rigidbody2D.AddForce ( force );

				_currentTarget.photonView.RPC (
					"onTargetShot",
					PhotonTargets.MasterClient,
					new object[] { force } );

				_currentTarget = null;
			}
		}


        public override void notifyTakeover ( ) {
            _baseModule.sortingOrder = 10;
        }


        public override void notifyRelease ( ) {
            _baseModule.sortingOrder = _initialOrder;
        }


		protected void Update ( ) {
			base.Update ( );

			transform.localPosition = Vector3.zero;

			attractTarget ( );
		}


		private void attractTarget ( ) {
			if ( _currentTarget != null && !_currentTarget.locked ) {
				float attraction = _attractionForce / Vector3.Distance ( _currentTarget.transform.position, _anchor.position );
				Vector2 velocity = ( _anchor.position - _currentTarget.transform.position ) * attraction;

				_currentTarget.GetComponent<Rigidbody2D>().velocity = velocity;
			}
		}


		void OnTriggerEnter2D ( Collider2D col ) {
			GravitonTarget gt = col.GetComponent<GravitonTarget> ( );

			if ( gt != null && gt == _currentTarget ) {
				gt.lock_ ( _anchor );
			}
		}

	}
}
