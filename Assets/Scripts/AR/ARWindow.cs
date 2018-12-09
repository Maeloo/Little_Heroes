using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ARWindow : MonoBehaviour, Vuforia.ITrackableEventHandler {

	public Text OutputMessage;
    public GameObject sight;

	private Vuforia.TrackableBehaviour mTrackableBehaviour;

	private bool mDetected = false;




	// On create
	void Start ( ) {
		mTrackableBehaviour = GetComponent<Vuforia.TrackableBehaviour> ( );

		if ( mTrackableBehaviour ) {
			mTrackableBehaviour.RegisterTrackableEventHandler ( this );
		}
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
				mDetected			= true;
				//OutputMessage.text	= "DETECTED";

                if ( sight != null )
                    sight.SetActive ( false );
			}
		}
		else {
			Log.i ( "LOST" );
			if ( mDetected ) {
				mDetected			= false;
				//OutputMessage.text	= "LOST";

                if ( sight != null )
                    sight.SetActive ( true );
			}
		}
	}

}
