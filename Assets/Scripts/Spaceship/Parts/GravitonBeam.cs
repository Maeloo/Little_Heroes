namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class GravitonBeam : MonoBehaviour {

		private GravitonTarget _closestTarget;
		public	GravitonTarget	currentTarget {
			get { return _closestTarget; }
		}


		void OnTriggerStay2D ( Collider2D col ) {
			GravitonTarget gt = col.GetComponent<GravitonTarget> ( );

			if ( gt != null ) {
				if ( _closestTarget == null ) {
					_closestTarget = gt;
					Log.i ( "Target acquired." );
				}
				else {
					if ( Vector3.Distance ( gt.transform.position, transform.position ) < Vector3.Distance ( _closestTarget.transform.position, transform.position ) ) {
						_closestTarget = gt;
						Log.i ( "Target acquired." );
					}
				}
			}
		}


		void OnTriggerExit2D ( Collider2D col ) {
			GravitonTarget gt = col.GetComponent<GravitonTarget> ( );

			if ( gt != null ) {
				if ( gt == _closestTarget ) {
					_closestTarget = null;
					Log.i ( "Target lost." );
				}
			}
		}

	}
}

