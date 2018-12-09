namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class SmoothFollow : MonoBehaviour {

		public float		smoothTime	= .3f;

		private Transform	target		= null;

		private Vector2 velocity;
		private Vector3 position;

        private float myZ;

		
        void Start ( ) {
			target  = Spaceship.i.transform;
            myZ     = transform.position.z;
		}


		void Update ( ) {
			if ( target != null ) {
				position = transform.position;
                
                float vz = 0;

				position.x = Mathf.SmoothDamp ( transform.position.x, target.position.x, ref velocity.x, smoothTime );
				position.y = Mathf.SmoothDamp ( transform.position.y, target.position.y, ref velocity.y, smoothTime );
                position.z = Mathf.Lerp ( transform.position.z, myZ, smoothTime );

				transform.position = position;
			}
		}

	}
}
