namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class GeneratorShield : Photon.MonoBehaviour {

		public float _life;
		public float _regen; // life point per second

		private float _maxLife;
		private float _lastHit;


		void Start ( ) {
			_maxLife = _life;
			_lastHit = Time.time;
		}


		protected void OnTriggerEnter2D ( Collider2D collider ) {
			PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet> ( );

			if ( bullet ) {
				_life -= bullet.damage;

				bullet.destroy ( true );

				_lastHit = Time.time;

				if ( PhotonNetwork.isMasterClient && _life <= 0 ) {
					photonView.RPC (
						"onShieldDestroyed",
						PhotonTargets.All,
						new object[] { } );
				}
			}
		}


		[RPC]
		public void onShieldDestroyed ( ) {
			gameObject.SetActive ( false );
		}


		void Update ( ) {
			if ( Time.time - _lastHit > .1f ) {
				_life += _regen * Time.deltaTime;
				_life = _life > _maxLife ? _maxLife : _life;
			}
		}
	}
}