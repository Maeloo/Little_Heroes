using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLManager : MonoBehaviour {

	public static XmlDocument loadXML ( string file ) {
		XmlDocument xmlDoc	= new XmlDocument ( );
		TextAsset textAsset = ( TextAsset ) Resources.Load ( file );
		xmlDoc.LoadXml ( textAsset.text );
		
		return xmlDoc;
	}

}
