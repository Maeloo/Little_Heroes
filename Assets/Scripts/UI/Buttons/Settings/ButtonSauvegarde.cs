namespace LittleHeroes{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class ButtonSauvegarde : ABasicButton {
		[SerializeField] private ABasicPanel _panelClose;
		[SerializeField] private InputField _inputPseudo;
		[SerializeField] private InputField _inputServerRoom;
		[SerializeField] private InputField _inputServerIP;
		[SerializeField] private InputField _inputServerPORT;

		protected override void onClick () {
			//Pseudo
			UIManager.i.avatar.setPlayerName (_inputPseudo.text);
			//Server Room, IP, PORT
			//Save
			UIManager.i.setServerIp (_inputServerIP.text);
			UIManager.i.setServerPort (int.Parse (_inputServerPORT.text));
			UIManager.i.setServerName (_inputServerRoom.text);
			//Photon
			AppDataModel.ROOM_NAME = _inputServerRoom.text;
			NetworkConfig.setServerSettings ( _inputServerIP.text, int.Parse ( _inputServerPORT.text ) );
			//Close
			_panelClose.hidePanel ();
			UIManager.i.setOldClick ("");
		} 

	}

}