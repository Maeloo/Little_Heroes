namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Planet : MonoBehaviour {

		private Vector3 _initPos;

		private bool _loading;

		void Start ( ) {
			_initPos = transform.localPosition;
			
			Init ( );
		}


		private void Init ( ) {
			transform.localPosition = RandomCircle ( Vector3.zero, 20f );
		}


		void Update ( ) {
			transform.rotation = Quaternion.identity;

			if ( InputController.i.touchCount > 0 ) {
				RaycastHit2D hitInfo = Physics2D.Raycast ( InputController.i.primeTouchWorldPosition, Vector2.zero );
				if ( !_loading && hitInfo ) {
					_loading = true;

                    FindObjectOfType<Lobby> ( ).onLoadLevel ( );

                    Loader.getInstance ( ).showLoader ( true );
					PhotonNetwork.LoadLevel ( "Game-Scene" );
				}
			}
		}


		private Vector3 RandomCircle ( Vector3 center, float radius ) {
			// create random angle between 0 to 360 degrees 
			float ang = Random.value * 360f;
			Vector3 pos;
			pos.x = center.x + radius * Mathf.Sin ( ang * Mathf.Deg2Rad );
			pos.y = center.y + radius * Mathf.Cos ( ang * Mathf.Deg2Rad );
			pos.z = center.z;
			return pos;
		}


		public void show ( ) {
			iTween.MoveTo ( gameObject, iTween.Hash (
				"islocal", true,
				"time", 6f,
				"position", _initPos,
				"easetype", iTween.EaseType.easeOutExpo ) );
		}

	}
}


