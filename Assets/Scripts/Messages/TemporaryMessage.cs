namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class TemporaryMessage : MonoBehaviour {

		public float _lifeTime; // in s

		[SerializeField]
		private Text _message;

		private bool _displayed;


		void Update ( ) {
			/*if ( InputController.i.touchCount > 0 && _displayed ) {
				hideMessage ( );
			}*/
		}


		public void displayMessage ( string msg ) {
			_message.text = msg;

			// TODO : anim apparition
			GetComponent<UITweener> ( ).PlayForward ( );

			_displayed = true;

			Invoke ( "hideMessage", _lifeTime );
		}


		public void hideMessage ( ) {
			// TODO : anim disparition
			GetComponent<UITweener> ( ).PlayReverse ( );

			_displayed = false;

			Invoke ( "destroyMessage", GetComponent<UITweener> ( ).duration );
		}


		private void destroyMessage ( ) {
			DestroyImmediate ( gameObject );
		}

	}
}