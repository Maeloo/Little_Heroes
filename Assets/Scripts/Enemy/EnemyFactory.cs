namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class EnemyFactory : MonoBehaviour {

		public float	MaxEnemies;
		public float	MaxMeteor;
		public Vector2	XRange;
		public Vector2	YRange;

		[SerializeField]
		private GameObject			_planet;

		[SerializeField]
		private string				_enemyPrefab;

		[SerializeField]
		private string				_bossPrefab;

		[SerializeField]
		private string				_meteorPrefab;

		private List<GameObject>	_enemyPool;
		private List<GameObject>	_meteorPool;
		private float				_spawnTime;

		private GameObject			_boss;
		private Boss				_bossComponent;

		private void Start ( ) {
			Init ( );

			_spawnTime = Time.time;
		}


		private void Init ( ) {
			Log.i ( "Initialization" );

			if ( !PhotonNetwork.isMasterClient ) {
				this.enabled = false;
				return;
			}

			_enemyPool = new List<GameObject> ( );
			for ( int i = 0; i < MaxEnemies; ++i ) {
				GameObject enemy				= PhotonNetwork.Instantiate ( _enemyPrefab, transform.position, Quaternion.identity, 0 );
				enemy.transform.parent			= transform;
				enemy.transform.localPosition	= Vector3.zero;
				//enemy.SetActive ( false );

				_enemyPool.Add ( enemy );
			}

			_meteorPool = new List<GameObject> ( );
			for ( int i = 0; i < MaxMeteor; ++i ) {
				GameObject meteor		= PhotonNetwork.Instantiate ( _meteorPrefab, transform.position, Quaternion.identity, 0 );
				meteor.transform.parent = transform;
				meteor.SetActive ( false );

				_meteorPool.Add ( meteor );
			}

			Vector2 bossPos = getRandomCoord ( );
			_boss = PhotonNetwork.Instantiate ( _bossPrefab, bossPos, Quaternion.identity, 0 );
			_boss.transform.position = bossPos;

			_bossComponent = _boss.GetComponentInChildren<Boss> ( );
		}


		private void Update ( ) {
			if ( Time.time - _spawnTime > 10f ) {
				spawnEnemy ( );
			}
		}


		private Vector2 getRandomCoord ( ) {
			bool not_valid;
			Vector2 randCoord;

			do {
				randCoord = MathUtils.randomBetweenRange ( XRange, YRange );
				not_valid = _planet.GetComponent<Renderer>().bounds.Contains ( randCoord );
			} while ( not_valid );

			return randCoord;
		}


		private void spawnEnemy ( ) {
			if ( !_boss.activeSelf ) {
				Vector2 newPos = getRandomCoord ( );

				_boss.transform.position = newPos;

				_bossComponent.gameObject.GetPhotonView ( ).RPC (
							"activeBoss",
							PhotonTargets.All,
							new object[] { newPos } );
			}
			else {
				GameObject enemy;
				int i = 0;
				/*do {
					enemy = _enemyPool[i];
					++i;
				} while ( enemy.activeSelf && i < MaxEnemies );

				_spawnTime = Time.time;
				 
				 if ( i == MaxEnemies )
					return;*/

				_spawnTime = Time.time;
				
				enemy = _enemyPool[i];

				_enemyPool.RemoveAt ( i );
				_enemyPool.Add ( enemy );

				Vector2 newPos = getRandomCoord ( );

				enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
				enemy.transform.position = newPos;

				enemy.GetPhotonView ( ).RPC (
							"activeEnemy",
							PhotonTargets.All,
							new object[] { newPos } );
			}
		}

	}
}
