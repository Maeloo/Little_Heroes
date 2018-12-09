namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class GameEnvironment : MonoBehaviour {

        public GameObject[] backgrounds;
        public Vector2      gridSize;
        public float        imageSize;


        void Start ( ) {
            Init ( );
        }


        private void Init ( ) {
            Log.i ( "Initialization" );
            for ( int i = 0; i < gridSize.y; ++i ) {
                for ( int j = 0; j < gridSize.x; ++j ) {
                    GameObject prefab   = backgrounds[( int ) Mathf.Floor ( Random.value * backgrounds.Length )];
                    GameObject bg       = NGUITools.AddChild ( gameObject, prefab );

                    bg.transform.localPosition = new Vector3 ( j * imageSize, i * imageSize, 1f );
                    bg.transform.Rotate ( new Vector3 ( 0f, 0f, 90 * Mathf.Floor ( Random.value * 4 ) ) );
                }
            }
        }

    }
}
