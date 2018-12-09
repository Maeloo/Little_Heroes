namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class Detector : MonoBehaviour {

		private Enemy self;
        private float lastSend;



		void Start ( ) {
			self        = transform.parent.GetComponent<Enemy> ( );
            lastSend    = Time.time;
		}


		void OnTriggerStay2D ( Collider2D col ) {
			if ( col.tag == "Spaceship" && Time.time - lastSend  > .3f ) {
                lastSend = Time.time;
				self.sendTargetCoord ( col.transform.position );
			}
		}


		/*void OnCollisionStay2D ( Collision2D col ) {
			if ( col.gameObject.tag == "Spaceship" ) {
				self.sendTargetCoord ( col.transform.position );
			}
		}*/

	}
}
