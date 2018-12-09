using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    
    public Transform camTransform;

    public float shake = 0f;

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    //public Vector3 originalPos;

    void Awake ( ) {
        if ( camTransform == null ) {
            camTransform = GetComponent ( typeof ( Transform ) ) as Transform;
        }
    }

    void OnEnable ( ) {
        //originalPos = camTransform.localPosition;
    }

    void Update ( ) {
        if ( shake > 0 ) {
            camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        } else {
            shake = 0f;
            //camTransform.localPosition = originalPos;
        }
    }
}
