namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class ARPosterSpaceship : MonoBehaviour, Vuforia.ITrackableEventHandler {

        public GameObject sight;
        public GameObject fx;
        public GameObject spaceship;

        private Vuforia.TrackableBehaviour mTrackableBehaviour;

        private bool mDetected = false;

        private Vector3 _spaceshipScale;


        // On create
        void Start ( ) {
            fx.SetActive ( false );

            _spaceshipScale = spaceship.transform.localScale;

            spaceship.transform.localScale = Vector3.zero;

            mTrackableBehaviour = GetComponent<Vuforia.TrackableBehaviour> ( );

            if ( mTrackableBehaviour ) {
                mTrackableBehaviour.RegisterTrackableEventHandler ( this );
            }
        }


        private void disableFX ( ) {
            fx.SetActive ( false );
        }


        // Changement d'état de la détection
        public void OnTrackableStateChanged (
            Vuforia.TrackableBehaviour.Status previousStatus,
            Vuforia.TrackableBehaviour.Status newStatus ) {
            if (
                ( newStatus == Vuforia.TrackableBehaviour.Status.DETECTED ||
                 newStatus == Vuforia.TrackableBehaviour.Status.TRACKED ||
                 newStatus == Vuforia.TrackableBehaviour.Status.EXTENDED_TRACKED ) ) {
                Log.i ( "DETECTED" );
                if ( !mDetected ) {
                    mDetected = true;

                    if ( sight != null )
                        sight.SetActive ( false );

                    fx.SetActive ( true );

                    iTween.ScaleTo ( spaceship, iTween.Hash (
                        "delay", 1f,
                        "time", 3f,
                        "scale", _spaceshipScale ) );

                    Invoke ( "disableFX", 5f );
                }
            } else {
                Log.i ( "LOST" );
                if ( mDetected ) {
                    mDetected = false;

                    if ( sight != null )
                        sight.SetActive ( true );

                    fx.SetActive ( false );

                    spaceship.transform.localScale = Vector3.zero;
                }
            }
        }
    }
}
