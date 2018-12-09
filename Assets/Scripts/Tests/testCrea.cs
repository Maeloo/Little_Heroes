using UnityEngine;
using System.Collections;
using System.Xml;

public class testCrea : MonoBehaviour {

	public GameObject prefabAvatar;

	// Use this for initialization
	void Start ( ) {
		XmlDocument doc = XMLManager.loadXML ( "avatar_00001" );
		GameObject 	go 	= Instantiate ( prefabAvatar ) as GameObject;
		go.GetComponent<LittleHeroes.Avatar> ( ).createFromXML ( doc );
	}
}
