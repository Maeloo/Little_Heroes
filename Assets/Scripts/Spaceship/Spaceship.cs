// TODO : Synchroniser vie
namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Spaceship : Photon.MonoBehaviour {

		#region Singleton Stuff
		private static Spaceship		_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion

        public float        _maxLife;
        public float        _regen;

		public GameObject	hull;
        public GameObject   prefabFlash;
        public GameObject[] damages;
		public GameObject[] modules;
        public GameObject[] modules_zoom;

		private Reactor		_reactor;
		//private Cannon		_cannon;
		private GameObject	_controllers;

		private Vector3		_realPosition;
		private Quaternion	_realRotation;

        private float       _life;
        private bool        _gameOver;

        public Transform[]  _generators;


		public static Spaceship i {
			get { return instance; }
		}

		public static Spaceship instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( Spaceship ) GameObject.Find ( "_spaceship" ).GetComponent<Spaceship> ( );
					}
					return _instance;
				}
			}
		}


		private void Start ( ) {
			Init ( );
		}


		private void Init ( ) {
			Log.i ( "Initlization" );

			_reactor = transform.FindChild ( "modules/module_reactor" ).GetComponent<Reactor> ( );
			//_cannon			= transform.FindChild ( "modules/module_cannon" ).GetComponent<Cannon> ( );
			_controllers	= transform.FindChild ( "controllers" ).gameObject;
			_realPosition	= transform.position;
			_realRotation	= transform.rotation;
            _life           = _maxLife;

            for ( int i = 0; i < damages.Length; ++i ) {
                damages[i].SetActive ( false );
            }
		}


		void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.position );
				stream.SendNext ( transform.rotation );
			}
			else {
				//Network player, receive data
				_realPosition = ( Vector3 ) stream.ReceiveNext ( );
				_realRotation = ( Quaternion ) stream.ReceiveNext ( );
			}
		}


		private void Update ( ) {
			if ( photonView.isMine ) {
				GetComponent<Rigidbody2D>().velocity = _reactor.getPropulsion ( );
			}
			else {
				transform.position = Vector3.Lerp ( transform.position, _realPosition, Time.deltaTime * AppDataModel.NETWORK_LERP );
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime * AppDataModel.NETWORK_LERP );
			}

            updateLife ( );
		}


        private void updateLife ( ) {
            if ( !_gameOver ) {
                _life += _regen;
                _life = _life > _maxLife ? _maxLife : _life;

                if ( PhotonNetwork.isMasterClient && GameMaster.instance != null ) {
                    GameMaster.instance.onLifeChange ( _life / _maxLife );
                }
            }
        }


		private void OnTriggerEnter2D ( Collider2D col ) {
			EnemyBullet eb = col.gameObject.GetComponent<EnemyBullet> ( );

			if ( eb ) {
                _life -= eb.Damage;


				SoundManager.getInstance().playSoundimpactSpaceship();

                bool checkShake = false;
                for ( int i = 0; i < damages.Length; ++i ) {
                    checkShake = damages[i].activeSelf;
                    damages[i].SetActive ( _life < ( damages.Length - i ) * ( _maxLife / ( damages.Length + 1 ) ) );

                    if ( checkShake != damages[i].activeSelf ) {
                        Camera.main.gameObject.GetComponent<CameraShake> ( ).shake = 1f;
                    }
                }

				eb.destroy ( true );
			}

            if ( PhotonNetwork.isMasterClient && GameMaster.instance != null ) {
                GameMaster.instance.onLifeChange ( _life / _maxLife );
            }

            if ( !_gameOver && _life <= 0 ) {
                _life = 0;

                Log.i ( "Game Over. You Loose." );
                onLoose ( );
            }
		}


        private void onLoose ( ) {
            _gameOver = true;

            GameObject.Instantiate ( prefabFlash, transform.position, Quaternion.identity );

            iTween.ScaleTo ( gameObject, Vector3.zero, .3f );

            Invoke ( "endGame", .3f );

            if ( PhotonNetwork.isMasterClient && GameMaster.instance != null ) {
                TabletPopup.instance.initDisplayPopup ( "COURAGE !", "Il reste encore 7 planetes a sauver.", -1f );
            } else {
                GamePopup.instance.initDisplayPopup ( "Vaisseau endommage !", "Vous avez ete teleporte dans un endroit sur.", -1f );
                InputController.i.gameObject.SetActive ( false );
            }
        }

        [RPC]
        public void onWin ( ) {
            _gameOver = true;

            GameObject.Instantiate ( prefabFlash, transform.position, Quaternion.identity );

            iTween.ScaleTo ( gameObject, Vector3.zero, .3f );

            Invoke ( "endGame", .3f );

            if ( PhotonNetwork.isMasterClient && GameMaster.instance != null ) {
                TabletPopup.instance.initDisplayPopup ( "GG !", "blablabla", -1f );
            } else {
                GamePopup.instance.initDisplayPopup ( "GG !", "blablabla", -1f );
                InputController.i.gameObject.SetActive ( false );
            }
        }


        private void endGame ( ) {
            GetComponent<CircleCollider2D> ( ).enabled = false;

            if ( CameraManager.i != null )
                CameraManager.i.blockZoom ( true );
        }


		public void showInside ( bool show ) {
			iTween.FadeTo ( hull, iTween.Hash (
					"alpha", show ? 0f : 1f,
					"time", AppDataModel.ZOOM_TIME ) );

            for ( int i = 0; i < damages.Length; ++i ) {
                if ( damages[i].activeSelf ) {
                    iTween.FadeTo ( damages[i], iTween.Hash (
                    "alpha", show ? 0f : 1f,
                    "time", AppDataModel.ZOOM_TIME ) );
                }
            }

			foreach ( GameObject module in modules ) {
                if ( module != null )
                    iTween.FadeTo ( module, iTween.Hash (
                        "alpha", show ? 0f : 1f,
                        "time", AppDataModel.ZOOM_TIME ) );
			}

            foreach ( GameObject module in modules_zoom ) {
                module.SetActive ( show );
            }

			foreach ( Transform child in _controllers.transform ) {
				SphereCollider sc = child.GetComponent<SphereCollider> ( );

				if ( sc )
					sc.enabled = show;

				/*iTween.FadeTo ( child.gameObject, iTween.Hash (
					"alpha", show ? 1f : 0f,
					"time", AppDataModel.ZOOM_TIME ) );*/
			}
		}


		public void embark ( Transform player ) {
			player.parent = transform;
		}


		public PhotonView getPhotonView ( ) {
			return GetComponent<PhotonView> ( );
		}


		public Vector3 getCurrentPosition ( ) {
			return transform.position;
		}


        public Vector3 closestGenerator ( ) {
            Transform closest = _generators[0];

            foreach ( Transform gen in _generators ) {
                if ( gen.gameObject.activeSelf && Vector3.Distance ( transform.position, gen.position ) < Vector3.Distance ( transform.position, closest.position ) ) {
                    closest = gen;
                }
            }

            return closest.position;
        }

	}
}
