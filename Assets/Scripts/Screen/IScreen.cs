namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public interface IScreen {

		void loadScreen ( bool forward );
		void unloadScreen ( bool forward );

	}

	public class ScreenBase : MonoBehaviour, IScreen {

		protected static bool isLoading = false;

		protected IEnumerator loadPresumeComplete ( ) {
			yield return new WaitForSeconds ( .2f );

			isLoading = false;
		}

		public virtual void loadScreen ( bool forward ) { }
		public virtual void unloadScreen ( bool forward ) { }

		public void forceTransition ( ) { isLoading = false; }
	}
}
