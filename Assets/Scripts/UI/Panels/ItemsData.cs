namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;
	
	public class ItemsData : MonoBehaviour {
		
		[SerializeField] private string _id;
		public string id {
			get { return _id; }
		}

		
		
		protected void Start ( ) {
		
		}

	}
}