namespace LittleHeroes {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class TrapTrigger : Photon.MonoBehaviour {

        public bool scaleOnTrigger;
        public bool moveOnTrigger;

        [Multiline]
        public string title;

        [Multiline]
        public string content;

        public GameObject particle;
        public GameObject enemy;

        private Vector3  _initialScale;


        void Start ( ) {
            _initialScale = enemy.transform.localScale;

            if ( scaleOnTrigger )
                enemy.transform.localScale = Vector3.zero;

            if ( moveOnTrigger )
                enemy.SetActive ( false );

            if ( particle != null )
                particle.SetActive ( false );
        }


        void OnTriggerEnter2D ( Collider2D col ) {
            if ( col.tag == "Spaceship" ) {
                GetComponent<CircleCollider2D> ( ).enabled = false;

                if ( PhotonNetwork.isMasterClient ) {
                    Player target = Observer.instance.selectTarget ( );

                    if ( target != null ) {
                        target.photonView.RPC ( "onNewMessage",
                            PhotonTargets.Others,
                            new object[] { title, content } );
                    } 
                }

                if ( particle != null )
                    particle.SetActive ( true );

                enemy.SetActive ( true );

                if ( scaleOnTrigger )
                    iTween.ScaleTo ( enemy, iTween.Hash (
                        "delay", 3f,
                        "time", 4f,
                        "scale", _initialScale,
                        "easetype", iTween.EaseType.easeInOutExpo ) );

                if ( moveOnTrigger )
                    iTween.MoveTo ( enemy, iTween.Hash (
                        "islocal", true,
                        "delay", 3f,
                        "time", 4f,
                        "position", Vector3.zero,
                        "easetype", iTween.EaseType.easeInOutExpo ) );

                Invoke ( "disableParticle", 8f );
            }
        }


        private void disableParticle ( ) {
            if ( particle != null )
                particle.SetActive ( false );
        }

    }
}
