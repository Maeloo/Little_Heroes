namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Xml;
	
	public class UIItemPower : MonoBehaviour {
		
		private bool _isLock;
		
		[SerializeField] private Image	_icon;
		[SerializeField] private Text	_name;
		[SerializeField] private Image	_lock;
		[SerializeField] private Image  _imgActive;	
		
		private int _index;
		private string _item;
		private string _type;
		private ABasicPanel _bgPanel;
		private bool   _itemActive = false;
		private ItemsPowerPanel _parentPanel;
		
		void Start ( ) {
			_isLock = true;
			
			GetComponentInChildren<Button> ( ).onClick.AddListener ( ( ) => {
				onClick ( );
			} );
		}
		
		
		protected void onClick ( ) {
			SoundManagerUI.getInstance ().playPowerSelect ();

			UIManager.i.avatar.setPower (_item);

			PowerSelector.instance.displayBackground ("bg" + _item);

			_parentPanel.selectItems (_index);
		}
		
		
		
		public void create ( int indexItem, XmlNode descriptor, string type, ItemsPowerPanel panel ) {
			
			_index = indexItem;
			_parentPanel = panel;
			
			gameObject.name = type + "_" + descriptor.Attributes["id"].Value;
			
			_isLock = bool.Parse ( descriptor.Attributes["lock"].Value );
			
			_lock.gameObject.SetActive ( _isLock );
			
			_name.text = descriptor.Attributes["name"].Value;
			_name.color = Color.white;
			
			string icon = descriptor.Attributes["icon"].Value;
			
			if ( !string.IsNullOrEmpty ( icon ) ) {
				_icon.sprite = Resources.Load<Sprite>( "icons/" + icon ) ;
			}			
			
			_type = type;
			_item = descriptor.InnerText;
			
			if (_isLock == true) {
				gameObject.SetActive (false);
			}
			
			if (_itemActive == false) {
				_imgActive.enabled = false;
			}
			
			//Active items
			string powerAvatar = UIManager.i.avatar.getPower();
			if (powerAvatar != "" && powerAvatar == _item) {
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
		
	}
}