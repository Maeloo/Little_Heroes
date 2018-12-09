namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	
	public class CustoPanelButton : ABasicButton {
		
		[SerializeField] private ABasicPanel _panel;
		[SerializeField] private CustoMenu _panelParent;

		private int index = 0;
		private GameObject objActive;
		
		protected override void onClick () {
			if ( _panel != null ) {
				SoundManagerUI.getInstance().playItemsMasterSelect();
				_panelParent.selectItems(index);
				if ( !_panel.isActive ) {
					UIManager.i.openPanel ( _panel.showPanel, _closeException );
						_btActive = true;					
								
				}
				else {				
					_panel.hidePanel ( );
					GameObject objActive = transform.Find ("active").gameObject;
					if (objActive != null) {
						transform.Find ("active").gameObject.SetActive (false);	
					}
					_btActive = false;
				}
			}
			else {
				UIManager.i.closeAllPanels ( );
			}
		}

		public void setIndex(int _index){
			index = _index;
		}

		public void setActive(bool active){
			GameObject objActive = transform.Find ("active").gameObject;
			if (objActive != null) {
				transform.Find ("active").gameObject.SetActive (active);	
			}
		}
	}
}
