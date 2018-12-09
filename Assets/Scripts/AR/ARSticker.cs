namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class ARSticker : MonoBehaviour, Vuforia.ITrackableEventHandler {

        public GameObject sight;

        private Vuforia.TrackableBehaviour mTrackableBehaviour;

        private bool mDetected = false;

        public GameObject top;
        public ParticleSystem fx;
        public GameObject item;

        private bool activated;


        // On create
        void Start ( ) {
            activated = false;
            mTrackableBehaviour = GetComponent<Vuforia.TrackableBehaviour> ( );

            if ( mTrackableBehaviour ) {
                mTrackableBehaviour.RegisterTrackableEventHandler ( this );
            }
        }


        void Update ( ) {
            if ( !activated && Input.touchCount == 1 ) {
                Touch touch0 = Input.GetTouch ( 0 );

                Ray rTouch = Camera.main.ScreenPointToRay ( touch0.position );

                RaycastHit hit = new RaycastHit ( );

                Debug.DrawRay ( rTouch.origin, rTouch.direction * 1000, Color.green );

                if ( Physics.Raycast ( rTouch, out hit, 1000 ) ) {
                    if ( hit.collider.CompareTag ( "Box" ) ) {
                        activated = true;

                        fx.Play ( );

                        Invoke ( "openBox", 2f );
                    }
                }
            }
        }


        private void openBox ( ) {
            iTween.FadeTo ( top, 0f, 2f );
            iTween.MoveTo ( item, new Vector3 ( 0f, 7f, 0f ), 2f );
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
                }
            } else {
                Log.i ( "LOST" );
                if ( mDetected ) {
                    mDetected = false;

                    if ( sight != null )
                        sight.SetActive ( true );
                }
            }
        }
    }
}
