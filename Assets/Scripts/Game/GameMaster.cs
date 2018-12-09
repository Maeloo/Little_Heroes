namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;

	public class GameMaster : Photon.MonoBehaviour {

		/*[SerializeField]
		private Text[]		_playersText;
		[SerializeField]
		private Text		_playerCountText;
		[SerializeField]
		private Text		_buttonText;*/

        #region Singleton Stuff
        private static GameMaster		_instance		= null;
        private static readonly object	singletonLock	= new object ( );
        #endregion

		[SerializeField] private Text		_roomNameText;
		[SerializeField] private Text		_timerText;
        [SerializeField] private Text		_scoreText;
        [SerializeField] private Text		_generatorText;

        [SerializeField] private Image		_lifeGauge;

		[SerializeField] private GameObject	_spectatorCamera;
		[SerializeField] private GameObject	_interface;

		private float   _timer;

		private bool    _spectatorActive;

        private float   _score;
        private int     _destroyed;


        public static GameMaster instance {
            get {
                lock ( singletonLock ) {
                    if ( _instance == null ) {
                        GameObject instance = GameObject.Find ( "GameMaster" );

                        if ( instance == null )
                            return null;

                        _instance = ( GameMaster ) instance.GetComponent<GameMaster> ( );

                        DontDestroyOnLoad ( _instance );
                    }
                    return _instance;
                }
            }
        }


		public void Awake ( ) {
			_roomNameText.text	= PhotonNetwork.room.name;
			_spectatorActive	= false;
			_timer				= 0;
            _score              = 0;
            _destroyed          = 0;
		}


		void Update ( ) {
			_timer += Time.deltaTime;
			string minutes = Mathf.Floor ( _timer / 60 ).ToString ( "00" );
			string seconds = ( _timer % 60 ).ToString ( "00" );

			_timerText.text = minutes + ":" + seconds;

			/*if ( !PhotonNetwork.isMasterClient ) {
				_playerCountText.text = ( PhotonNetwork.room.playerCount - 1 ).ToString ( );
			}
			
			updatePlayerList ( );*/
		}


		public void OnMasterClientSwitched ( PhotonPlayer player ) {
			Log.i ( "OnMasterClientSwitched: " + player );

			string message;
			InRoomChat chatComponent = GetComponent<InRoomChat> ( );  // if we find a InRoomChat component, we print out a short message

			if ( chatComponent != null ) {
				// to check if this client is the new master...
				if ( player.isLocal ) {
					message = "You are Master Client now.";
				}
				else {
					message = player.name + " is Master Client now.";
				}

				chatComponent.AddLine ( message ); // the Chat method is a RPC. as we don't want to send an RPC and neither create a PhotonMessageInfo, lets call AddLine()
			}
		}


        public void onEnemyKilled ( float score ) {
            _score += score;
            _scoreText.text = _score.ToString ( "000 000" );
        }


        public void onGeneratorDestroyed ( ) {
            _destroyed++;
            _generatorText.text = _destroyed + " / 4";

            if ( _destroyed == 4 ) {
                Spaceship.i.photonView.RPC (
                    "onWin",
                    PhotonTargets.All,
                    new object[] { } );
            }
        }


        public void onLifeChange ( float value ) {
            _lifeGauge.fillAmount = value;
        }


		public void OnLeftRoom ( ) {
			Log.i ( "OnLeftRoom (local)" );
		}


		public void OnDisconnectedFromPhoton ( ) {
			Log.i ( "OnDisconnectedFromPhoton" );
		}


		public void OnPhotonInstantiate ( PhotonMessageInfo info ) {
			Log.i ( "OnPhotonInstantiate " + info.sender );    // you could use this info to store this or react
		}


		public void OnPhotonPlayerConnected ( PhotonPlayer player ) {
			Log.i ( "OnPhotonPlayerConnected: " + player );
		}


		public void OnPhotonPlayerDisconnected ( PhotonPlayer player ) {
			Log.i ( "OnPlayerDisconneced: " + player );
		}


		public void OnFailedToConnectToPhoton ( ) {
			Log.i ( "OnFailedToConnectToPhoton" );

			// back to main menu        
			Application.LoadLevel ( WorkerMenu.SceneNameMenu );
		}


		/*private void updatePlayerList ( ) {

			int i = 0;
			foreach ( PhotonPlayer player in PhotonNetwork.otherPlayers ) {
				_playersText[i].text = player.name;
				++i;
			}

			for ( int j = i; j < 4; ++j ) {
				_playersText[j].text = "EMPTY";
			}
		}


		public void switchMode ( ) {
			_spectatorActive = !_spectatorActive;
			_buttonText.text = _spectatorActive ? "INFOS MODE" : "SPECTATOR MODE";

			_spectatorCamera.SetActive ( _spectatorActive );
			_interface.SetActive ( !_spectatorActive );
		}*/

	}
}
