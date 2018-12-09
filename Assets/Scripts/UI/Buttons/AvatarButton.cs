namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
		
	public class AvatarButton : ABasicButton {
		
		protected override void onClick () {
			SoundManagerUI.getInstance ().playVoix ();
			UIManager.i.avatar.clickAvatar ();
			Debug.Log ("OnClick Avatar");
		} 
		
	}
	
}