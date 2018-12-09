namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class ScreenUtils : object {

		public static bool isPointInLeftPart ( Vector3 point ) {
			return point.x < Screen.width / 2;
		}


		public static bool isPointInRightPart ( Vector3 point ) {
			return point.x >= Screen.width / 2;
		}

	}
}
