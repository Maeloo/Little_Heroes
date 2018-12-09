using UnityEngine;
using System.Collections;

// Gère les sons dans l'application
public class SoundManager : MonoBehaviour {

    [SerializeField] AudioSource    _musicSolo;
    [SerializeField] AudioSource    _musicMulti;
    [SerializeField] AudioSource    _ambientSpaceship;
    [SerializeField] AudioSource    _alertMessage;
    [SerializeField] AudioSource[]  _explosions;
    [SerializeField] AudioSource    _lowLife;
    [SerializeField] AudioSource    _moduleActivation;
    [SerializeField] AudioSource    _moduleDessativation;
    [SerializeField] AudioSource    _shieldLoop;
    [SerializeField] AudioSource    _reactorLoop;
    [SerializeField] AudioSource    _impactSpaceship;
    [SerializeField] AudioSource    _impactShield;
    [SerializeField] AudioSource    _teleportation;

	[SerializeField] AudioSource    _shootLaser;
	[SerializeField] AudioSource    _shootCannon;
	[SerializeField] AudioSource    _shootDephasor;

	[SerializeField] AudioSource    _shootLaserEnemy;

	#region Singleton Stuff
	private static SoundManager		_instance		= null;
	private static readonly object singletonLock	= new object ( );
	#endregion

	
    private static bool _created = false;

	
    public static SoundManager getInstance ( ) {
		lock ( singletonLock ) {
			if ( _instance == null && !_created ) {
				_instance = ( SoundManager ) GameObject.Find ( "SoundManager" ).GetComponent<SoundManager> ( );
				_created = true;
			}

			return _instance;
		}
	}

	
    private void Start ( ) {
		if ( _created ) {
			Destroy ( gameObject );
			return;
		}

		DontDestroyOnLoad ( gameObject );
	}


    public void playMusicMulti ( bool active ) {
        if ( active ) {
            _musicMulti.Play ( );
        } else {
            _musicMulti.Stop ( );
        }
    }
    
    
    public void playMusicSolo ( bool active ) {
        if ( active ) {
            _musicSolo.Play ( );
        } else {
            _musicSolo.Stop ( );
        }
    }
    
    
    public void playSoundAmbient ( bool active ) {
        if ( active ) {
            _ambientSpaceship.Play ( );
        } else {
            _ambientSpaceship.Stop ( );
        }
    }
    
    
    public void playSoundAlert ( ) { _alertMessage.Play ( ); }

    
    public void playSoundExplosion ( ) {
		AudioSource explo = _explosions [(int)Mathf.Floor (Random.value * _explosions.Length)];
		explo.playOnAwake = true;
		Instantiate (explo);
    }

    
    public void playSoundLowLife ( bool active ) {
        if ( active ) {
            _lowLife.Play ( );
        } else {
            _lowLife.Stop ( ); 
        }
    }
    
    
    public void playSoundModule ( bool active ) {
        if ( active ) {
            _moduleActivation.Play ( );
        } else {
            _moduleDessativation.Play ( );
        }
    }


    public void playShieldLoop ( bool active ) {
        if ( active ) {
            _shieldLoop.Play ( );
        } else {
            _shieldLoop.Stop ( );
        }
    }


    public void playReactorLoop ( bool active ) {
        if ( active ) {
            _reactorLoop.Play ( );
        } else {
            _reactorLoop.Stop ( );
        }
    }


    public void playSoundimpactSpaceship ( ) { 
		_impactSpaceship.playOnAwake = true;
		Instantiate (_impactSpaceship);
	}


    public void playSoundimpactShield ( ) {
		_impactShield.playOnAwake = true;
		Instantiate (_impactShield);
	}

    
    public void playSoundTeleport ( ) { _teleportation.Play ( ); }

	public void playShootLaser ( ) { 
		_shootLaser.playOnAwake = true;
		Instantiate (_shootLaser);
	}
	public void playShootCannon ( ) { 
		_shootCannon.playOnAwake = true;
		Instantiate (_shootCannon);
	}
	public void playShootDephasor ( ) {
		_shootDephasor.playOnAwake = true;
	}

	public void playShootLaserEnemy() {
		_shootLaserEnemy.playOnAwake = true;
		Instantiate (_shootLaserEnemy);
	}


}
