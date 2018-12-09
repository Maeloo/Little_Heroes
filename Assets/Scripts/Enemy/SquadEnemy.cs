namespace LittleHeroes {
	using UnityEngine;
	using System.Collections;

	public class SquadEnemy : Enemy {

        private Vector3 _initialLocalPosition;


        protected void Start ( ) {
            base.Start ( );

            _initialLocalPosition = transform.localPosition;
        }

		protected void Update ( ) {
			if ( !photonView.isMine ) {
				transform.position = Vector3.Lerp ( transform.position, _realPosition, Time.deltaTime );
				transform.rotation = Quaternion.Lerp ( transform.rotation, _realRotation, Time.deltaTime );
			}
			else {
                if ( _life > 0 )
                    transform.localPosition = Vector3.Lerp ( transform.localPosition, _initialLocalPosition, Time.deltaTime * 5 );

				if ( _isAgressive ) {
					if ( Time.time - _lastShot > FireRate ) {
						shootBullet ( );
					}
				}
			}
		}

	}
}
