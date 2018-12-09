namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class PlayerBullet : MonoBehaviour {

		public BulletType	type;
		public float		damage;
		public GameObject	hitParticle;


		public void destroy ( bool showHit ) {
			if ( showHit && hitParticle )
				GameObject.Instantiate ( hitParticle, transform.position, Quaternion.identity );

			gameObject.SetActive ( false );
		}


		private float		_lastTest;
		private Renderer	_renderer;


		void Start ( ) {
			_lastTest = Time.time;
			_renderer = GetComponent<Renderer> ( );
		}


		void Update ( ) {
			if ( Time.time - _lastTest > .5f ) {
				_lastTest = Time.time;

				// TODO: change renderer to sprite
				if ( gameObject.activeSelf && !GeometryUtility.TestPlanesAABB ( GeometryUtility.CalculateFrustumPlanes ( Camera.main ), _renderer.bounds ) )
					destroy ( false );
			}
		}

	}
}
