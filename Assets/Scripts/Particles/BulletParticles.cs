namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;


	[RequireComponent ( typeof ( ParticleSystem ) )]
	public class BulletParticles : MonoBehaviour {

		public bool OnlyDeactivate;
		public bool DestroyParent;


		void OnEnable ( ) {
			StartCoroutine ( "CheckIfAlive" );
		}

		IEnumerator CheckIfAlive ( ) {
			while ( true ) {
				yield return new WaitForSeconds ( 0.5f );
				if ( !GetComponent<ParticleSystem>().IsAlive ( true ) ) {
					if ( OnlyDeactivate ) {
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
						this.gameObject.SetActive ( false );
#endif
					}
					else
						if ( !DestroyParent )
							GameObject.Destroy ( this.gameObject );
						else
							GameObject.Destroy ( this.transform.parent.gameObject );
					break;
				}
			}
		}
	}
}