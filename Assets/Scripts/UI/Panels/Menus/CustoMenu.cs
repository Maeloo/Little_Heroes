namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class CustoMenu : ABasicPanel {

		[SerializeField] private GameObject _background;
		[SerializeField] private ABasicPanel _closePanel;

		private CustoPanelButton[] buttons;


		public override void showPanel ( ) {
			//base.showPanel ( );
			if ( _isActive )
				return;

			buttons = gameObject.GetComponentsInChildren<CustoPanelButton> ( );

			GameObject gameBtMulti = GameObject.Find("ButtonMulti");     
			ActionButton btMulti = gameBtMulti.GetComponent<ActionButton> ();
			btMulti.hideButton ();
			GameObject gameBtCusto = GameObject.Find("ButtonCusto");     
			PanelButton btCusto = gameBtCusto.GetComponent<PanelButton> ();
			btCusto.hideButton ();

			int indexButton = 0;
			foreach ( CustoPanelButton button in buttons ) {
				button.showButton ( );
				button.setIndex(indexButton);
				indexButton++;
			}

			if ( _background != null )
				_background.GetComponent<UITweener> ( ).PlayForward ( );

			_isActive = true;
		}


		public override float hidePanel ( ) {
			//base.hidePanel ( );

			if ( !_isActive )
				return 0;

			float delay = 0f;
			
			foreach ( CustoPanelButton button in buttons ) {
				delay += button.hideButton ( );
			}

			_closePanel.showPanel ();

			if ( _background != null )
				_background.GetComponent<UITweener> ( ).PlayReverse ( );

			_isActive = false;

			return delay;
		}

		public void selectItems(int index){
			foreach ( CustoPanelButton button in buttons ) {
				button.setActive(false);
			}
			buttons [index].setActive (true);
		}
	
	}
}