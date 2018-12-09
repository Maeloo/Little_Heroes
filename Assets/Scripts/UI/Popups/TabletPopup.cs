namespace LittleHeroes {
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class TabletPopup : Popup {

        #region Singleton Stuff
        private static TabletPopup		_instance		= null;
        private static readonly object	singletonLock	= new object ( );
        #endregion


        protected Vector3   _initialTopPos;
        protected Vector3   _initialBotPos;
        protected Vector3   _initialBtnScale;

        public Button       button;


        public static TabletPopup instance {
            get {
                lock ( singletonLock ) {
                    if ( _instance == null ) {
                        _instance = ( TabletPopup ) GameObject.Find ( "TabletPopup" ).GetComponent<TabletPopup> ( );

                        DontDestroyOnLoad ( _instance );
                    }
                    return _instance;
                }
            }
        }


        void Start ( ) {
            _initialTopPos = top.transform.position;
            _initialBotPos = bot.transform.position;

            top.transform.localPosition = Vector3.zero;
            bot.transform.localPosition = Vector3.zero;

            title.CrossFadeAlpha ( 0f, 0f, true );
            content.CrossFadeAlpha ( 0f, 0f, true );

            _initialBtnScale = button.transform.localScale;

            button.transform.localScale = Vector3.zero;

            // solution degeu bug button
            button.transform.parent = transform.parent;

            _displayed = false;
        }


        void Update ( ) {
            if ( _displayed && Time.time - _displayTime > _currentLifeTime && _currentLifeTime > 0f ) {
                initHidePopup ( );
            }
        }


        public void initDisplayPopup ( string p_title, string p_content, float lifeTime = 5f ) {
            title.text = p_title;
            content.text = p_content;

            _currentLifeTime = lifeTime;

            top.display ( );
            bot.display ( );

            Invoke ( "displayPopup", .6f );
        }


        protected void displayPopup ( ) {
            iTween.MoveTo ( bot.gameObject, _initialBotPos, .3f );
            iTween.MoveTo ( top.gameObject, _initialTopPos, .3f );

            iTween.ScaleTo ( button.gameObject, Vector3.one, .6f );

            title.CrossFadeAlpha ( 255f, .6f, true );
            content.CrossFadeAlpha ( 255f, .6f, true );

            _displayTime = Time.time;
            _displayed = _currentLifeTime > 0f ? true : false;
        }


        public void initHidePopup ( ) {
            _displayed = false;

            iTween.ScaleTo ( button.gameObject, Vector3.zero, .6f );

            title.CrossFadeAlpha ( 0f, .6f, true );
            content.CrossFadeAlpha ( 0f, .6f, true );

            Invoke ( "hidePopup", .6f );
        }


        protected void hidePopup ( ) {
            top.hide ( );
            bot.hide ( );
        }


        public void onClick ( ) {
            Loader.getInstance ( ).showLoader ( true );
            PhotonNetwork.LoadLevel ( "Lobby-Scene" );
        }

    }
}
