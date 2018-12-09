namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class RotationSync : Photon.MonoBehaviour {

		protected Quaternion _realRotation;


		protected void OnPhotonSerializeView ( PhotonStream stream, PhotonMessageInfo info ) {
			if ( stream.isWriting ) {
				//We own this player: send the others our data
				stream.SendNext ( transform.rotation );
			}
			else {
				//Network player, receive data
				_realRotation = ( Quaternion ) stream.ReceiveNext ( );
			}
		}


		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime * AppDataModel.NETWORK_LERP );
			}
		}

	}
}