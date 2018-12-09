namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;
	
	public abstract class ABasicPanel : MonoBehaviour {
		
		protected 	bool _isActive;
		public 		bool  isActive {
			get { return _isActive; }
		}

		
		
		protected void Start() {
			UIManager.i.registerPanel ( this );
		}
		
		
		public virtual void showPanel ( ) {
			if (_isActive)
				return;
			
			_isActive = true;
		}
		
		
		public virtual float hidePanel ( ) {
			if (!_isActive)
				return 0f;
			
			_isActive = false;
			
			return 0f;
		}
		
	}
}