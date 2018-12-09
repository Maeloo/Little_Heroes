using UnityEngine;
using System.Collections;

public class CameraRT : MonoBehaviour {

	[SerializeField]
	private Transform _mainCamera;
	
	void Update () {
		transform.rotation = _mainCamera.rotation;
	}
}
