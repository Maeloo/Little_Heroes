namespace LittleHeroes {
    using UnityEngine;
    using System.Xml;
    using System.Collections;
    using System.Collections.Generic;

    public class Player : Photon.MonoBehaviour {

        public GameObject			_messagePrefab;

        private bool				_active;
        private bool                _created;
        private ModuleController	_currentMC;

        private Vector3				_currentPosition;
        private float				_moveTime;

        private Vector3				_realPosition;
        private Quaternion			_realRotation;

        private int					_enemiesKilled;
        private int					_bossKilled;


        void Start ( ) {
            Init ( );
        }


        public void Init ( ) {
            Log.i ( "Initialization" );

            Spaceship.i.embark ( transform );

            ModuleController[] list = FindObjectsOfType<ModuleController> ( );

            if ( photonView.isMine ) {
                for ( int i = 0; i < list.Length; ++i ) {
                    if ( list[i].type == "empty" && list[i].isUnoccupied ( ) ) {
                        transform.position = list[i].transform.position;

                        _currentMC = list[i];
                        _currentMC.gameObject.GetComponent<PhotonView> ( ).RPC (
                            "takeControl",
                            PhotonTargets.All,
                            new object[] { PhotonNetwork.player.ID } );

                        break;
                    }
                }
            }

            _currentPosition    = Vector3.zero;
            _realPosition       = transform.localPosition;
            _realRotation       = transform.rotation;
            _enemiesKilled      = 0;
            _bossKilled         = 0;
            _moveTime           = 0f;

            if ( photonView.isMine ) {
                _created = true;

                photonView.RPC (
                    "onNewPlayer",
                    PhotonTargets.Others,
                    new object[] { transform.localPosition, GameClient.i.myAvatar.Descriptor } );
            }
        }


        void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
            if ( stream.isWriting ) {
                //We own this player: send the others our data
                stream.SendNext ( transform.localPosition );
                stream.SendNext ( transform.rotation );
            } else {
                //Network player, receive data
                _realPosition = ( Vector3 ) stream.ReceiveNext ( );
                _realRotation = ( Quaternion ) stream.ReceiveNext ( );
            }
        }


        private void Update ( ) {
            if ( photonView.isMine ) {
                checkInput ( );

                transform.localPosition = Vector3.Lerp ( transform.localPosition, _currentPosition, Time.deltaTime * _moveTime );
            } else {
                transform.localPosition = Vector3.Lerp ( transform.localPosition, _realPosition, Time.deltaTime );
                transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime );
            }
        }


        private void checkInput ( ) {
            //if ( InputController.i.touchCount > 0 ) {
            if ( InputController.i.touchCount == 1 ) {
                bool sendInputToController = true;

                /** Check le déplacement à l'intérieur du vaisseau **/
                Ray ray = Camera.main.ScreenPointToRay ( InputController.i.primeTouch );

                RaycastHit hit;
                if ( _active && Physics.Raycast ( ray, out hit, 100 ) ) {
                    if ( hit.collider != null ) {
                        ModuleController mc = hit.collider.transform.GetComponent<ModuleController> ( );

                        if ( mc && mc.isUnoccupied ( ) ) {

                            if ( _currentMC != null ) {
                                SoundManager.getInstance ( ).playSoundModule ( false );

                                _currentMC.gameObject.GetComponent<PhotonView> ( ).RPC (
                                    "freeControl",
                                    PhotonTargets.All,
                                    new object[] { PhotonNetwork.player.ID } );
                            }

                            //moveTo ( mc.transform.position );
                            moveTo ( mc.transform.localPosition );

                            _currentMC = mc;

                            sendInputToController = false;
                        }
                    }
                }
                /****************************************************/

                /********** Envoie les infos au controller **********/
                if ( _currentMC && sendInputToController && CameraManager.i.state == CameraManager.ZoomState.OUT ) {
                    _currentMC.sendInput ( InputController.i.primeTouchWorldPosition );
                }
                /****************************************************/

            } else {
                if ( _currentMC )
                    _currentMC.sendInputEnd ( );
            }
        }


        private void moveTo ( Vector3 position ) {
            CancelInvoke ( );

            _currentPosition = position;
            _moveTime = Vector3.Distance ( position, transform.localPosition ) / AppDataModel.PLAYER_MOVE_SPEED;

            Invoke ( "moveComplete", 1f );
        }


        private void moveComplete ( ) {
            SoundManager.getInstance ( ).playSoundModule ( true );

            List<PhotonView> pvs = _currentMC.getModuleViews ( );
            for ( int i = 0; i < pvs.Count; ++i ) {
                pvs[i].RequestOwnership ( );
            }

            if ( _currentMC.gameObject.name == "reactor_controller" ) {
                Spaceship.i.getPhotonView ( ).RequestOwnership ( );
            }

            _currentMC.gameObject.GetComponent<PhotonView> ( ).RPC (
                "takeControl",
                PhotonTargets.All,
                new object[] { PhotonNetwork.player.ID } );
        }


        public void showPlayer ( bool show ) {
            _active = show;

            foreach ( Transform child in transform ) {
                iTween.FadeTo ( child.gameObject, iTween.Hash (
                    "alpha", show ? 1f : 0f,
                    "time", AppDataModel.ZOOM_TIME ) );
            }
        }


        public void OnPhotonPlayerDisconnected ( PhotonPlayer leaver ) {
            foreach ( Player player in FindObjectsOfType<Player> ( ) ) {
                if ( player.photonView.viewID == leaver.ID ) {
                    Destroy ( player.gameObject );
                }
            }
        }


        public bool isLocal ( ) {
            return GetComponent<PhotonView> ( ).isMine;
        }


        [RPC]
        public void onNewPlayer ( Vector3 position, Dictionary<string, string> descriptor ) {
            if ( !PhotonNetwork.isMasterClient )
                CameraManager.i.UpdateZoom ( );

            if ( !photonView.isMine ) {
                gameObject.GetComponent<Avatar> ( ).createFromDictionnary ( descriptor );

                transform.localScale = new Vector3 ( .1f, .1f, 1f );
                transform.localPosition = position;

                _created = true;

                if ( PhotonNetwork.isMasterClient )
                    showPlayer ( false );
            }

            photonView.RPC (
                "updateOtherPlayers",
                PhotonTargets.Others,
                new object[] { transform.localPosition, gameObject.GetComponent<Avatar> ( ).Descriptor } );
        }


        [RPC]
        public void updateOtherPlayers ( Vector3 position, Dictionary<string, string> descriptor ) {
            if ( !PhotonNetwork.isMasterClient && !_created ) {
                _created = true;

                gameObject.GetComponent<Avatar> ( ).createFromDictionnary ( descriptor );
                transform.localScale = new Vector3 ( .1f, .1f, 1f );
            }

            _realPosition           = position;

            transform.localPosition = position;
        }


        [RPC]
        private void onNewMessage ( string title, string msg ) {
            if ( photonView.isMine ) {
                Log.i ( "New Message !" );
                GamePopup.instance.initDisplayPopup ( title, msg, 4f );
            }
        }


        [RPC]
        public void notifyEnemyKilled ( ) {
            if ( photonView.isMine ) {
                _enemiesKilled++;
                GameClient.i.updateEnemiesKilled ( _enemiesKilled );
            }
        }


        [RPC]
        public void notifyBossKilled ( ) {
            if ( photonView.isMine ) {
                _bossKilled++;
                GameClient.i.updateBossKilled ( _bossKilled );
            }
        }

    }
}
