namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class HomeMenu : ABasicPanel {

		public override float hidePanel ( ) {
			if ( !_isActive )
				return 0f;

			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );

			float delay = 0f;

			foreach ( ABasicButton button in buttons ) {
				button.hideButton ( );
			}

			_isActive = false;

			return delay;
		}


		public override void showPanel ( ) {
			if ( _isActive )
				return;

			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );

			foreach ( ABasicButton button in buttons ) {
				button.showButton ( );
			}

			_isActive = true;
		}

	}
}