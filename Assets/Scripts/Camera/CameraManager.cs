namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class CameraManager : MonoBehaviour {

		#region Singleton Stuff
		private static CameraManager	_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion

		public enum ZoomState {
			IN,
			OUT
		}

		public float ZoomMax;
		public float ZoomMin;

		[Range ( 0, 1 )]
		public float ZoomStep;

		private bool		_zoomEnable;
		private bool		_isZooming;

		private ZoomState	_state;
		public ZoomState state {
			get { return _state; }
		}

		public static CameraManager i {
			get { return instance; }
		}

		public static CameraManager instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
                        GameObject instance =  GameObject.Find ( "CameraManager" );

                        if ( instance != null )
                            _instance = ( CameraManager ) instance.GetComponent<CameraManager> ( );
                        else
                            return null;

						DontDestroyOnLoad ( _instance );
					}
					return _instance;
				}
			}
		}


		private void Start ( ) {
			Init ( );
		}


		private void Update ( ) {
			if ( ( InputController.i.touchCount == 0 && _isZooming ) || ( !_zoomEnable && _isZooming ) ) {
				UpdateZoom ( );
			}
		}


		public void UpdateZoom ( ) {
			if ( _state == ZoomState.OUT ) {
				zoomOut ( );
			}
			else if ( _state == ZoomState.IN ) {
				zoomIn ( );
			}
		}


		private void Init ( ) {
			Log.i ( "Initialization" );

			zoomOut ( );

			_zoomEnable = true;
			_state = ZoomState.OUT;

			TKPinchRecognizer recognizer = new TKPinchRecognizer ( );
			recognizer.gestureRecognizedEvent += ( r ) => {
				pinchZoom ( recognizer.deltaScale );
			};
			TouchKit.addGestureRecognizer ( recognizer );
		}


		private void pinchZoom ( float deltaScale ) {
			if ( float.IsNaN ( deltaScale ) || !_zoomEnable )
				return;

			if ( _state == ZoomState.OUT && deltaScale > 0f ) {
				if ( Camera.main.fieldOfView > ( 1f - ZoomStep ) * ZoomMax ) {
					_isZooming = true;

					Camera.main.fieldOfView *= ( 1f - deltaScale / 10f );
				}
				else {
					_zoomEnable = false;
					_state = ZoomState.IN;

					zoomIn ( );
				}
			}
			else if ( _state == ZoomState.IN && deltaScale < 0f ) {
				if ( Camera.main.fieldOfView < ( 1f + ZoomStep ) * ZoomMin ) {
					_isZooming = true;

					Camera.main.fieldOfView *= ( 1f - deltaScale / 5f );
				}
				else {
					_zoomEnable = false;
					_state = ZoomState.OUT;

					zoomOut ( );
				}
			}
		}


		private void zoom ( float value ) {
			iTween.ValueTo ( gameObject, iTween.Hash (
						"from", Camera.main.fieldOfView,
						"to", value,
						"time", AppDataModel.ZOOM_TIME,
						"easetype", iTween.EaseType.easeInOutExpo,
						"onupdate", "setZoom",
						"oncomplete", "zoomComplete" ) );
		}


		private void zoomIn ( ) {
			_isZooming = false;

			zoom ( ZoomMin );
			Spaceship.i.showInside ( true );
			GameClient.i.displayCharacter ( true );
		}


		private void zoomOut ( ) {
			_isZooming = false;

			zoom ( ZoomMax );
			Spaceship.i.showInside ( false );
			GameClient.i.displayCharacter ( false );
		}


		private void setZoom ( float value ) {
			Camera.main.fieldOfView = value;
		}


		private void zoomComplete ( ) {
			_zoomEnable = true;
		}


		public void blockZoom ( bool block ) {
			_zoomEnable = !block;
		}

		public void forceSwitchZoomState ( ) {
			if ( _state == ZoomState.IN ) {
				zoomOut ( );

				_state = ZoomState.OUT;
			}
			else if ( _state == ZoomState.OUT ) {
				zoomIn ( );

				_state = ZoomState.IN;
			}
		}

	}
}
