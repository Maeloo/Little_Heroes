namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Light : Module {

		[SerializeField]
		private GameObject _light;


		void Start ( ) {
			_light.SetActive ( false );
		}


		public override void onPlayerInput ( Vector3 inputPosition ) {
			base.onPlayerInput ( inputPosition );

			_light.transform.rotation = Quaternion.LookRotation ( Vector3.forward, inputPosition - transform.position );
		}


		public override void onPlayerInputEnd ( ) {
			_light.transform.localRotation = Quaternion.identity;
		}


		public override void notifyTakeover ( ) {
			_light.SetActive ( true );

			gameObject.GetPhotonView ( ).RPC (
				"activeLight",
				PhotonTargets.Others,
				new object[] { true } );
		}


		public override void notifyRelease ( ) {
			_light.SetActive ( false );

			gameObject.GetPhotonView ( ).RPC (
				"activeLight",
				PhotonTargets.Others,
				new object[] { false } );
		}


		[RPC]
		public void activeLight ( bool active ) {
			_light.SetActive ( active );
		}

	}
}
