using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

	public float Damage;

	
	public void destroy ( ) {
		gameObject.SetActive ( false );
	}
		
}
