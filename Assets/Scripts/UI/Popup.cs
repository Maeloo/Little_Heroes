namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class Popup : MonoBehaviour {

		public Bandeau top;
		public Bandeau bot;

		public RawImage bg;

		public Text title;
		public Text content;

		protected float	_displayTime;
		protected float	_currentLifeTime;
		protected bool	_displayed;


		void Start ( ) {
			bg.CrossFadeAlpha ( 0f, 0f, true );
			title.CrossFadeAlpha ( 0f, 0f, true );
			content.CrossFadeAlpha ( 0f, 0f, true );

			_displayed = false;			
		}


		void Update ( ) {
			if ( _displayed && Time.time - _displayTime > _currentLifeTime ) {
				initHidePopup ( );
			}
		}


		public void initDisplayPopup ( string p_title, string p_content, float lifeTime = 5f ) {
			title.text		= p_title;
			content.text	= p_content;

			_currentLifeTime = lifeTime;

			bg.CrossFadeAlpha ( 190f, .2f, true );

			Invoke ( "displayPopup", .2f );
		}


		protected void displayPopup ( ) {
			top.display ( );
			bot.display ( );

			title.CrossFadeAlpha ( 255f, .6f, true );
			content.CrossFadeAlpha ( 255f, .6f, true );

			_displayTime	= Time.time;
			_displayed		= _currentLifeTime > 0f ? true : false;
		}


		public void initHidePopup ( ) {
			_displayed = false;

			top.hide ( );
			bot.hide ( );

			title.CrossFadeAlpha ( 0f, .6f, true );
			content.CrossFadeAlpha ( 0f, .6f, true );

			Invoke ( "hidePopup", .6f );
		}


		protected void hidePopup ( ) {
			bg.CrossFadeAlpha ( 0f, .2f, true );
		}
		
	}
}
