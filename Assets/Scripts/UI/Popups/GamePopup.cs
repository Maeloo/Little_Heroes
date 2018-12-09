namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;

    public class GamePopup : Popup {

        #region Singleton Stuff
        private static GamePopup		_instance		= null;
        private static readonly object	singletonLock	= new object ( );
        #endregion


        protected Vector3 _initialTopPos;
        protected Vector3 _initialBotPos;


        public static GamePopup instance {
            get {
                lock ( singletonLock ) {
                    if ( _instance == null ) {
                        _instance = ( GamePopup ) GameObject.Find ( "GamePopup" ).GetComponent<GamePopup> ( );

                        DontDestroyOnLoad ( _instance );
                    }
                    return _instance;
                }
            }
        }


        void Start ( ) {
            _initialTopPos = top.transform.localPosition;
            _initialBotPos = bot.transform.localPosition;

            top.transform.localPosition = new Vector3 ( 0f, 15f, 0f );
            bot.transform.localPosition = new Vector3 ( 0f, -15f, 0f );

            title.CrossFadeAlpha ( 0f, 0f, true );
            content.CrossFadeAlpha ( 0f, 0f, true );

            _displayed = false;
        }


        void Update ( ) {
            if ( _displayed && Time.time - _displayTime > _currentLifeTime && _currentLifeTime > 0f ) {
                initHidePopup ( );
            }
        }


        public void initDisplayPopup ( string p_title, string p_content, float lifeTime = 5f ) {
            title.text      = p_title;
            content.text    = p_content;

            _currentLifeTime = lifeTime;

            top.display ( );
            bot.display ( );

            Invoke ( "displayPopup", .6f );
        }


        protected void displayPopup ( ) {
            iTween.MoveTo ( bot.gameObject, iTween.Hash(
                "islocal", true,
                "time", .3f,
                "position", _initialBotPos ) );

            iTween.MoveTo ( top.gameObject, iTween.Hash (
                "islocal", true,
                "time", .3f,
                "position", _initialTopPos ) );

            title.CrossFadeAlpha ( 255f, .6f, true );
            content.CrossFadeAlpha ( 255f, .6f, true );

            _displayTime = Time.time;
            _displayed = _currentLifeTime > 0f ? true : false;
        }
		

        public void initHidePopup ( ) {
            _displayed = false;

            iTween.MoveTo ( bot.gameObject, iTween.Hash (
                "islocal", true,
                "time", .3f,
                "position", new Vector3 ( 0f, -15f, 0f ) ) );

            iTween.MoveTo ( top.gameObject, iTween.Hash (
                "islocal", true,
                "time", .3f,
                "position", new Vector3 ( 0f, 15f, 0f ) ) );

            title.CrossFadeAlpha ( 0f, .1f, true );
            content.CrossFadeAlpha ( 0f, .1f, true );

            Invoke ( "hidePopup", .6f );
        }


        protected void hidePopup ( ) {
            top.hide ( );
            bot.hide ( );
        }

    }
}
