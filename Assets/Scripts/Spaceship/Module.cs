namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Module : Photon.MonoBehaviour {

        [SerializeField] protected SpriteRenderer   _baseModule;
        [SerializeField] protected GameObject       _animModule;

        protected int _initialOrder;

		public float RotationDelay = 1f;

		protected ModuleController _controller;

		protected Quaternion _realRotation;
		protected Quaternion _realLocalRotation;


		public void registerController ( ModuleController controller ) {
			_controller			= controller;
			_realRotation		= transform.rotation;
			_realLocalRotation	= transform.rotation;
		}


		// Contrôle au touch : le module s'oriente sur le touch
		public virtual void onPlayerInput ( Vector3 inputPosition ) {
			_realLocalRotation = Quaternion.LookRotation ( Vector3.forward, inputPosition - transform.position );
		}


		protected void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.rotation );
			}
			else {
				//Network player, receive data
				_realRotation = ( Quaternion ) stream.ReceiveNext ( );
			}
		}


		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime * AppDataModel.NETWORK_LERP );
			}
			else {
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realLocalRotation, Time.deltaTime * RotationDelay );
			}
		}


		public virtual void onPlayerInputEnd ( ) { }
		public virtual void notifyTakeover ( ) { }
		public virtual void notifyRelease ( ) { }


		// [DEPRECATED]

		/*public enum ControlType { 
			SPLIT_SCREEN,
			SIMPLE_TOUCH,
			DUAL_TOUCH
		}


		public ControlType	controlType;
		public float		RotationSpeed;*/


		/*protected void Init ( ) {
			Log.i ( "Initialization" );

			switch ( controlType ) {
				case ControlType.DUAL_TOUCH:
					TKRotationRecognizer recognizer = new TKRotationRecognizer ( );
					recognizer.gestureRecognizedEvent += ( r ) => {
						if ( _controller.isModuleWorking ( ) && !MathUtils.isInRange ( -1f, 1f, recognizer.deltaRotation ) )
							controlDualTouch ( recognizer.deltaRotation );
					};
					TouchKit.addGestureRecognizer ( recognizer );
					break;			
			}	
		}*/


		/*protected void Update ( ) {
			if ( _controller.isModuleWorking ( ) ) {
				switch ( controlType ) {
					case ControlType.SPLIT_SCREEN:
						controlSplitScreen ( );
						break;

					case ControlType.SIMPLE_TOUCH:
						controlSimpleTouch ( );
						break;
				}
			}
		}*/


		// Contrôle au touch : partie gauche de l'écran = rotation à gauche et vice versa
		/*protected void controlSplitScreen ( ) {
			if ( InputController.instance.touchCount > 0 ) {
				if ( ScreenUtils.isPointInLeftPart ( InputController.i.primeTouch ) ) {
					rotateLeft ( RotationSpeed );
				}

				if ( ScreenUtils.isPointInRightPart ( InputController.i.primeTouch ) ) {
					rotateRight ( RotationSpeed );
				}
			}
		}*/


		// Contrôle avec deux touchs	
		/*protected void controlDualTouch ( float rotation ) {
			CameraManager.i.blockZoom ( true );
		
			transform.Rotate ( Vector3.forward, -rotation );
		}*/


		/*protected void rotateRight ( float angle = .1f ) {
			transform.Rotate ( new Vector3 ( 0f, 0f, -angle ) );
		}*/


		/*protected void rotateLeft ( float angle = .1f ) {
			transform.Rotate ( new Vector3 ( 0f, 0f, angle ) );
		}*/


	}
}
