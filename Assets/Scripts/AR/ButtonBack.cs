namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class ButtonBack : MonoBehaviour {

        void Start ( ) {
            Loader.getInstance ( ).showLoader ( false );
        }


        public void onClick ( ) {
            Loader.getInstance ( ).showLoader ( true );
            Application.LoadLevelAsync ( "HomeSolo-Scene" );
        }

    }
}
