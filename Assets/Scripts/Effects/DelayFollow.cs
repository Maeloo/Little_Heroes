namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class DelayFollow : MonoBehaviour {

		public float Smooth = 15f;

		[SerializeField]
		private Transform Target;

		private float smooth;


		// Start
		void Start ( ) {
			smooth = Smooth + Random.value * Smooth;
		}

		// Update
		void Update ( ) {
			transform.position = Vector3.Lerp ( transform.position, Target.position, Time.deltaTime * smooth );
		}
	}
}