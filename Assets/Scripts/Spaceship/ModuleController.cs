namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class ModuleController : Photon.MonoBehaviour {

        public string type;

		[SerializeField] protected Module       _module;
        [SerializeField] protected GameObject	_on;
        [SerializeField] protected GameObject	_off;

		public PhotonView[] _pvToRequest;

		protected int		_currentPlayer;
		protected bool		_isFree;

		protected void Start ( ) {
			Init ( );
		}


		protected void Init ( ) {
			Log.i ( "Initialization" );

			_module.registerController ( this );
			_isFree = true;
            _on.SetActive ( false );
		}


		public void OnPhotonPlayerConnected ( PhotonPlayer newcomer ) {
			Debug.Log ( "Player Connected " + newcomer.name );
			if ( !_isFree ) {
				photonView.RPC (
				"takeControl",
				PhotonTargets.Others,
				new object[] { _currentPlayer } );
			}
		}


		public void OnPhotonPlayerDisconnected ( PhotonPlayer leaver ) {
			Debug.Log ( "Player Disconnected " + leaver.name );
			freeControl ( leaver.ID );
		}


		[RPC]
		public void takeControl ( int idPlayer ) {
			if ( _isFree ) {
				Log.i ( "Player " + idPlayer + " takes control of " + _module.gameObject.name );
				_currentPlayer = idPlayer;
				_isFree = false;

				_module.notifyTakeover ( );

                _on.SetActive ( true );
                _off.SetActive ( false );
			}
		}


		[RPC]
		public void freeControl ( int idPlayer ) {
			if ( !_isFree ) {
				if ( _currentPlayer == idPlayer ) {
					Log.i ( "Player " + idPlayer + "frees control of " + _module.gameObject.name );
					_isFree = true;

					_module.notifyRelease ( );

                    _on.SetActive ( false );
                    _off.SetActive ( true );

					if ( PhotonNetwork.isMasterClient ) {
						_module.gameObject.GetComponent<PhotonView> ( ).RequestOwnership ( );

						for ( int i = 0; i < _pvToRequest.Length; ++i ) {
							_pvToRequest[i].RequestOwnership ( );
						}
					}
				}
			}
		}


		public List<PhotonView> getModuleViews ( ) {
			List<PhotonView> pvs = new List<PhotonView> ( );
			pvs.Add ( _module.photonView );

			for ( int i = 0; i < _pvToRequest.Length; ++i ) {
				pvs.Add ( _pvToRequest[i] );
			}

			return pvs;
		}


		public Vector3 getCurrentPosition ( ) {
			return transform.position;
		}


		public bool isUnoccupied ( ) {
			return _isFree;
		}


		public bool isModuleWorking ( ) {
			return !_isFree;
		}


		public void sendInput ( Vector3 inputPosition ) {
			_module.onPlayerInput ( inputPosition );
		}


		public void sendInputEnd ( ) {
			_module.onPlayerInputEnd ( );
		}

	}
}