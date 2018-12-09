namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class ActionButton : ABasicButton {
		public string level;

		protected override void onClick () {
            Loader.getInstance ( ).showLoader ( true );
			Application.LoadLevelAsync (level);
		} 


	}
}
