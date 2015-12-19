using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    private bool isInterAction = false;
    private bool isUserInterAction = true;
    private int time = 10;
    private bool open = false;
    void OnInteractionEnd(Transform t)
    {
        isInterAction = true;
        isUserInterAction = false;
        time = 200;
    }

    // Use this for initialization
    void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
        if (time > 0 && isInterAction == true)
        {
            transform.Translate(transform.right * Time.deltaTime);
            time--;
        }

    }
}
