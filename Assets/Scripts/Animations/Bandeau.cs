namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class Bandeau : MonoBehaviour {

		public float yScale;

		private Image self;


		void Start ( ) {
			self = GetComponent<Image> ( );
			self.fillAmount = .11f;

			transform.localScale = new Vector3 ( 0f, yScale, 1f );
		}

		
		public float display ( ) {
			Vector3 newScale = new Vector3 ( 1f, yScale, 1f );

			iTween.ScaleTo ( gameObject, newScale, .3f );
			iTween.ValueTo ( gameObject, iTween.Hash ( 
				"delay", .3f,
				"time", .3f,
				"from", .11f,
				"to", 1f,
				"easetype", iTween.EaseType.easeInExpo,
				"onupdate", "updateFilledValue"
				) );

            return .6f;
		}


		public float hide ( ) {
			Vector3 newScale = new Vector3 ( 0f, yScale, 1f );

			iTween.ValueTo ( gameObject, iTween.Hash (
				"time", .3f,
				"from", 1f,
				"to", .11f,
				"easetype", iTween.EaseType.easeInExpo,
				"onupdate", "updateFilledValue"
				) );

			iTween.ScaleTo ( gameObject, iTween.Hash (
				"scale", newScale,
				"delay", .3f,
				"time", .3f
				) );

            return .6f;
		}


		public void updateFilledValue ( float value ) {
			self.fillAmount = value;
		}

	}
}
