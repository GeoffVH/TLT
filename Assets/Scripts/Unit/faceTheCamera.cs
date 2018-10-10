/*
This script will forced whatever object it is attatched to, to face the camera. 
 */




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceTheCamera : MonoBehaviour {
public bool fix = true;

//If you moved the gameobject rotation around to face the camera, the optimal setup is (105, 0, -10)
//If active is set to true, The fix will be applied at each frame, useful if the camera rotate
public bool active = true;
public Vector3 fixPosition = new Vector3(-25f, 180f, 0f);
  private void OnEnable () {
    Fix ();
  }
  private void Update () {
    if (active)
        Fix ();
  }
  private void Fix() {
    if (fix)
        this.transform.rotation = Quaternion.LookRotation( -Camera.main.transform.forward ) * Quaternion.Euler (fixPosition);
  }
}

