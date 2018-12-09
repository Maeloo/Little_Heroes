namespace LittleHeroes {
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;
	
	public class ItemsPowerPanel : ABasicPanel {
		
		[SerializeField] 
		private string _type;
		
		[SerializeField] 
		private GameObject _grid;
		
		public GameObject	_itemPrefab;
		public float		_margin;
		
		private List<UIItemPower>	_items;
		private int				_version;
		private float			_width;
		
		
		
		protected void Start ( ) {
			base.Start ( );
			
			//GetComponent<RectTransform> ( ).sizeDelta = new Vector2 ( Screen.width, 100 );
			
			_items		= new List<UIItemPower> ( );
			_version	= -1;
			_width		= 0f;
			
			updateItems ( );
		}
		
		
		public override float hidePanel ( ) {
			//base.hidePanel ( );
			
			if ( !_isActive )
				return 0f;
			
			CancelInvoke ( );
			
			Log.i ( "Hide Items" );
			
			uint count = 0;
			foreach ( UIItemPower item in _items ) {
				iTween.MoveTo ( item.gameObject, iTween.Hash (
					"time", .6f,
					"delay", .1f * count,
					"x", -1000f,
					"easetype", iTween.EaseType.easeInOutExpo
					) );
				
				count++;
			}
			
			_isActive = false;
			GetComponent<ScrollRect> ().enabled = false;
			
			return .5f + .1f * count;
		}
		
		
		public override void showPanel ( ) {
			//base.showPanel ( );
			
			if ( _isActive )
				return;
			
			CancelInvoke ( );
			
			Log.i ( "show Items" );
			
			updateItems ( );
			
			//Ajust grid and parent
			float newGridWidth = (_width * _items.Count) + (_margin * (_items.Count-1));
			GetComponent<RectTransform> ( ).sizeDelta = new Vector2 ( Screen.width/2, 100f );
			_grid.GetComponent<RectTransform> ( ).sizeDelta = new Vector2 ( newGridWidth, 100f);

			uint count = 0;
			foreach ( UIItemPower item in _items ) {
				item.transform.localPosition = new Vector3 ( 1000f, 0f, 0f );
				item.transform.FindChild("icon").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				float marginLocal = 0;
				if(count >= 1){
					marginLocal = (_margin*count);
				}

				iTween.MoveTo ( item.gameObject, iTween.Hash ( 
                  "islocal", true,
                  "time", .6f,
                  "delay", .1f * count,
				  "x", _margin * ( count + 1 ) + _width * count, 
                  "easetype", iTween.EaseType.easeInOutExpo
                  ) );
				count++;
				
			}
			
			_isActive = true;
			GetComponent<ScrollRect> ().enabled = true;
		}
		
		
		private void updateItems ( ) {
			XmlDocument playerItems = XMLManager.loadXML ( "player_items" );
			XmlNode		item_root	= playerItems.SelectSingleNode ( "/items_root/" + _type );
			int			version		= int.Parse ( playerItems.SelectSingleNode ( "items_root" ).Attributes["version"].Value );
			
			if ( _version < version ) {
				_version = version;
				
				cleanItems ( );
				int indexItems = 0;
				foreach ( XmlNode item in item_root ) {
					GameObject	itemObj				= Instantiate ( _itemPrefab ) as GameObject;
					itemObj.transform.parent		= _grid.transform;
					itemObj.transform.localScale	= Vector3.one;
					itemObj.transform.localPosition	= new Vector3 ( 1000f, 0f, 0f );
					
					UIItemPower	itemCpt	= itemObj.GetComponent<UIItemPower> ( );

					itemCpt.create ( indexItems, item, _type, this );
					
					_width = _width < itemCpt.getWidth ( ) ? itemCpt.getWidth ( ) : _width;
					
					_items.Add ( itemCpt );
					indexItems++;
				}
			}
		}
		
		public void selectItems(int index) {
			//Disable all active items
			foreach (UIItemPower item in _items) {
				item.setActive(false);
			}
		
			_items [index].setActive (true);
		}
		
		private void cleanItems ( ) {
			foreach ( UIItemPower item in _items ) {
				_items.Remove ( item );
				
				DestroyImmediate ( item.gameObject );
			}
			
			_items = new List<UIItemPower> ( );
		}
		
	}
}