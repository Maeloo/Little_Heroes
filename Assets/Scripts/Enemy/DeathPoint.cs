namespace LittleHeroes {
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class DeathPoint : MonoBehaviour {

        private float       _startTime;
        private CanvasGroup _cg;

        public void init ( ) {
            _startTime = Time.time;
            _cg = GetComponent<CanvasGroup> ( );

            RectTransform rootCanvasRect = GetComponentInParent<Canvas> ( ).GetComponent<RectTransform> ( );

            Vector3 screenPos = Camera.main.WorldToViewportPoint ( transform.position );
            screenPos.x *= rootCanvasRect.rect.width;
            screenPos.y *= rootCanvasRect.rect.height;
            transform.position = screenPos;

            iTween.MoveTo ( gameObject, iTween.Hash (
                "y", transform.position.y + 10,
                "time", 2f ) );
        }


        void Update ( ) {
            _cg.alpha = Mathf.Lerp ( _cg.alpha, 0f, .02f );
        }


    }
}
