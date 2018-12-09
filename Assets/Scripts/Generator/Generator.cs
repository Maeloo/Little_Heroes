namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Generator : Photon.MonoBehaviour {

		public float _maxLife;
        public float _point = 1000;

        private float _life;

		[SerializeField] private GameObject		_explosionPrefab;
		[SerializeField] private SpriteRenderer _blackSwarm;
        [SerializeField] private GameObject     _damaged;
        [SerializeField] private GameObject     _intermediate;
        [SerializeField] private GameObject     _destroyed;
        [SerializeField] private GameObject     _smoke;

        private bool destroyedSelf;


        void Start ( ) {
            _life           = _maxLife;
            destroyedSelf   = false;
        }


		protected void OnTriggerEnter2D ( Collider2D collider ) {
			PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet> ( );

            if ( !destroyedSelf && bullet ) {
                _life -= bullet.damage;

                if ( _life <= 2 * _maxLife / 3 ) {
                    gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;

                    _damaged.SetActive ( true );
                    _intermediate.SetActive ( false );
                    _destroyed.SetActive ( false );
                }

                if ( _life <= _maxLife / 3 ) {
                    gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;

                    _damaged.SetActive ( false );
                    _intermediate.SetActive ( true );
                    _destroyed.SetActive ( false );
                }

                if ( _life <= 0 ) {
                    gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;

                    _damaged.SetActive ( false );
                    _intermediate.SetActive ( false );
                    _destroyed.SetActive ( true );
                }

				bullet.destroy ( true );

                if ( PhotonNetwork.isMasterClient && _life <= 0 && !destroyedSelf ) {
                    destroyedSelf = true;

					photonView.RPC (
						"onGeneratorDestroyed",
						PhotonTargets.All,
						new object[] { } );
				}
			}
		}


		[RPC]
		public void onGeneratorDestroyed ( ) {
            if ( _explosionPrefab != null )
                GameObject.Instantiate ( _explosionPrefab, transform.position, Quaternion.identity );

			//transform.parent.gameObject.SetActive ( false );
            _smoke.SetActive ( false );

            destroyedSelf = true;

			Color col = _blackSwarm.color;
			col.a -= .125f;
			_blackSwarm.color = col;

			if ( !PhotonNetwork.isMasterClient ) {
				GameClient.i.updateGeneratorDestroyed ( );
            } else {
                GameMaster.instance.onEnemyKilled ( _point );
                GameMaster.instance.onGeneratorDestroyed ( );
            }
		}		
	}
}