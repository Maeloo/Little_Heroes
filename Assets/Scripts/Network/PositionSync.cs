namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class PositionSync : Photon.MonoBehaviour {

		protected Vector3 _realPosition;


		protected void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.position );
			}
			else {
				//Network player, receive data
				_realPosition = ( Vector3 ) stream.ReceiveNext ( );
			}
		}


		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.position = Vector3.Lerp ( transform.position, _realPosition, Time.deltaTime * AppDataModel.NETWORK_LERP );
			}
		}
	}
}
