namespace LittleHeroes {
	using System;
	using UnityEngine;
	using Random = UnityEngine.Random;

	public class LobbyMaster : MonoBehaviour {

		public bool DEBUG;

		public GUISkin Skin;
		public Vector2 WidthAndHeight = new Vector2 ( 600, 400 );

		private string roomName = AppDataModel.ROOM_NAME;

		private bool connectFailed = false;

		public static readonly string SceneNameMenu = "MainMaster-Scene";
		public static readonly string SceneNameGame = "Lobby-Scene";

		private string errorDialog;
		private double timeToClearDialog;


		public string ErrorDialog {
			get {
				return errorDialog;
			}
			private set {
				errorDialog = value;
				if ( !string.IsNullOrEmpty ( value ) ) {
					timeToClearDialog = Time.time + 4.0f;
				}
			}
		}


		public void Awake ( ) {
            Screen.orientation = ScreenOrientation.Landscape;

			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.automaticallySyncScene = true;

			// the following line checks if this client was just created (and not yet online). if so, we connect
			if ( PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated ) {
				// Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
				PhotonNetwork.ConnectUsingSettings ( "1.0" );
			}

			// generate a name for this player, if none is assigned yet
			if ( String.IsNullOrEmpty ( PhotonNetwork.playerName ) ) {
				PhotonNetwork.playerName = "Master";
			}

			//PlayerPrefs.SetString ( "playerName", PhotonNetwork.playerName );

			// if you wanted more debug out, turn this on:
			// PhotonNetwork.logLevel = NetworkLogLevel.Full;
		}


		public void OnGUI ( ) {
			if ( DEBUG ) {
				if ( this.Skin != null ) {
					GUI.skin = this.Skin;
				}

				if ( !PhotonNetwork.connected ) {
					if ( PhotonNetwork.connecting ) {
						GUILayout.Label ( "Connecting to: " + PhotonNetwork.ServerAddress );
					}
					else {
						GUILayout.Label ( "Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress );
					}

					if ( this.connectFailed ) {
						GUILayout.Label ( "Connection failed. Check setup and use Setup Wizard to fix configuration." );
						GUILayout.Label ( String.Format ( "Server: {0}", new object[] { PhotonNetwork.ServerAddress } ) );
						GUILayout.Label ( "AppId: " + PhotonNetwork.PhotonServerSettings.AppID );

						if ( GUILayout.Button ( "Try Again", GUILayout.Width ( 100 ) ) ) {
							this.connectFailed = false;
							PhotonNetwork.ConnectUsingSettings ( "0.9" );
						}
					}

					return;
				}

				Rect content = new Rect ( ( Screen.width - WidthAndHeight.x ) / 2, ( Screen.height - WidthAndHeight.y ) / 2, WidthAndHeight.x, WidthAndHeight.y );
				GUI.Box ( content, "Create Room" );

				GUILayout.BeginArea ( content );
				GUILayout.Space ( 40 );

				// Join room by title
				GUILayout.BeginHorizontal ( );
				GUILayout.Label ( "Roomname:", GUILayout.Width ( 150 ) );

				this.roomName = GUILayout.TextField ( this.roomName );

				if ( GUILayout.Button ( "Create Room", GUILayout.Width ( 150 ) ) ) {
					PhotonNetwork.CreateRoom ( this.roomName, new RoomOptions ( ) { maxPlayers = 5 }, null );
				}

				GUILayout.EndHorizontal ( );

				GUILayout.EndArea ( );
			}
		}


		// We have two options here: we either joined(by title, list or random) or created a room.
		public void OnJoinedRoom ( ) {
			Log.i ( "OnJoinedRoom" );
		}


		public void OnPhotonCreateRoomFailed ( ) {
			this.ErrorDialog = "Error: Can't create room (room name maybe already used).";
			Log.i ( "OnPhotonCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name." );
            Application.LoadLevel ( "ConfigServer-Scene" );
		}


		public void OnPhotonJoinRoomFailed ( ) {
			this.ErrorDialog = "Error: Can't join room (full or unknown room name).";
			Log.i ( "OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed." );
            Application.LoadLevel ( "ConfigServer-Scene" );
		}


		public void OnPhotonRandomJoinFailed ( ) {
			this.ErrorDialog = "Error: Can't join random room (none found).";
			Log.i ( "OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms." );
		}


		public void OnCreatedRoom ( ) {
			Log.i ( "OnCreatedRoom" );
			PhotonNetwork.LoadLevel ( SceneNameGame );
		}


		public void OnDisconnectedFromPhoton ( ) {
			Log.i ( "Disconnected from Photon." );
            Application.LoadLevel ( "ConfigServer-Scene" );
		}


		public void OnFailedToConnectToPhoton ( object parameters ) {
			this.connectFailed = true;
			Log.i ( "OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress );
            Application.LoadLevel ( "ConfigServer-Scene" );
		}


		public void OnConnectedToPhoton ( ) {
			Log.i ( "Connected to Photon." );
			PhotonNetwork.CreateRoom ( this.roomName, new RoomOptions ( ) { maxPlayers = 5 }, null );
		}
	}
}
