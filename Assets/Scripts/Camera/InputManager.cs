using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public float panSpeed;
    private float panDetect = 15f;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        moveCamera();
    }

    void moveCamera(){
        float moveX = Camera.main.transform.position.x;
        float moveZ = Camera.main.transform.position.z;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if(Input.GetKey(KeyCode.A) || xPos > 0  && xPos < panDetect){
            moveX -= panSpeed;
        }
        else if(Input.GetKey(KeyCode.D) || xPos < Screen.width  && xPos > Screen.width - panDetect){
            moveX += panSpeed;
        }   

        if(Input.GetKey(KeyCode.S) || yPos > 0  && yPos < panDetect){
            moveZ -= panSpeed;
        }   
        else if(Input.GetKey(KeyCode.W) || yPos < Screen.height && yPos > Screen.height - panDetect){
            moveZ += panSpeed;
        }       

        Vector3 newPos = new Vector3(moveX, 7.155195f, moveZ);
        Camera.main.transform.position = newPos;
    }
}
