namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class Loader : MonoBehaviour {

        #region Singleton Stuff
        private static Loader		    _instance		= null;
        private static readonly object singletonLock	= new object ( );
        #endregion


        public static Loader getInstance ( ) {
            lock ( singletonLock ) {
                if ( _instance == null ) {
                    _instance   = ( Loader ) GameObject.Find ( "Loader" ).GetComponent<Loader> ( );
                }

                return _instance;
            }
        }


        public void showLoader ( bool show ) {
            foreach ( Transform child in transform ) {
                child.gameObject.SetActive ( show );
            }
        }

    }
}
