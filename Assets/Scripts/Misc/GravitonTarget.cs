namespace LittleHeroes {
	using UnityEngine;

	public class GravitonTarget : Photon.MonoBehaviour {

		private Vector3		_realPosition;
		private Quaternion	_realRotation;

		private bool _locked;
		public	bool  locked {
			get { return _locked; }
		}
		

		void Start ( ) {
			_locked = false;

			_realPosition = transform.position;
			_realRotation = transform.rotation;
		}

		
		public void lock_ ( Transform anchor ) {
			transform.parent		= anchor;
			transform.localPosition	= Vector3.zero;
			GetComponent<Rigidbody2D>().velocity	= Vector2.zero;
			GetComponent<Rigidbody2D>().isKinematic = true;

			_locked = true;
		}


		public void unlock_ ( ) {
			_locked = false;

			transform.parent		= null;
			GetComponent<Rigidbody2D>().isKinematic = false;
		}


		private void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) { 
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.rotation );
				stream.SendNext ( transform.position );
			}
			else {
				//Network player, receive data
				_realRotation = ( Quaternion ) stream.ReceiveNext ( );
				_realPosition = ( Vector3 ) stream.ReceiveNext ( );
			}
		}


		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime * AppDataModel.NETWORK_LERP );
				transform.position = Vector3.Lerp ( transform.position, _realPosition, Time.deltaTime * AppDataModel.NETWORK_LERP );
			}
		}


		[RPC]
		public void onTargetShot ( Vector3 force ) {
			photonView.RequestOwnership ( );
			GetComponent<Rigidbody2D>().AddForce ( force );
		}


	}
}
