namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;
	
	public class BackgroundPanel : MonoBehaviour {

		[SerializeField] protected string _id;
		public string id {
			get { return _id; }
		}
		
		
		public void fadeOut ( ) {
			foreach ( Transform child in transform ) {
				iTween.FadeTo ( child.gameObject, 0f, .5f );
			}
		}
		
		
		public void fadeIn ( ) {
			foreach ( Transform child in transform ) {
				iTween.FadeTo ( child.gameObject, 1f, .5f );
			}
		}

		public void hide(){
			foreach (Transform child in transform) {
				iTween.FadeTo ( child.gameObject, 0f, 0f );
			}
		}

		public void show(){
			foreach (Transform child in transform) {
				iTween.FadeTo ( child.gameObject, 1f, 0f );
			}
		}
	
	}
		
		


}