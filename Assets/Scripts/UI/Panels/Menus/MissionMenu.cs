namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	
	public class MissionMenu : ABasicPanel {

		
		public override void showPanel ( ) {
			if ( _isActive )
				return;

			GetComponent<UITweener> ( ).PlayForward ( );
			
			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );
			
			GameObject gameBtMulti = GameObject.Find("ButtonMulti");     
			ActionButton btMulti = gameBtMulti.GetComponent<ActionButton> ();
			btMulti.hideButton ();
			GameObject gameBtCusto = GameObject.Find("ButtonCusto");     
			PanelButton btCusto = gameBtCusto.GetComponent<PanelButton> ();
			btCusto.hideButton ();

			
			foreach ( ABasicButton button in buttons ) {
				button.showButton ( );
			}
			
			_isActive = true;

			UIManager.i.avatar.fadeOut ();			
			
		}
		
		
		public override float hidePanel ( ) {
			if ( !_isActive )
				return 0;

			UIManager.i.avatar.fadeIn ();
			
			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );

			GetComponent<UITweener> ( ).PlayReverse ( );

			float delay = 0f;
			
			foreach ( ABasicButton button in buttons ) {
				delay += button.hideButton ( );
			}

			_isActive = false;
			
			return delay;
		}
		
	}
}