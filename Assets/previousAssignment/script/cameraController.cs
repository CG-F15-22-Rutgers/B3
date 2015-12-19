using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
    int sensitivity = 10;
    int rotate_speed = 100;
    Vector3 trans;
    Vector3 rotate;
    private Quaternion mrotation;
	// Use this for initialization
	void Start () {

        


	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(2))
        {
            trans.x = -Input.GetAxis("Mouse X");
            trans.z = -Input.GetAxis("Mouse Y");
        }
        else
        {
            trans.x = 0;
            trans.z = 0;
        }


        trans.y = -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        transform.Translate(trans,Space.World);
        
          
    


	}
}
