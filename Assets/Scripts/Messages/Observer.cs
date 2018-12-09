namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Xml;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;

	public class Observer : Photon.MonoBehaviour {

		//public float			_messagesFrequency; // messages / minute
		//public GameObject		_messagePrefab;

		private List<Player>	_players;

		private int				_lastSelectedPlayer;
		private int				_lastSelectedObj;
		private float			_lastMessageTime;


        #region Singleton Stuff
        private static Observer 		_instance		= null;
        private static readonly object	singletonLock	= new object ( );
        #endregion


        public static Observer instance {
            get {
                lock ( singletonLock ) {
                    if ( _instance == null ) {
                        _instance = ( Observer ) GameObject.Find ( "Observer" ).GetComponent<Observer> ( );

                        DontDestroyOnLoad ( _instance );
                    }
                    return _instance;
                }
            }
        }


		void Start ( ) {
			_lastMessageTime = Time.time;
		}


        public Player selectTarget ( ) {
            updatePlayerList ( );

            if ( _players != null ) {
                int rand = ( int ) Mathf.Floor ( _players.Count * Random.value );
                //rand = rand == _lastSelectedPlayer ? -1 : rand;
                _lastSelectedPlayer = rand;

                return _players[rand];
            } else {
                return null;
            }
        }


		private void updatePlayerList ( ) {
			_players = GameObject.FindObjectsOfType<Player> ( ).ToList<Player> ( );
		}


		public void OnPhotonPlayerConnected ( PhotonPlayer player ) {
			Invoke ( "updatePlayerList", 1f );
		}


		public void OnPhotonPlayerDisconnected ( PhotonPlayer player ) {
			Invoke ( "updatePlayerList", 1f );
		}


        /*** [DEPRECATED] ***
        void Update ( ) {
            if ( Time.time - _lastMessageTime > 60 / _messagesFrequency ) {
                sendMessage ( );
            }
        }


		private void sendMessage ( ) {
			int		idx = selectTarget ( );
			string	msg = selectMessage ( );

			if ( idx != -1 ) {
				Log.i ( msg );
				_players[idx].photonView.RPC (
					"onNewMessage",
					PhotonTargets.Others,
					new object[] { msg } );
			}
			else {
				onNewMessage ( msg );
			}

			_lastMessageTime = Time.time;
		}


		private void onNewMessage ( string msg ) {
			TemporaryMessage newMsg = GameObject.Instantiate ( _messagePrefab ).GetComponent<TemporaryMessage> ( );
			newMsg.transform.parent = GameObject.Find ( "Canvas" ).transform;
			newMsg.displayMessage ( msg );
		}


		private float nextTimer ( ) {
			return 0f;
		}
         * 

		private string selectMessage ( ) {
			XmlDocument xml = XMLManager.loadXML ( AppDataModel.OBJECTIVES_DESCRIPTOR );

			int rand;
			do {
				rand = ( int ) Mathf.Floor ( Random.value * xml.FirstChild.ChildNodes.Count );
			} while ( rand == _lastSelectedObj );
			_lastSelectedObj = rand;

			XmlNode objNode = xml.FirstChild.ChildNodes[rand];
		
			string message	= objNode.SelectSingleNode ( "message" ).InnerText;

			return message;
		}
        */
	}
}