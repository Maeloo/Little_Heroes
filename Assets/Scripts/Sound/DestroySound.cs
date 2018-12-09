namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(AudioSource))]
    public class DestroySound : MonoBehaviour {

        // Use this for initialization
        void Start ( ) {
			if (gameObject.GetComponent<AudioSource> ().playOnAwake == true) {
				Invoke ("delete", 2f);
			}
        }

        void delete ( ) {
            DestroyImmediate ( gameObject );
        }
    }
}