namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class ShieldCollider : MonoBehaviour {

		public GameObject _hitParticle;

		void OnTriggerEnter2D ( Collider2D col ) {
			EnemyBullet eb = col.gameObject.GetComponent<EnemyBullet> ( );

			SoundManager.getInstance ().playSoundimpactShield ();

			if ( eb != null ) {
				eb.destroy ( false );
				GameObject.Instantiate ( _hitParticle, col.transform.position, Quaternion.identity );
			}
		}
	}
}
