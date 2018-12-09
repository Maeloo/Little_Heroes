namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;

	public class PanelButton : ABasicButton {

		[SerializeField] private ABasicPanel _panel;
		[SerializeField] bool _singlePanel;
		[SerializeField] string _id;

		protected override void Start ( ) {
			UIManager.i.registerButton ( this );

			GetComponent<Button> ( ).onClick.AddListener ( ( ) => {
				onClick ( );
			} );

		}

		protected override void onClick () {
			if ( _panel != null ) {	
				SoundManagerUI.getInstance().playMenuClick();
				if(UIManager.i.oldButtonClick != _id || UIManager.i.oldButtonClick == ""){
					if(_singlePanel){				
						UIManager.i.openSinglePanel ( _panel.showPanel );				
					}else{
						UIManager.i.openPanel ( _panel.showPanel, _closeException );
					}
				}
				if(_id != ""){
					UIManager.i.setOldClick(_id);
					//TODO MATH: Supprimer le menu
				}
				/*if ( !_panel.isActive ) {
					if(_singlePanel){
						UIManager.i.openSinglePanel ( _panel.showPanel );
					}else{
						UIManager.i.openPanel ( _panel.showPanel, _closeException );
					}
					if (_activeObj != null) {
						_activeObj.gameObject.SetActive(true);
						_btActive = true;					
					}

				}
				else {
					_panel.hidePanel ( );
					if(_activeObj != null){
						_activeObj.gameObject.SetActive(false);
					}
					_btActive = false;
				}*/
			}
			else {
				UIManager.i.closeAllPanels ( );
			}
		}
	}
}
