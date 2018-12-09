using UnityEngine;
using System.Collections;

// Gère les sons dans l'application
using System.Collections.Generic;


public class SoundManagerUI : MonoBehaviour {
	
	[SerializeField] AudioSource _musicMenu;
	[SerializeField] AudioSource _menuClick;
	[SerializeField] AudioSource _menuClickClose;
	[SerializeField] AudioSource _itemsSelect;
	[SerializeField] AudioSource _itemsMasterSelect;
	[SerializeField] AudioSource _powerSelect;
	[SerializeField] AudioSource[]  _voix;

	#region Singleton Stuff
	private static SoundManagerUI		_instance		= null;
	private static readonly object singletonLock	= new object ( );
	#endregion
		
	
	public static SoundManagerUI getInstance ( ) {
		lock ( singletonLock ) {
			if ( _instance == null ) {
				_instance = ( SoundManagerUI ) GameObject.Find ( "SoundManagerUI" ).GetComponent<SoundManagerUI> ( );
			}
			
			return _instance;
		}
	}
	
	
	public void playMusicMenu ( bool active ) {
		if ( active ) {
			_musicMenu.Play ( );
		} else {
			_musicMenu.Stop ( );
		}
	}

	public void playMenuClick ( ) { _menuClick.Play ( ); }
	public void playMenuClickClose ( ) { _menuClickClose.Play ( ); }
	public void playItemsSelect ( ) { _itemsSelect.Play ( ); }
	public void playPowerSelect ( ) { _powerSelect.Play ( ); }
	public void playItemsMasterSelect ( ) { _itemsMasterSelect.Play ( ); }

	public void playVoix ( ) {
		_voix[( int ) Mathf.Floor ( Random.value * _voix.Length )].Play ( );
	}
}
