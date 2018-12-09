namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class MainMenu : ABasicPanel {

		[SerializeField] private Image _background;
		private float _backgroundAlpha;


		protected void Start () {
			base.Start ();

		}


		public override float hidePanel ( ) {
			/*if ( !_isActive )
				return 0f;*/

			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );

			float delay = 0f;

			foreach ( ABasicButton button in buttons ) {
				button.hideButton ( );
			}

			GetComponent<UITweener> ( ).PlayReverse ( );

			_isActive = false;

			return delay;
		}


		public override void showPanel ( ) {
			/*if ( _isActive )
				return;*/

			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );
			
			foreach ( ABasicButton button in buttons ) {
				button.showButton ( );
			}

			GetComponent<UITweener> ( ).PlayForward ( );

			_isActive = true;
		}


		void onAlphaChange ( float value ) {
			Color col 	= _background.color;
			col.a 		= value;
			
			_background.color = col;
		}

	}
}
