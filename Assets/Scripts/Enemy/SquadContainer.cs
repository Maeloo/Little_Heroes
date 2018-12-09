namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class SquadContainer : MonoBehaviour {

        void Update ( ) {
            Vector3 dir = Spaceship.i.getCurrentPosition ( ) - transform.position;
            transform.rotation = Quaternion.LookRotation ( Vector3.forward, dir );
        }

    }
}
