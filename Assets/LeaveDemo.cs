using UnityEngine;
using System.Collections;

public class LeaveDemo : MonoBehaviour {

    public void onClick ( ) {
        PhotonNetwork.Disconnect ( );
    }


    void OnDisconnectedFromPhoton ( ) {
        Application.LoadLevelAsync ( "HomeSolo-Scene" );
    }

}
