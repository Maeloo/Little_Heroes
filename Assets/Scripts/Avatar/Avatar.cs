namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;
	
	public class Avatar : Photon.MonoBehaviour {
		
		private uint 		_id;
		private string 		_name;
		private BulletType	_power;
		private GameObject 	_head;
		private GameObject 	_eyes;
		private GameObject 	_helmet;
		private GameObject 	_body;
		private string 		_colorHex;
		private GameObject 	_accessory;
		private XmlDocument _descriptor;
		private float shake = 0f;		
		private float shakeAmount = 2.0f;
		private float decreaseFactor = 1.0f;
		
		private Dictionary<string, string> _descriptor_d;
		public	Dictionary<string, string> Descriptor {
			get { return _descriptor_d; }
		}
		
		
		public string playerName {
			get { return _name; }
		}
		
		
		public BulletType power {
			get { return _power; }
		}
		
		
		public void createFromXML ( XmlDocument descriptor ) {
			_descriptor_d	= new Dictionary<string, string> ( );
			_descriptor		= descriptor;
			
			_name = descriptor.SelectSingleNode ( "/avatar_root/name" ).InnerText;
			_descriptor_d["name"] = _name;
			PlayerPrefs.SetString ("name", name);

			//Power
			string power = descriptor.SelectSingleNode ( "/avatar_root/power" ).InnerText;
			_descriptor_d["power"] = power;
			_power = BulletTypeClass.stringToType ( power );
			PlayerPrefs.SetString ("power", getPower());
			
			//Color
			_colorHex = descriptor.SelectSingleNode ( "/avatar_root/color" ).InnerText;
			_descriptor_d["color"] = _colorHex;
			PlayerPrefs.SetString ("color", _colorHex);

			// Creates head
			XmlNode 	headNode 		= descriptor.SelectSingleNode ( "/avatar_root/head" );
			string 		head 			= "avatar/head/" + headNode.InnerText;
			//string 		headComponent 	= headNode.Attributes ["component"].Value;
			GameObject 	headObj 		= Resources.Load ( head ) as GameObject;
			Vector3 	headPos 		= new Vector3 ( float.Parse ( headNode.Attributes["x"].Value ), float.Parse ( headNode.Attributes["y"].Value ), 0f );
			_head = Instantiate ( headObj ) as GameObject;
			
			_descriptor_d["head"]		= head;
			_descriptor_d["head_posx"]	= headNode.Attributes["x"].Value;
			_descriptor_d["head_posy"]	= headNode.Attributes["y"].Value;

			PlayerPrefs.SetString ("head", head);
			PlayerPrefs.SetString ("head_posx", headNode.Attributes["x"].Value);
			PlayerPrefs.SetString ("head_posy", headNode.Attributes["y"].Value);
			
			// Creates body
			XmlNode 	bodyNode 		= descriptor.SelectSingleNode ( "/avatar_root/body" );
			string 		body 			= "avatar/body/" + bodyNode.InnerText;
			//string 		bodyComponent 	= bodyNode.Attributes ["component"].Value;
			GameObject 	bodyObj 		= Resources.Load ( body ) as GameObject;
			Vector3 	bodyPos 		= new Vector3 ( float.Parse ( bodyNode.Attributes["x"].Value ), float.Parse ( bodyNode.Attributes["y"].Value ), 0f );
			_body = Instantiate ( bodyObj ) as GameObject;
			
			_descriptor_d["body"]		= body;
			_descriptor_d["body_posx"]	= bodyNode.Attributes["x"].Value;
			_descriptor_d["body_posy"]	= bodyNode.Attributes["y"].Value;

			PlayerPrefs.SetString ("body", body);
			PlayerPrefs.SetString ("body_posx", bodyNode.Attributes["x"].Value);
			PlayerPrefs.SetString ("body_posy", bodyNode.Attributes["y"].Value);
			
			// Creates helmet
			XmlNode 	helmetNode 		= descriptor.SelectSingleNode ( "/avatar_root/helmet" );
			string 		helmet 			= "avatar/helmet/" + helmetNode.InnerText;
			//string 		helmetComponent = helmetNode.Attributes ["component"].Value;
			GameObject 	helmetObj 		= Resources.Load ( helmet ) as GameObject;
			Vector3 	helmetPos 		= new Vector3 ( float.Parse ( helmetNode.Attributes["x"].Value ), float.Parse ( helmetNode.Attributes["y"].Value ), 0f );
			_helmet = Instantiate ( helmetObj ) as GameObject;
			
			_descriptor_d["helmet"]	= helmet;
			_descriptor_d["helmet_posx"] = helmetNode.Attributes["x"].Value;
			_descriptor_d["helmet_posy"] = helmetNode.Attributes["y"].Value;

			PlayerPrefs.SetString ("helmet", helmet);
			PlayerPrefs.SetString ("helmet_posx", helmetNode.Attributes["x"].Value);
			PlayerPrefs.SetString ("helmet_posy", helmetNode.Attributes["y"].Value);
			
			// Creates accessory
			XmlNode 	accNode		= descriptor.SelectSingleNode ( "/avatar_root/acc" );
			Vector3 	accPos		= Vector3.zero;
			
			_descriptor_d["acc"]	= "";
			PlayerPrefs.SetString ("acc", helmet);
		

			if ( !string.IsNullOrEmpty ( accNode.InnerText ) ) {
				string 		acc 			= "avatar/acc/" + accNode.InnerText;
				//string 		accComponent 	= helmetNode.Attributes ["component"].Value;
				GameObject 	accObj 			= Resources.Load ( acc ) as GameObject;
				accPos = new Vector3 ( float.Parse ( accNode.Attributes["x"].Value ), float.Parse ( accNode.Attributes["y"].Value ), 0f );
				_accessory = Instantiate ( accObj ) as GameObject;
				
				_descriptor_d["acc"] = acc;
				_descriptor_d["acc_posx"] = accNode.Attributes["x"].Value;
				_descriptor_d["acc_posy"] = accNode.Attributes["y"].Value;

				PlayerPrefs.SetString ("acc", acc);
				PlayerPrefs.SetString ("acc_posx", accNode.Attributes["x"].Value);
				PlayerPrefs.SetString ("acc_posy", accNode.Attributes["y"].Value);				
			}
			else {
				_accessory = new GameObject ( );
			}
			
			_eyes = new GameObject ( );
			
			Eyes eyes = _eyes.AddComponent<Eyes> ( );
			
			// Creates left eye
			XmlNode 	leyeNode 		= descriptor.SelectSingleNode ( "/avatar_root/eyes/left_eye" );
			string 		leye 			= "avatar/eyes/" + leyeNode.InnerText;
			//string 		leyeComponent 	= leyeNode.Attributes ["component"].Value;
			GameObject 	leyeObj 		= Resources.Load ( leye ) as GameObject;
			Vector3 	leyePos 		= new Vector3 ( float.Parse ( leyeNode.Attributes["x"].Value ), float.Parse ( leyeNode.Attributes["y"].Value ), 0f );
			GameObject  leyeGO			= Instantiate ( leyeObj ) as GameObject;
			
			eyes.registerLeftEye ( leyeGO );
			
			_descriptor_d["left_eye"] = leye;
			_descriptor_d["left_eye_posx"] = leyeNode.Attributes["x"].Value;
			_descriptor_d["left_eye_posy"] = leyeNode.Attributes["y"].Value;

			PlayerPrefs.SetString ("left_eye", leye);
			PlayerPrefs.SetString ("left_eye_posx", leyeNode.Attributes["x"].Value);
			PlayerPrefs.SetString ("left_eye_posy", leyeNode.Attributes["y"].Value);			
			
			// Creates right eye
			XmlNode 	reyeNode 		= descriptor.SelectSingleNode ( "/avatar_root/eyes/right_eye" );
			string 		reye 			= "avatar/eyes/" + reyeNode.InnerText;
			//string 		reyeComponent 	= reyeNode.Attributes ["component"].Value;
			GameObject 	reyeObj 		= Resources.Load ( reye ) as GameObject;
			Vector3 	reyePos 		= new Vector3 ( float.Parse ( reyeNode.Attributes["x"].Value ), float.Parse ( reyeNode.Attributes["y"].Value ), 0f );
			GameObject  reyeGO			= Instantiate ( reyeObj ) as GameObject;
			
			eyes.registerRightEye ( reyeGO );
			
			_descriptor_d["right_eye"] = reye;
			_descriptor_d["right_eye_posx"] = reyeNode.Attributes["x"].Value;
			_descriptor_d["right_eye_posy"] = reyeNode.Attributes["y"].Value;

			PlayerPrefs.SetString ("right_eye", reye);
			PlayerPrefs.SetString ("right_eye_posx", reyeNode.Attributes["x"].Value);
			PlayerPrefs.SetString ("right_eye_posy", reyeNode.Attributes["y"].Value);
			
			leyeGO.transform.SetParent ( _eyes.transform );
			leyeGO.transform.localPosition = leyePos;
			
			reyeGO.transform.SetParent ( _eyes.transform );
			reyeGO.transform.localPosition = reyePos;
			
			// Attach all parts to the correct parent
			_head.transform.SetParent ( transform );
			_head.transform.localPosition = headPos;
			
			_eyes.transform.SetParent ( transform );
			_eyes.transform.localPosition = Vector3.zero;
			
			_helmet.transform.SetParent ( transform );
			_helmet.transform.localPosition = helmetPos;
			
			_body.transform.SetParent ( transform );
			_body.transform.localPosition = bodyPos;
			
			_accessory.transform.SetParent ( transform );
			_accessory.transform.localPosition = accPos;

			//Color
			this.changeColor (_colorHex);

			//Check
			PlayerPrefs.SetInt ("created", 1);
			DictionaryToPlayerPref ();

		}

		public void createFromPlayerPref(){
			_descriptor_d = new Dictionary<string, string> ();
			//Custo
			_descriptor_d ["name"] = PlayerPrefs.GetString ("name");
			_descriptor_d ["power"] = PlayerPrefs.GetString ("power");
			_descriptor_d ["color"] = PlayerPrefs.GetString ("color");
			//Head
			_descriptor_d ["head"] = PlayerPrefs.GetString ("head");
			_descriptor_d ["head_posx"] = PlayerPrefs.GetString ("head_posx");
			_descriptor_d ["head_posy"] = PlayerPrefs.GetString ("head_posy");
			//Body
			_descriptor_d ["body"] = PlayerPrefs.GetString ("body");
			_descriptor_d ["body_posx"] = PlayerPrefs.GetString ("body_posx");
			_descriptor_d ["body_posy"] = PlayerPrefs.GetString ("body_posy");
			//Helmet
			_descriptor_d ["helmet"] = PlayerPrefs.GetString ("helmet");
			_descriptor_d ["helmet_posx"] = PlayerPrefs.GetString ("helmet_posx");
			_descriptor_d ["helmet_posy"] = PlayerPrefs.GetString ("helmet_posy");
			//Acc
			_descriptor_d ["acc"] = PlayerPrefs.GetString ("acc");
			_descriptor_d ["acc_posx"] = PlayerPrefs.GetString ("acc_posx");
			_descriptor_d ["acc_posy"] = PlayerPrefs.GetString ("acc_posy");
			//Left eye
			_descriptor_d ["left_eye"] = PlayerPrefs.GetString ("left_eye");
			_descriptor_d ["left_eye_posx"] = PlayerPrefs.GetString ("left_eye_posx");
			_descriptor_d ["left_eye_posy"] = PlayerPrefs.GetString ("left_eye_posy");
			//Right eye
			_descriptor_d ["right_eye"] = PlayerPrefs.GetString ("right_eye");
			_descriptor_d ["right_eye_posx"] = PlayerPrefs.GetString ("right_eye_posx");
			_descriptor_d ["right_eye_posy"] = PlayerPrefs.GetString ("right_eye_posy");

			//Create dico
			createFromDictionnary (_descriptor_d);

		}
		
		
		public void createFromDictionnary ( Dictionary<string, string> descriptor ) {
			_descriptor_d = descriptor;
			
			_name		= descriptor["name"];
			//Power
			_power		= BulletTypeClass.stringToType ( descriptor["power"] );
			//Color 
			_colorHex	=  descriptor["color"];
			
			// Creates head
			GameObject 	headObj 		= Resources.Load ( descriptor["head"] ) as GameObject;
			Vector3 	headPos 		= new Vector3 ( float.Parse ( descriptor["head_posx"] ), float.Parse ( descriptor["head_posy"] ), 0f );
			_head = Instantiate ( headObj ) as GameObject;
			
			// Creates body
			GameObject 	bodyObj 		= Resources.Load ( descriptor["body"] ) as GameObject;
			Vector3 	bodyPos 		= new Vector3 ( float.Parse ( descriptor["body_posx"] ), float.Parse ( descriptor["body_posy"] ), 0f );
			_body = Instantiate ( bodyObj ) as GameObject;
			
			// Creates helmet
			GameObject 	helmetObj 		= Resources.Load ( descriptor["helmet"] ) as GameObject;
			Vector3 	helmetPos 		= new Vector3 ( float.Parse ( descriptor["helmet_posx"] ), float.Parse ( descriptor["helmet_posy"] ), 0f );
			_helmet = Instantiate ( helmetObj ) as GameObject;
			
			// Creates accessory
			Vector3 	accPos	= Vector3.zero;
			if ( !string.IsNullOrEmpty ( descriptor["acc"] ) ) {
				GameObject 	accObj 			= Resources.Load ( descriptor["acc"] ) as GameObject;
				accPos = new Vector3 ( float.Parse ( descriptor["acc_posx"] ), float.Parse ( descriptor["acc_posy"] ), 0f );
				_accessory = Instantiate ( accObj ) as GameObject;
				
			}
			else {
				_accessory = new GameObject ( );
			}
			
			_eyes = new GameObject ( );
			
			Eyes eyes = _eyes.AddComponent<Eyes> ( );
			
			// Creates left eye
			GameObject 	leyeObj 		= Resources.Load ( descriptor["left_eye"] ) as GameObject;
			Vector3 	leyePos 		= new Vector3 ( float.Parse ( descriptor["left_eye_posx"] ), float.Parse ( descriptor["left_eye_posy"] ), 0f );
			GameObject  leyeGO			= Instantiate ( leyeObj ) as GameObject;
			
			eyes.registerLeftEye ( leyeGO );
			
			// Creates right eye
			GameObject 	reyeObj 		= Resources.Load ( descriptor["right_eye"] ) as GameObject;
			Vector3 	reyePos 		= new Vector3 ( float.Parse ( descriptor["right_eye_posx"] ), float.Parse ( descriptor["right_eye_posy"] ), 0f );
			GameObject  reyeGO			= Instantiate ( reyeObj ) as GameObject;
			
			eyes.registerRightEye ( reyeGO );
			
			leyeGO.transform.SetParent ( _eyes.transform );
			leyeGO.transform.localPosition = leyePos;
			
			reyeGO.transform.SetParent ( _eyes.transform );
			reyeGO.transform.localPosition = reyePos;
			
			// Attach all parts to the correct parent
			_head.transform.SetParent ( transform );
			_head.transform.localPosition = headPos;
			
			_eyes.transform.SetParent ( transform );
			_eyes.transform.localPosition = Vector3.zero;
			
			_helmet.transform.SetParent ( transform );
			_helmet.transform.localPosition = helmetPos;
			
			_body.transform.SetParent ( transform );
			_body.transform.localPosition = bodyPos;
			
			_accessory.transform.SetParent ( transform );
			_accessory.transform.localPosition = accPos;

			//Besoin des power et color ? 
			//Color
			this.changeColor (_colorHex);
			


		}

		public GameObject getItem(string type){
			switch (type) {
			case "body":
				return _body;
				break;
				
			case "helmet":
				return _helmet;
				break;
				
			case "head":
				return _head;
				break;

			case "acc":
				return _accessory;
				break;
			}

			return null;
		}

		//POWER
		public string getPower(){
			return BulletTypeClass.typeToString(_power);
		}

		public void setPower(string name){
			//_descriptor.SelectSingleNode ( "/avatar_root/power" ).InnerText = name;
			//_descriptor.Save ( Application.dataPath + "/Resources/" + AppDataModel.AVATAR_DESCRIPTOR + ".xml" );
			PlayerPrefs.SetString ("power", name);
			_power = BulletTypeClass.stringToType(name);
			_descriptor_d ["power"] = name;
			DictionaryToPlayerPref ();
		}

		//COLOR
		public string getColorHex(){
			return _colorHex;
		}
		
		public void setColor(string colorHex){
			//_descriptor.SelectSingleNode ( "/avatar_root/color" ).InnerText = colorHex;
			//_descriptor.Save ( Application.dataPath + "/Resources/" + AppDataModel.AVATAR_DESCRIPTOR + ".xml" );
			PlayerPrefs.SetString ("color", colorHex);
			_descriptor_d ["color"] = colorHex;
			_colorHex = colorHex;
			DictionaryToPlayerPref ();
		}

		public void changeColor(string colorHex){
			_body.transform.Find ("piece").GetComponent<SpriteRenderer> ().color = HexToColor (colorHex);
		}


		public void changeColorOld(){
			_body.transform.Find ("piece").GetComponent<SpriteRenderer> ().color = HexToColor (_descriptor_d ["color"]);
		}

		public void startAnimation(){
			GetComponent<TweenPosition> ().Play ();
		}



		public void clickAvatar(){
			shake = 0.8f;
		}

		void Update ( ) {
			if ( shake > 0 ) {

				transform.Rotate(Random.insideUnitSphere * shakeAmount);
			

				shake -= Time.deltaTime * decreaseFactor;
			} else {
				shake = 0f;
				transform.rotation = Quaternion.identity;
			}
		}

		
		public void setItem ( GameObject item, string type, string name ) {
			//_descriptor.SelectSingleNode ( "/avatar_root/" + type ).InnerText = name;

			
			item.transform.SetParent ( transform );
			
			Vector3 newPos = Vector3.zero;
			
			Log.i ( "Set Item " + type + " " + name );
			
			switch ( type ) {
				case "body":
					newPos = _body.transform.localPosition;
					_descriptor_d["body"] = "avatar/body/" + name;
					//_descriptor_d["color"] = "ffffff";
					DestroyImmediate ( _body );
					
					_body = item;
					break;
					
				case "helmet":
					newPos = _helmet.transform.localPosition;
					_descriptor_d["helmet"] = "avatar/helmet/" +name;
					DestroyImmediate ( _helmet );
					
					_helmet = item;
					break;
					
				case "head":
					newPos = _head.transform.localPosition;
					
					_descriptor_d["head"] = "avatar/head/" +name;
					DestroyImmediate ( _head );
					
					_head = item;
					break;

				case "acc":
					newPos = _accessory.transform.localPosition;
					_descriptor_d["acc"] = "avatar/acc/" +name;
					DestroyImmediate ( _accessory );
					
					_accessory = item;
					break;

				case "power":
					_descriptor_d["power"] = name;
					break;

				case "color":
					_descriptor_d["color"] = name;
				break;
			}
			
			item.transform.localPosition = newPos;
			//_descriptor.Save ( Application.dataPath + "/Resources/" + AppDataModel.AVATAR_DESCRIPTOR + ".xml" );
			DictionaryToPlayerPref ();
		}

		public void setPlayerName(string name){
			PlayerPrefs.SetString ("name", name);
			_name = name;
			_descriptor_d ["name"] = name;
			DictionaryToPlayerPref ();
		}
		
		private void OnTriggerEnter ( Collider col ) {
			if ( col.CompareTag ( "ModuleController" ) ) {
				Log.i ( col.tag );
			}
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

		public void DictionaryToPlayerPref(){
			foreach (string key in _descriptor_d.Keys) {
				PlayerPrefs.SetString(key, _descriptor_d[key]);
			}
		}


		//Tools
		public Color32 HexToColor(string hex){
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b, 255);
		}
		
	}
}