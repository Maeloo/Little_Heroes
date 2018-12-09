namespace LittleHeroes {
	using UnityEngine;
	using System.Xml;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;
	
	public class UIManager : MonoBehaviour {
		
		#region Singleton Stuff
		private static UIManager		_instance		= null;
		private static readonly object	singletonLock	= new object ( );
		#endregion
		
		public GameObject prefabAvatar;
		
		private Avatar _avatar;
		public	Avatar avatar {
			get { return _avatar; }
		}
		
		private List<ABasicPanel> 	_panels;
		private List<ABasicButton> 	_buttons;

		public delegate void CallAfterClose ( );
		CallAfterClose _toCall;
		
		private ABasicPanel[]		_closeExceptions;

		private string _oldButtonClick;
		public	string oldButtonClick {
			get { return _oldButtonClick; }
		}
		
		
		public static UIManager i {
			get { return instance; }
		}
		
		public static UIManager instance {
			get {
				lock ( singletonLock ) {
					if ( _instance == null ) {
						_instance = ( UIManager ) GameObject.Find ( "UIManager" ).GetComponent<UIManager> ( );
					}
					return _instance;
				}
			}
		}
		
		
		void Start() {
			Init ();
		}
		
		
		void Init() {
			Log.i ("Initialisation...");
			
			XmlDocument doc = XMLManager.loadXML ( AppDataModel.AVATAR_DESCRIPTOR );
			GameObject 	go 	= Instantiate ( prefabAvatar ) as GameObject;

			SoundManagerUI.getInstance ().playMusicMenu (true);

			//Server setup
			AppDataModel.ROOM_NAME = getServerName();
			NetworkConfig.setServerSettings ( getServerIp(), getServerPort() );
			
			go.GetComponent<Player> ( ).enabled = false;
			_avatar = go.GetComponent<Avatar> ();
			//PlayerPrefs.DeleteAll ();
			if (PlayerPrefs.GetInt ("created") == 0) {
				_avatar.createFromXML (doc);
			} else {
				_avatar.createFromPlayerPref ();
			
			}
			_avatar.startAnimation();

			//Change bg
			Invoke ("initBg", 1f);

			//_panels 	= new List<ABasicPanel> ();
			//_buttons 	= new List<ABasicButton> ();
			Screen.orientation = ScreenOrientation.Portrait;

            Loader.getInstance ( ).showLoader ( false );
		}

		private void initBg(){
			PowerSelector.instance.displayBackground ("bg" + _avatar.getPower());
		}
		
		public void registerPanel(ABasicPanel panel) {
            if ( _panels == null )
                _panels = new List<ABasicPanel> ( );

			_panels.Add (panel);
		}
		
		
		public void registerButton(ABasicButton button) {
            if ( _buttons == null )
                _buttons = new List<ABasicButton> ( );

			_buttons.Add (button);
		}


		
		
		public void closePanels ( ) {
			float delay = 0f;
			
			List<ABasicPanel> closeExceptions = new List<ABasicPanel> ( );
			
			if ( _closeExceptions != null )
				closeExceptions = _closeExceptions.ToList<ABasicPanel> ( );


			for ( int i = 0; i < _panels.Count; ++i ) {
				if ( !closeExceptions.Contains ( _panels[i] ) )
					delay += _panels[i].hidePanel ( );
			}


			
			_closeExceptions = null;
			
			StartCoroutine ( onOpenPanel ( delay ) );
		}
		
		
		public void closeAllPanels ( ) {
			for ( int i = 0; i < _panels.Count; ++i ) {
				_panels[i].hidePanel ( );
			}
		}
		
		
		IEnumerator onOpenPanel ( float delay ) {
			yield return new WaitForSeconds ( delay );
			
			if ( _toCall != null )
				_toCall ( );
		}
		
		
		public void openPanel ( CallAfterClose func, ABasicPanel[] closeExceptions ) {
			_toCall				= func;
			_closeExceptions	= closeExceptions;
			
			closePanels ( );
		}

		public void openSinglePanel ( CallAfterClose func) {
			_toCall				= func;
			StartCoroutine ( onOpenPanel ( 0f ) );		

		}

		public void setOldClick(string id){
			_oldButtonClick = id;
		}

		public string getServerName(){
			return PlayerPrefs.GetString("servername");
		}
		public string getServerIp(){
			return PlayerPrefs.GetString("serverip");
		}
		public int getServerPort(){
			return PlayerPrefs .GetInt("serverport");
		}

		public void setServerName(string name){
			PlayerPrefs.SetString ("servername", name);
		}
		public void setServerIp(string ip){
			PlayerPrefs.SetString ("serverip", ip);
		}
		public void setServerPort(int port){
			PlayerPrefs.SetInt ("serverport", port);
		}



		
	}
}

