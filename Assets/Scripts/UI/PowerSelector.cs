namespace LittleHeroes{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class PowerSelector : MonoBehaviour {
		#region Singleton Stuff
		private static PowerSelector		_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion

		private Dictionary<string, GameObject> powers;
		private GameObject currentBg;

		public static PowerSelector instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( PowerSelector ) GameObject.Find ( "bgScreen" ).GetComponent<PowerSelector> ( );
						
						DontDestroyOnLoad ( _instance );
					}
					return _instance;
				}
			}
		}

		// Use this for initialization
		void Start () {
			powers = new Dictionary<string, GameObject> ();
			foreach (Transform child in transform) {
				BackgroundPanel panel = child.GetComponent<BackgroundPanel>();
				if(panel != null){
					powers.Add (panel.id, child.gameObject);
					panel.hide();
				}
				string bg = "bg"+UIManager.i.avatar.getPower();
				if(panel.id == bg){
					panel.show ();
				}
			}


		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void displayBackground(string id){
			if (currentBg != null) {
				currentBg.GetComponent<BackgroundPanel> ().fadeOut ();
			}
			if (powers [id] != null) {
				powers[id].GetComponent<BackgroundPanel>().fadeIn();
				currentBg = powers[id];
			}
		}
	}
}
