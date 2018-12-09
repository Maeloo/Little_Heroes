namespace LittleHeroes {
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

	public class CloseMenu : ABasicButton {

		[SerializeField] private ABasicPanel _panelClose;

		protected override void onClick ()
		{
			SoundManagerUI.getInstance ().playMenuClickClose ();
			_panelClose.hidePanel ();
		}
	}
}
