namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class Router : Photon.MonoBehaviour {

		private void Start ( ) {
			routeUser ( );
		}


		private void routeUser ( ) {
			Log.i ( "Routing ..." );
			if ( !PhotonNetwork.isMasterClient ) {
				Application.LoadLevelAdditive ( "GameClient-Scene" );
			}
			else {
				Application.LoadLevelAdditive ( "GameMaster-Scene" );
			}
		}

	}
}
