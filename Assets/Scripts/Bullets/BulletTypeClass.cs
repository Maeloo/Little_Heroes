namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public enum BulletType {
		FORCE,
		PLASMA,
		ELEC,
		ICE,
		ERROR
	}

	public static class BulletTypeClass {

		public static string typeToString ( BulletType type ) {
			string res = "";

			switch ( type ) { 
				case BulletType.ELEC:
					res = "elec";
					break;

				case BulletType.FORCE:
					res = "force";
					break;

				case BulletType.ICE:
					res = "ice";
					break;

				case BulletType.PLASMA:
					res = "plasma";
					break;
			}

			return res;
		}


		public static BulletType stringToType ( string type ) {
			switch ( type ) {
				case "elec":
					return BulletType.ELEC;
					break;

				case "force":
					return BulletType.FORCE;
					break;

				case "ice":
					return BulletType.ICE;
					break;

				case "plasma":
					return BulletType.PLASMA;
					break;
			}

			return BulletType.ERROR;
		}
		
	}
}