namespace LittleHeroes {
	using System;
	using UnityEngine;
	using Random = UnityEngine.Random;

	public class LobbyClient : MonoBehaviour {

		public bool DEBUG;

		public GUISkin Skin;
		public Vector2 WidthAndHeight	= new Vector2 ( 600, 400 );

		private Vector2 scrollPos		= Vector2.zero;

		private bool	connectFailed	= false;

		public static readonly string SceneNameMenu = "MainClient-Scene";

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
				PhotonNetwork.playerName = "Player" + Random.Range ( 1, 9999 );
			}

			// if you wanted more debug out, turn this on:
			// PhotonNetwork.logLevel = NetworkLogLevel.Full;
		}


		public void OnGUI ( ) {
			if ( !DEBUG )
				return;

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
			GUI.Box ( content, "Join Room" );

			GUILayout.BeginArea ( content );
			GUILayout.Space ( 40 );

			// Player name
			GUILayout.BeginHorizontal ( );
			GUILayout.Label ( "Player name:", GUILayout.Width ( 150 ) );

			PhotonNetwork.playerName = GUILayout.TextField ( PhotonNetwork.playerName );

			GUILayout.Space ( 158 );

			if ( GUI.changed ) {
				// Save name
				PlayerPrefs.SetString ( "playerName", PhotonNetwork.playerName );
			}

			GUILayout.EndHorizontal ( );
			GUILayout.Space ( 15 );

			if ( !string.IsNullOrEmpty ( this.ErrorDialog ) ) {
				GUILayout.Label ( this.ErrorDialog );

				if ( timeToClearDialog < Time.time ) {
					timeToClearDialog = 0;
					this.ErrorDialog = "";
				}
			}

			GUILayout.Space ( 15 );

			// Join random room
			GUILayout.BeginHorizontal ( );

			GUILayout.Label ( PhotonNetwork.countOfPlayers + " users are online in " + PhotonNetwork.countOfRooms + " rooms." );
			GUILayout.FlexibleSpace ( );

			if ( GUILayout.Button ( "Join Random", GUILayout.Width ( 150 ) ) ) {
				PhotonNetwork.JoinRandomRoom ( );
			}

			GUILayout.EndHorizontal ( );

			GUILayout.Space ( 15 );

			if ( PhotonNetwork.GetRoomList ( ).Length == 0 ) {
				GUILayout.Label ( "Currently no games are available." );
				GUILayout.Label ( "Rooms will be listed here, when they become available." );
			}
			else {
				GUILayout.Label ( PhotonNetwork.GetRoomList ( ).Length + " rooms available:" );

				// Room listing: simply call GetRoomList: no need to fetch/poll whatever!
				this.scrollPos = GUILayout.BeginScrollView ( this.scrollPos );

				foreach ( RoomInfo roomInfo in PhotonNetwork.GetRoomList ( ) ) {
					GUILayout.BeginHorizontal ( );
					GUILayout.Label ( roomInfo.name + " " + ( roomInfo.playerCount - 1 ) + "/" + ( roomInfo.maxPlayers - 1 ) );

					if ( GUILayout.Button ( "Join", GUILayout.Width ( 150 ) ) ) {
						PhotonNetwork.JoinRoom ( roomInfo.name );
					}

					GUILayout.EndHorizontal ( );
				}

				GUILayout.EndScrollView ( );
			}

			GUILayout.EndArea ( );
		}


		// We have two options here: we either joined(by title, list or random) or created a room.
		public void OnJoinedRoom ( ) {
			Log.i ( "OnJoinedRoom" );
		}


		public void OnPhotonCreateRoomFailed ( ) {
			this.ErrorDialog = "Error: Can't create room (room name maybe already used).";
			Log.i ( "OnPhotonCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name." );
		}


		public void OnPhotonJoinRoomFailed ( ) {
			this.ErrorDialog = "Error: Can't join room (full or unknown room name).";
			Log.i ( "OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed." );
            Application.LoadLevel ( "HomeSolo-Scene" );
		}


		public void OnPhotonRandomJoinFailed ( ) {
			this.ErrorDialog = "Error: Can't join random room (none found).";
			Log.i ( "OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms." );
            Application.LoadLevel ( "HomeSolo-Scene" );
		}


		public void OnDisconnectedFromPhoton ( ) {
			Log.i ( "Disconnected from Photon." );
            Application.LoadLevel ( "HomeSolo-Scene" );
		}


		public void OnConnectedToPhoton ( ) {
			Log.i ( "Connected to Photon." );
			PhotonNetwork.JoinRoom ( AppDataModel.ROOM_NAME );
		}


		public void OnFailedToConnectToPhoton ( object parameters ) {
			this.connectFailed = true;
			Log.i ( "OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress );
            Application.LoadLevel ( "HomeSolo-Scene" );
		}

	}
}