using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public float Damage;

	
	public void destroy ( ) {
		gameObject.SetActive ( false );
	}

}
