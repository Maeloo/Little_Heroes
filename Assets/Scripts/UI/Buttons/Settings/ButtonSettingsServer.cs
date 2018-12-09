namespace LittleHeroes {
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class ButtonSettingsServer : MonoBehaviour {

        public InputField serverAddress;
        public InputField serverPort;
        public InputField roomName;


        void Start ( ) {
            Screen.orientation = ScreenOrientation.Landscape;
        }


        public void onClick ( ) {
            AppDataModel.ROOM_NAME = roomName.text;
            NetworkConfig.setServerSettings ( serverAddress.text, int.Parse ( serverPort.text ) );
            Application.LoadLevelAsync ( "MainMaster-Scene" );
        }
    }
}