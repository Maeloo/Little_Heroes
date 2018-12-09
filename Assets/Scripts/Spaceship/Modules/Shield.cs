namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Shield : Module {

		[SerializeField] private GameObject _shieldObject;

        private Vector3 _shieldScale;


		void Start ( ) {
            _initialOrder   = _baseModule.sortingOrder;
            _shieldScale    = _shieldObject.transform.localScale;

            _shieldObject.transform.localScale = Vector3.zero;

            _animModule.SetActive ( false );
		}


		public override void notifyTakeover ( ) {
            //_baseModule.sortingOrder = 10;
            _animModule.SetActive ( true );

            SoundManager.getInstance ( ).playShieldLoop ( true );

			iTween.ScaleTo ( _shieldObject, iTween.Hash (
				"time", .3f,
                "scale", _shieldScale,
				"easetype", iTween.EaseType.easeInOutExpo ) );
		}


		public override void notifyRelease ( ) {
            //_baseModule.sortingOrder = _initialOrder;
            _animModule.SetActive ( false );

            SoundManager.getInstance ( ).playShieldLoop ( false );

			iTween.ScaleTo ( _shieldObject, iTween.Hash (
				"time", .3f,
				"scale", Vector3.zero,
				"easetype", iTween.EaseType.easeInOutExpo ) );
		}

	}
}
