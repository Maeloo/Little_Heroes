namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Eyes : MonoBehaviour {

		private Vector2 	_blinkCountPerMinuteRange = new Vector2(7f, 21f);
		private float 		_lastBlink;
		private float 		_nextBlink;
		private GameObject 	_leftEye;
		private GameObject 	_rightEye;	


		// Use this for initialization
		void Start () {
			_lastBlink = Time.time;

			setNextBlinkTime ();
		}
		

		// Update is called once per frame
		void Update () {
			if (Time.time - _lastBlink > _nextBlink) {
				blink(_leftEye);
				blink(_rightEye);

				setNextBlinkTime();
			}
		}


		public void registerLeftEye(GameObject go) {
			_leftEye = go;
		}


		public void registerRightEye(GameObject go) {
			_rightEye = go;
		}


		private void blink(GameObject eye) {
			_lastBlink = Time.time;

			iTween.ScaleTo (eye, iTween.Hash (
				"y", .1f,
				"time", .15f,
				"easetype", iTween.EaseType.easeInOutExpo
			));

			iTween.ScaleTo (eye, iTween.Hash (
				"delay", .15f,
				"y", 1f,
				"time", .15f,
				"easetype", iTween.EaseType.easeInOutExpo
			));
		}


		private void setNextBlinkTime() {
			_nextBlink = .3f + 60f / Random.Range (_blinkCountPerMinuteRange.x, _blinkCountPerMinuteRange.y);
		}

	}
}
