namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class Arrow : MonoBehaviour {

        void Update ( ) {
            Vector3 dir = Spaceship.i.closestGenerator ( ) - Spaceship.i.getCurrentPosition ( );
            transform.rotation = Quaternion.LookRotation ( Vector3.forward, dir );
        }
       
    }
}
