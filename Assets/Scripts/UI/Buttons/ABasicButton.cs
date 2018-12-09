namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public abstract class ABasicButton : MonoBehaviour {

		[SerializeField] protected ABasicPanel[] _closeException;
		[SerializeField] private GameObject _activeObj;
		protected bool _btActive = false;


		protected virtual void Start ( ) {
			UIManager.i.registerButton ( this );
			GetComponent<Button> ( ).onClick.AddListener ( ( ) => {
				onClick ( );
			} );
		}


		public virtual void showButton ( ) {
			GetComponent<UITweener> ( ).PlayForward ( );
		}


		public virtual float hideButton ( ) {
			float delay = 0f;

			GetComponent<UITweener> ( ).PlayReverse ( );

			delay += GetComponent<UITweener> ( ).delay;

			if (_activeObj != null) {
				_activeObj.gameObject.SetActive(false);
			}

			return delay;
		}


		protected abstract void onClick ( );

	}
}
