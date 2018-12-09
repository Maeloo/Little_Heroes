namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Xml;

	public class GameClient : MonoBehaviour {

        public bool DEBUG;

		#region Singleton Stuff
		private static GameClient		_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion

		public Text Name;
		public Text Ping;
		public Text EnemiesK;
		public Text BossK;
		public Text GeneratorD;

		private Avatar _myAvatar;
		public	Avatar myAvatar {
			get { return _myAvatar; }
		}

		private uint _generatorD;


		public static GameClient i {
			get { return instance; }
		}

		public static GameClient instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( GameClient ) GameObject.Find ( "LocalGame" ).GetComponent<GameClient> ( );

						DontDestroyOnLoad ( _instance );
					}
					return _instance;
				}
			}
		}


		void Start ( ) {
			_generatorD = 0;

            if ( !DEBUG ) {
                Name.gameObject.SetActive ( false );
                Ping.gameObject.SetActive ( false );
                EnemiesK.gameObject.SetActive ( false );
                BossK.gameObject.SetActive ( false );
                GeneratorD.gameObject.SetActive ( false );
            }

			if ( PhotonNetwork.connected ) {
				XmlDocument doc = XMLManager.loadXML ( AppDataModel.AVATAR_DESCRIPTOR );
				GameObject 	go 	= PhotonNetwork.Instantiate ( "avatar", Spaceship.i.transform.position, Quaternion.identity, 0 );

				_myAvatar = go.GetComponent<Avatar> ( );

                if ( PlayerPrefs.GetInt ( "created" ) == 0 ) {
                    _myAvatar.createFromXML ( doc );
                } else {
                    _myAvatar.createFromPlayerPref ( );
                }

				go.transform.localScale = new Vector3 ( .1f, .1f, 1f );

                GamePopup.instance.initDisplayPopup ( "DECOLLAGE", "Activation des moteurs hyper-luminiques.", 2.4f );                
			}
		}


		void Update ( ) {
			if ( DEBUG & PhotonNetwork.connected ) {
				Ping.text = "Latence " + PhotonNetwork.GetPing ( ) + " ms";
			}
		}


		public void displayCharacter ( bool display ) {
			foreach ( Player player in FindObjectsOfType<Player> ( ) ) {
				player.showPlayer ( display );
			}
		}


		public void updateEnemiesKilled ( int count ) {
            if ( DEBUG )
                EnemiesK.text = "Enemies killed : " + count;
		}


		public void updateBossKilled ( int count ) {
            if ( DEBUG )
                BossK.text = "Boss killed : " + count;
		}


		public void updateGeneratorDestroyed ( ) {
            if ( DEBUG )
                GeneratorD.text = "Generator : " + ++_generatorD + " / 4";
		}

	}
}
