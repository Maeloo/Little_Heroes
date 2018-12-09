namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Xml;

	public class UIItem : MonoBehaviour {
				
		[SerializeField] protected Image	_icon;
		[SerializeField] protected Text	_name;
		[SerializeField] protected Image	_lock;
		[SerializeField] protected GameObject  _imgActive;	

		protected bool _isLock;
		protected int _index;
		protected string _item;
		protected string _type;
		protected bool   _itemActive = false;
		protected ItemsPanel _parentPanel;

		void Start ( ) {
			_isLock = true;

			GetComponentInChildren<Button> ( ).onClick.AddListener ( ( ) => {
				onClick ( );
			} );
		}

		public virtual void onClick ( ) {
			GameObject obj	= Resources.Load ( "avatar/" + _type + "/" + _item ) as GameObject;
			GameObject item = Instantiate ( obj ) as GameObject;

			SoundManagerUI.getInstance ().playItemsSelect ();

			UIManager.i.avatar.setItem ( item, _type, _item );

			if (_type == "body") {
				UIManager.i.avatar.changeColorOld ();
			}

			_parentPanel.selectItems (_index);
		}


		
		public virtual void create ( int indexItem, XmlNode descriptor, string type, ItemsPanel panel ) {

			_index = indexItem;
			_parentPanel = panel;

			gameObject.name = type + "_" + descriptor.Attributes["id"].Value;

			_isLock = bool.Parse ( descriptor.Attributes["lock"].Value );
			
			_lock.gameObject.SetActive ( _isLock );

			_name.text = descriptor.Attributes["name"].Value;
			_name.color = Color.white;

			if (_type == "color") {

			} else {
				string icon = descriptor.Attributes ["icon"].Value;

				if (!string.IsNullOrEmpty (icon)) {
					_icon.sprite = Resources.Load<Sprite> ("icons/" + icon);
				}			
			}

			_type = type;
			_item = descriptor.InnerText;

			if (_isLock == true) {
				gameObject.SetActive (false);
			}

			if (_itemActive == false) {
				_imgActive.SetActive( false);
			}

			//Active items
			GameObject ItemAvatar = UIManager.i.avatar.getItem (_type);
			string nameItems = "";
			if (_type == "color") {

			} else {
                nameItems = ItemAvatar.GetComponent<ItemsData> ( ).id;
			}

			if (nameItems != "" && nameItems == _item) {
				_imgActive.SetActive( true);
			}

			_name.enabled = false;

		}

		public void setActive(bool _activeData){
			_imgActive.SetActive( _activeData);
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