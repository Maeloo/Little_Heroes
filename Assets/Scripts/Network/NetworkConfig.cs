namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class NetworkConfig : MonoBehaviour {

        public static void setServerSettings ( string address, int port ) {
            PhotonNetwork.PhotonServerSettings.UseMyServer (
                    address,
                    port,
                    PhotonNetwork.PhotonServerSettings.AppID );
        }

    }
}
