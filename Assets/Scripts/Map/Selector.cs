namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class Selector : MonoBehaviour {

		public GameObject[] rings;

		private Planet[] _planets;


		void Start ( ) {
			_planets = GetComponentsInChildren<Planet> ( );

			foreach ( GameObject ring in rings ) {
				ring.transform.localScale = Vector3.zero;
			}
		}


		public void Init ( ) {
			int i = 0;
			foreach ( GameObject ring in rings ) {
				iTween.ScaleTo ( ring, iTween.Hash (
					"delay", i * .2f,
					"time", 1f,
					"scale", Vector3.one,
					"easetype", iTween.EaseType.easeInOutExpo ) );

				i++;
			}

			Invoke ( "showPlanets", 1f );
		}


		private void showPlanets ( ) {
			foreach ( Planet planet in _planets ) {
				planet.show ( );
			}
		}

	}
}
