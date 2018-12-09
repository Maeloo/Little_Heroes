namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class MunitionManager : MonoBehaviour {

		#region Singleton Stuff
		private static MunitionManager	_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion

		public float PoolSize;

		[SerializeField] private Transform	_ammoStock;

		[SerializeField] private GameObject	_iceBulletPrefab;
		[SerializeField] private GameObject	_iceLaserPrefab;

		[SerializeField] private GameObject	_plasmaBulletPrefab;
		[SerializeField] private GameObject	_plasmaLaserPrefab;

		[SerializeField] private GameObject	_elecBulletPrefab;
		[SerializeField] private GameObject	_elecLaserPrefab;

		[SerializeField] private GameObject	_forceBulletPrefab;
		[SerializeField] private GameObject	_forceLaserPrefab;

		private List<GameObject> _iceBulletPool;
		private List<GameObject> _iceLaserPool;

		private List<GameObject> _forceBulletPool;
		private List<GameObject> _forceLaserPool;

		private List<GameObject> _plasmaBulletPool;
		private List<GameObject> _plasmaLaserPool;

		private List<GameObject> _elecBulletPool;
		private List<GameObject> _elecLaserPool;


		public static MunitionManager i {
			get { return instance; }
		}

		public static MunitionManager instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( MunitionManager ) GameObject.Find ( "bullets_stock" ).GetComponent<MunitionManager> ( );

						DontDestroyOnLoad ( _instance );
					}
					return _instance;
				}
			}
		}


		void Start ( ) {
			Init ( );
		}


		private void Init ( ) {
			Log.i ( "Initialization" );

			_iceBulletPool		= new List<GameObject> ( );
			_iceLaserPool		= new List<GameObject> ( );

			_forceBulletPool	= new List<GameObject> ( );
			_forceLaserPool		= new List<GameObject> ( );

			_plasmaBulletPool	= new List<GameObject> ( );
			_plasmaLaserPool	= new List<GameObject> ( );

			_elecBulletPool		= new List<GameObject> ( );
			_elecLaserPool		= new List<GameObject> ( );

			for ( int i = 0; i < PoolSize; ++i ) {
				GameObject iceBullet		= ( GameObject ) Instantiate ( _iceBulletPrefab );
				iceBullet.transform.parent	= _ammoStock;
				iceBullet.SetActive ( false );

				_iceBulletPool.Add ( iceBullet );

				GameObject iceLaser			= ( GameObject ) Instantiate ( _iceLaserPrefab );
				iceLaser.transform.parent	= _ammoStock;
				iceLaser.SetActive ( false );

				_iceLaserPool.Add ( iceLaser );

				/******/

				GameObject forceBullet			= ( GameObject ) Instantiate ( _forceBulletPrefab );
				forceBullet.transform.parent	= _ammoStock;
				forceBullet.SetActive ( false );

				_forceBulletPool.Add ( forceBullet );

				GameObject forceLaser		= ( GameObject ) Instantiate ( _forceLaserPrefab );
				forceLaser.transform.parent = _ammoStock;
				forceLaser.SetActive ( false );

				_forceLaserPool.Add ( forceLaser );

				/******/

				GameObject plasmaBullet			= ( GameObject ) Instantiate ( _plasmaBulletPrefab );
				plasmaBullet.transform.parent = _ammoStock;
				plasmaBullet.SetActive ( false );

				_plasmaBulletPool.Add ( plasmaBullet );

				GameObject plasmaLaser		= ( GameObject ) Instantiate ( _plasmaLaserPrefab );
				plasmaLaser.transform.parent = _ammoStock;
				plasmaLaser.SetActive ( false );

				_plasmaLaserPool.Add ( plasmaLaser );

				/******/

				GameObject elecBullet			= ( GameObject ) Instantiate ( _elecBulletPrefab );
				elecBullet.transform.parent = _ammoStock;
				elecBullet.SetActive ( false );

				_elecBulletPool.Add ( elecBullet );

				GameObject elecLaser		= ( GameObject ) Instantiate ( _elecLaserPrefab );
				elecLaser.transform.parent = _ammoStock;
				elecLaser.SetActive ( false );

                _elecLaserPool.Add ( elecLaser );
			}
		}


		public List<GameObject> getBullets ( BulletType type ) {
			switch ( type ) { 
				case BulletType.ELEC:
					return _elecBulletPool;
					break;

				case BulletType.ICE:
					return _iceBulletPool;
					break;

				case BulletType.FORCE:
					return _forceBulletPool;
					break;

				case BulletType.PLASMA:
					return _plasmaBulletPool;
					break;
			}

			return null;
		}


		public List<GameObject> getLasers ( BulletType type ) {
			switch ( type ) {
				case BulletType.ELEC:
					return _elecLaserPool;
					break;

				case BulletType.ICE:
					return _iceLaserPool;
					break;

				case BulletType.FORCE:
					return _forceLaserPool;
					break;

				case BulletType.PLASMA:
					return _plasmaLaserPool;
					break;
			}

			return null;
		}

	}
}
