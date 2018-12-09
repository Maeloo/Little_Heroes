using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MunitionManager : MonoBehaviour {

	#region Singleton Stuff
	private static MunitionManager	_instance		= null;
	private static readonly object	singletonLock	= new object ( );
	#endregion

	public float PoolSize;

	[SerializeField]	private Transform	_ammoStock;
	[SerializeField]	private GameObject	_bulletPrefab;
	[SerializeField]	private GameObject	_laserPrefab;

	private List<GameObject> _bulletPool;
	private List<GameObject> _laserPool;


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

		_bulletPool = new List<GameObject> ( );
		_laserPool	= new List<GameObject> ( );
		for ( int i = 0; i < PoolSize; ++i ) {
			GameObject bullet		= ( GameObject ) Instantiate ( _bulletPrefab );
			bullet.transform.parent = _ammoStock;
			bullet.SetActive ( false );

			_bulletPool.Add ( bullet );

			GameObject laser		= ( GameObject ) Instantiate ( _laserPrefab );
			laser.transform.parent = _ammoStock;
			laser.SetActive ( false );

			_laserPool.Add ( laser );
		}
	}


	public List<GameObject> getBullets ( ) {
		return _bulletPool;
	}


	public List<GameObject> getLasers ( ) {
		return _laserPool;
	}

}
