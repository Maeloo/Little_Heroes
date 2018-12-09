namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;

	public class Lobby : Photon.MonoBehaviour {

		public Transform	mainCamera;
		public Text			roomName;
		public Text			readyText;
		public Selector		selec;
		public GameObject	nameContainer;
		public GameObject	avatarPrefab;
		public GameObject	tabletObjects;
		public GameObject	phoneObjects;
		public Vector3[]	playerSlots;
		public Text[]		playerNames;
		public Popup		popup;
        public UITweener[]  tweensToReverse;
        public GameObject[] goToHide;
		

		private GameObject[]	        _players;
		private bool[]				    _playersStatus;
        private bool[]				    _playersPresence;
        private Dictionary<int, int>    _playersPosition;
        private int                     _playersCount;

		private Vector3				    _startPos = new Vector3 ( 20f, -3f, 10f );
		
		private Avatar				    _localPlayer;
        private int                     _localIdx;
		private bool				    _localStatus;
		private bool				    _waiting;

		void Start ( ) {
            _players            = new GameObject[4];
			_playersStatus	    = new bool[] { false, false, false, false };
            _playersPresence    = new bool[] { false, false, false, false };
            _playersPosition    = new Dictionary<int, int> ( );
            _playersCount       = 0;            

			phoneObjects.SetActive ( false );
			tabletObjects.SetActive ( false );

			_waiting = true;

			foreach ( Text txt in playerNames ) {
				txt.text = "";
                txt.gameObject.SetActive ( false );
			}

            if ( PhotonNetwork.room != null ) {
                if ( !PhotonNetwork.isMasterClient ) {
                    clientStart ( );

                    phoneObjects.SetActive ( true );
                } else {
                    tabletObjects.SetActive ( true );
                }

                Loader.getInstance ( ).showLoader ( false );
            }
		}


		void Update ( ) {
			if ( arePlayersReady ( ) && PhotonNetwork.isMasterClient && _waiting ) {
				_waiting = false;

				iTween.ScaleTo ( roomName.gameObject, iTween.Hash (
					"time", .6f,
					"scale", Vector3.zero,
					"easetype", iTween.EaseType.easeInOutExpo ) );

				iTween.ScaleTo ( nameContainer, iTween.Hash (
					"delay", .2f,
					"time", .6f,
					"scale", Vector3.zero,
					"easetype", iTween.EaseType.easeInOutExpo ) );

                for ( int i = 0; i < 4; ++i ) {
                    if ( _players[i] != null )
                        _players[i].GetComponent<Avatar> ( ).fadeOut ( );
                }

				Invoke ( "goToMap", 1f );
			}
		}


		private void goToMap ( ) {
			selec.Init ( );

            foreach ( UITweener tween in tweensToReverse ) {
                tween.PlayReverse ( );
            }

            foreach ( GameObject go in goToHide ) {
                go.SetActive ( false );
            }

			photonView.RPC (
					"startSelec",
					PhotonTargets.Others,
					new object[] {  } );
		}


		public void OnJoinedRoom ( ) {
			Log.i ( "OnJoinedRoom " );
			roomName.text = PhotonNetwork.room.name;

            //if ( !PhotonNetwork.isMasterClient ) {
            //    clientStart ( );

            //    phoneObjects.SetActive ( true );
            //}
            //else {
            //    tabletObjects.SetActive ( true );
            //}
		}


        public void OnPhotonPlayerDisconnected ( PhotonPlayer player ) {
            Debug.Log ( "Player Disconnected " + player.name );

            if ( PhotonNetwork.isMasterClient ) {
                int idx = _playersPosition[player.ID];

                _playersPresence[idx] = false;

                playerNames[idx].gameObject.SetActive ( false );
                playerNames[idx].text = "";

                iTween.MoveTo ( _players[idx], iTween.Hash (
                    "islocal", true,
                    "x", playerSlots[idx].x - 100,
                    "time", 2f,
                    "easetype", iTween.EaseType.easeInOutQuint ) );

                _playersCount--;

                _players[idx] = null;

                List<string> names = new List<string> ( );

                for ( int i = 0; i < playerNames.Length; ++i ) {
                    if ( !string.IsNullOrEmpty ( playerNames[i].text ) ) {
                        names.Add ( playerNames[i].text );
                    }
                }

                photonView.RPC (
                    "onPlayerChange",
                    PhotonTargets.Others,
                    new object[] { -1, -1, names.ToArray ( ) } );
            }
        }


		private void clientStart ( ) {
			XmlDocument doc = XMLManager.loadXML ( AppDataModel.AVATAR_DESCRIPTOR );
			GameObject 	go 	= Instantiate<GameObject> ( avatarPrefab );

			_localPlayer = go.GetComponent<Avatar> ( );

			go.GetComponent<Player> ( ).enabled = false;

            if ( PlayerPrefs.GetInt ( "created" ) == 0 ) {
                go.GetComponent<Avatar> ( ).createFromXML ( doc );
            } else {
                go.GetComponent<Avatar> ( ).createFromPlayerPref ( );
            }

			photonView.RPC (
					"newPlayerArrived",
					PhotonTargets.Others,
                    new object[] { go.GetComponent<Avatar> ( ).Descriptor, PhotonNetwork.player.ID, go.GetComponent<Avatar> ( ).playerName } );

            Invoke ( "LoadComplete", .3f );
		}


        private void LoadComplete ( ) {
            Loader.getInstance ( ).showLoader ( false );
        }


		public void onPlayerReady ( ) {
			_localStatus = !_localStatus;

			readyText.text = _localStatus ? "PRÊT !" : "PRÊT ?";

			photonView.RPC (
					"playerReady",
					PhotonTargets.Others,
					new object[] { _localStatus, _localIdx} );
		}


		private bool arePlayersReady ( ) {
			int rdy     = 0;

			for ( int i = 0; i < _playersStatus.Length; ++i ) {
				if ( _playersStatus[i] )
					rdy++;
			}

            return _playersCount > 0 && _playersCount == rdy;
		}


        public void onLoadLevel ( ) {
            photonView.RPC (
                "onLoading",
                PhotonTargets.Others,
                new object[] { } );
        }


        [RPC]
        public void onLoading ( ) {
            Loader.getInstance ( ).showLoader ( true );
        }


		[RPC]
		public void startSelec ( ) {
			Log.i ( "all players ready" );
            popup.initDisplayPopup ( "En avant !", "Sélectionne une planète sur la carte.", -1f );
		}


        /*[RPC]
        public void playerReady ( bool ready, string playerName ) {
            for ( int i = 0; i < 4; ++i ) {
                if ( _players[i] != null && _players[i].GetComponent<Avatar> ( ).playerName == playerName ) {
                    Log.i ( playerName + " ready." );
                    _playersStatus[i] = ready;
                }
            }
        }*/


        [RPC]
        public void playerReady ( bool ready, int idx ) {
            _playersStatus[idx] = ready;
        }


        [RPC]
        public void onPlayerChange ( int playerID, int idx, string[] names ) {
            int i = 0;
            foreach ( string name in names ) {
                playerNames[i].text = name;
                playerNames[i].gameObject.SetActive ( true );
                i++;
            }

            for ( ; i < playerNames.Length; ++i ) {
                playerNames[i].gameObject.SetActive ( false );
            }

            if ( PhotonNetwork.player.ID == playerID ) {
                _localIdx = idx;
            }
        }


        [RPC]
        public void newPlayerArrived ( Dictionary<string, string> descriptor, int id, string name ) {
            if ( !PhotonNetwork.isMasterClient ) {
                popup.initDisplayPopup ( "NOUVEL ÉQUIPIER", name + " vient de s'ammarer au vaisseau mère.\nConsulte la tablette pour plus de détails." );
            } else {

                int idx = 0;
                while ( _playersPresence[idx] ) {
                    idx++;
                }

                _playersPosition[id] = idx;

                GameObject newPlayer = Instantiate<GameObject> ( avatarPrefab );

                newPlayer.GetComponent<Player> ( ).enabled = false;
                newPlayer.GetComponent<Avatar> ( ).createFromDictionnary ( descriptor );
                newPlayer.transform.localPosition = _startPos;
                newPlayer.transform.parent = mainCamera;

                _playersPresence[idx] = true;
                _players[idx] = newPlayer;

                _playersCount++;

                iTween.MoveTo ( newPlayer, iTween.Hash (
                    "islocal", true,
                    "x", playerSlots[idx].x,
                    "time", 2f,
                    "easetype", iTween.EaseType.easeInOutQuint ) );

                playerNames[idx].gameObject.SetActive ( true );
                playerNames[idx].text = name;

                List<string> names = new List<string> ( );

                for ( int i = 0; i < playerNames.Length; ++i ) {
                    if ( !string.IsNullOrEmpty ( playerNames[i].text ) ) {
                        names.Add ( playerNames[i].text );
                    }
                }

                photonView.RPC (
                    "onPlayerChange",
                    PhotonTargets.Others,
                    new object[] { id, idx, names.ToArray ( ) } );
            }
        }
	}
}
