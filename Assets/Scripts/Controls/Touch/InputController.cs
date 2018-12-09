namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class InputController : MonoBehaviour {

		#region Singleton Stuff
		private static InputController _instance		= null;
		private static readonly object singletonLock	= new object ( );
		#endregion

		private Camera		_inputCamera;

		private int			_touchCount;
		private Vector3[]	_touchList;

		public Rect         DisableArea;
        public GameObject   Touch;


		public int touchCount {
			get { return _touchCount; }
		}


		public Vector3[] touchList {
			get { return _touchList; }
		}


		public Vector3 primeTouch {
			get { return _touchList[0]; }
		}

		public Camera inputCamera {
			get {
				if ( _inputCamera == null )
					_inputCamera = GameObject.FindGameObjectWithTag ( "InputCamera" ).GetComponent<Camera> ( );
				return _inputCamera; 
			}
		}


		public static InputController i {
			get { return instance; }
		}

		public static InputController instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( InputController ) GameObject.Find ( "InputController" ).GetComponent<InputController> ( );

						DontDestroyOnLoad ( _instance );
					}
					return _instance;
				}
			}
		}


		private void Start ( ) {
			Init ( );
		}


		private void Init ( ) {
			Log.i ( "Initialization" );
			_touchCount		= 0;
			_touchList		= new Vector3[10];

            Touch.transform.parent = inputCamera.transform;
		}


        void displayTouch ( ) {
            //TweenAlpha.Begin ( Touch, .2f, .65f );
            //TweenScale.Begin ( Touch, .2f, new Vector3 ( .09f, .09f, 1 ) );

            iTween.FadeTo ( Touch, iTween.Hash (
                "time", .2f,
                "easetype", iTween.EaseType.easeInOutExpo,
                "alpha", .65f ) );

            iTween.ScaleTo ( Touch, iTween.Hash (
                "time", .2f,
                "easetype", iTween.EaseType.easeInOutExpo,
                "scale", new Vector3 ( 1f, 1f, 1f ) ) );
        }

        void hideTouch ( ) {
            //TweenAlpha.Begin ( Touch, .2f, 0f );
            //sTweenScale.Begin ( Touch, .2f, new Vector3 ( .14f, .14f, 1 ) );

            iTween.FadeTo ( Touch, iTween.Hash (
                "time", .2f,
                "easetype", iTween.EaseType.easeInOutExpo,
                "alpha", 0 ) );

            iTween.ScaleTo ( Touch, iTween.Hash (
                "time", .2f,
                "easetype", iTween.EaseType.easeInOutExpo,
                "scale", new Vector3 ( 0f, 0f, 1f ) ) );
        }


		private void Update ( ) {
			if ( Input.touchCount > 0 || Input.GetButton ( "Fire1" ) ) {
				UpdateTouchInput ( );
				UpdateMouseInput ( );

                Vector3 touchPos = primeTouchWorldPosition;
                touchPos.z = 1f;
                Touch.transform.position = touchPos;

                displayTouch ( );
			}
			else {
				_touchCount = 0;

                hideTouch ( );
			}
		}


		private void UpdateTouchInput ( ) {
			if ( Input.touchCount > 0 ) {
				_touchCount = Input.touchCount;

				for ( int i = 0; i < _touchCount; ++i ) {
					_touchList[i] = Input.GetTouch ( i ).position;
				}

				if ( _touchCount >= 2 ) {

				}
			}
		}


		private void UpdateMouseInput ( ) {
			if ( Input.GetButtonDown ( "Fire1" ) || _touchCount > 0 ) {
				_touchCount = _touchCount > 1 ? _touchCount : 1;
				_touchList[0] = Input.mousePosition;
			}
		}


		public Vector3 primeTouchWorldPosition {
			get { return inputCamera.ScreenToWorldPoint ( _touchList[0] ); }
		}


		public Vector3 primeTouchRealWorldPosition {
			get { return Camera.main.ScreenToWorldPoint ( _touchList[0] ); }
		}
	}
}
