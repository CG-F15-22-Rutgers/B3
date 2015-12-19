using UnityEngine;
using System.Collections;

public class Obstaclemove : MonoBehaviour
{

    public bool isselect;
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    // Use this for initialization
    void Start()
    {
        isselect = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (isselect)
        { 
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;
            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
        }
    }
}
