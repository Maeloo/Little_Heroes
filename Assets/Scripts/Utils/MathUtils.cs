namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public static class MathUtils : object {

		public static bool isInRange ( float min, float max, float value ) {
			return ( value >= min && value <= max );
		}


		public static Vector2 randomBetweenRange ( Vector2 XRange, Vector2 YRange ) {
			return new Vector2 ( Random.Range ( XRange.x, XRange.y ), Random.Range ( YRange.x, YRange.y ) );
		}

	}
}

