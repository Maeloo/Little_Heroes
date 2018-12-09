namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class EnemyBullet : MonoBehaviour {

		public float Damage;
		public GameObject _hitParticle;


		public void destroy ( bool showHit ) {
			if ( showHit )
				GameObject.Instantiate ( _hitParticle, transform.position, Quaternion.identity );

			gameObject.SetActive ( false );
		}


		private float		_lastTest;
		private Renderer	_renderer;


		void Start ( ) {
			_lastTest = Time.time;
			_renderer = GetComponent<Renderer> ( );
		}


		void Update ( ) {
			if ( Time.time - _lastTest > 500 ) {
				_lastTest = Time.time;

				// TODO: change renderer to sprite
				if ( !GeometryUtility.TestPlanesAABB ( GeometryUtility.CalculateFrustumPlanes ( Camera.main ), _renderer.bounds ) )
					destroy ( false );
			}
		}

	}
}
