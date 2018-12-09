using UnityEngine;
using System.Collections;

public static class ExtensionGameObject {

    public static void ForceSetActiveRecursively ( this GameObject rootObject, bool active ) {
        rootObject.SetActive ( active );

        foreach ( Transform childTransform in rootObject.transform ) {
            ForceSetActiveRecursively ( childTransform.gameObject, active );
        }
    }

}
