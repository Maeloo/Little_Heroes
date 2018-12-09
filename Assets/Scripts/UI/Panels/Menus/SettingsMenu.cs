namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	
	public class SettingsMenu : ABasicPanel {
		[SerializeField] private InputField _inputPseudo;
		[SerializeField] private InputField _inputServerRoom;
		[SerializeField] private InputField _inputServerIP;
		[SerializeField] private InputField _inputServerPORT;
		
		protected void Start () {
			base.Start ();			
		}
				
		public override float hidePanel ( ) {
			if ( !_isActive )
				return 0f;
			
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
			if ( _isActive )
				return;
			
			ABasicButton[] buttons = gameObject.GetComponentsInChildren<ABasicButton> ( );
			
			foreach ( ABasicButton button in buttons ) {
				button.showButton ( );
			}
		
			GetComponent<UITweener> ( ).PlayForward ( );

			_inputPseudo.text = UIManager.i.avatar.playerName;
			//Server
			_inputServerRoom.text = UIManager.i.getServerName ();
			_inputServerIP.text = UIManager.i.getServerIp ();
			_inputServerPORT.text = UIManager.i.getServerPort ().ToString ();

			_isActive = true;
		}
		

		
	}
}
