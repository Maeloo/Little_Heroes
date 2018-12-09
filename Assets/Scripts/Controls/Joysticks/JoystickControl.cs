using UnityEngine;
using System.Collections;

// [DEPRECATED]
public class JoystickControl : MonoBehaviour {

	private static int idController = -1;

	public JoystickCS joystick;
	public float XAxisFactor;
	public float YAxisFactor;
	public float Smooth;

	void Start ( ) {
		idController++;
	}


	void FixedUpdate ( ) {

		if ( Input.touchCount == 0 ) {
			GetComponent<Rigidbody2D>().velocity = Vector2.Lerp ( GetComponent<Rigidbody2D>().velocity, Vector2.zero, Smooth * Time.deltaTime );
		}
		else {
			Vector2 newVelocity =  joystick.position;
			newVelocity.x *= XAxisFactor;
			newVelocity.y *= YAxisFactor;

			GetComponent<Rigidbody2D>().velocity = newVelocity;
		}

	}


	private int getTouchByID ( int id ) {
		int res = -1;
		
		for ( int i = 0; i < Input.touchCount; ++i ) {
			if ( Input.GetTouch ( i ).fingerId == id )
				res = i;
		}

		return res;
	}	

}
