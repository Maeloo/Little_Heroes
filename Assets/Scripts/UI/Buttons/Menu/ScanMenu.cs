namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	
	public class ScanMenu : ActionButton {
		private void onClick ()
		{
			SoundManagerUI.getInstance ().playMusicMenu (false);
			Loader.getInstance ( ).showLoader ( true );
			Application.LoadLevelAsync (level);
		
		}
	}
}
