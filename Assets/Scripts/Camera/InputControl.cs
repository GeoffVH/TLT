using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
   public GameObject cameraOrbit;
    public GameObject cameraFocus;
    public GameObject mapScene;
   public float rotateSpeed = 10f;

   void Update()
   {
       if (Input.GetMouseButton(0))
       {
           float h = rotateSpeed * Input.GetAxis("Mouse X");
           //float v = rotateSpeed * Input.GetAxis("Mouse Y");
		   float v = 0;

           if (cameraOrbit.transform.eulerAngles.z + v <= 0.1f || cameraOrbit.transform.eulerAngles.z + v >= 179.9f)
                v = 0;

           cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + h, cameraOrbit.transform.eulerAngles.z + v);
       }

	   if(Input.GetKey("d")){
		   cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + -1.5f, cameraOrbit.transform.eulerAngles.z);
	   }

	   if(Input.GetKey("a")){
		   cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + 1.5f, cameraOrbit.transform.eulerAngles.z);
	   }

       if(Input.GetKey("s")){
           //mapScene.transform.Translate(Vector3.forward * Time.deltaTime * rotateSpeed);
       }

        if(Input.GetKey("w")){
           //mapScene.transform.Translate(Vector3.back * Time.deltaTime * rotateSpeed);
       }

       float scrollFactor = Input.GetAxis("Mouse ScrollWheel");
       if (scrollFactor != 0)
       {
           cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - scrollFactor);
       }

   }
}
