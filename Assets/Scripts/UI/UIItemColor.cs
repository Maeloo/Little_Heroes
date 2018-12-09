namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Xml;
	
	public class UIItemColor : MonoBehaviour {
		
		private bool _isLock;
		
		[SerializeField] private Image	_icon;
		[SerializeField] private Text	_name;
		[SerializeField] private Image	_lock;
		[SerializeField] private Image  _imgActive;	
		
		private int _index;
		private string _item;
		private string _type;
		private string _colorHex;
		private Color32 _colorRGB;
		private ABasicPanel _bgPanel;
		private bool   _itemActive = false;
		private ItemsColorPanel _parentPanel;
		private string colorAvatar;
		
		void Start ( ) {
			_isLock = true;
			
			GetComponentInChildren<Button> ( ).onClick.AddListener ( ( ) => {
				onClick ( );
			} );
		}
		
		
		protected void onClick ( ) {
			SoundManagerUI.getInstance ().playItemsSelect ();

			UIManager.i.avatar.setColor (_colorHex);
			UIManager.i.avatar.changeColor (_colorHex);
			_parentPanel.selectItems (_index);
		}		
		
		public void create ( int indexItem, XmlNode descriptor, string type, ItemsColorPanel panel ) {			
			//Active items
			colorAvatar = UIManager.i.avatar.getColorHex();

			_index = indexItem;
			_parentPanel = panel;
			
			gameObject.name = type + "_" + descriptor.Attributes["id"].Value;
			
			_isLock = bool.Parse ( descriptor.Attributes["lock"].Value );
			
			_lock.gameObject.SetActive ( _isLock );
			
			_name.text = descriptor.Attributes["name"].Value;
			_name.color = Color.white;

			_type = type;
			_colorHex = descriptor.InnerText;

			//Color sprite
			_icon.color = HexToColor (_colorHex);

			if (_isLock == true) {
				gameObject.SetActive (false);
			}
			
			if (_itemActive == false) {
				_imgActive.enabled = false;
			}


			if (colorAvatar != "" && colorAvatar == _colorHex) {
				_imgActive.enabled = true;
			}
			
			_name.enabled = false;

			
		}
		
		public void setActive(bool _activeData){
			_imgActive.enabled = _activeData;
			_itemActive = _activeData;
		}
		
		
		public float getWidth ( ) {
			return _icon.rectTransform.rect.width;
		}
		
		public bool getLock(){
			return _isLock;
		}
		
		
		public float getHeight ( ) {
			return _icon.rectTransform.rect.height + _name.rectTransform.rect.height;
		}

		public Color32 HexToColor(string hex){
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b, 255);
		}

		
	}
}