using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject converse1;
    public GameObject converse2;
    public GameObject sadperson;
    public GameObject Daniel;
    public GameObject Peter;
    private GameObject player;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        player = Peter;
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
    
        transform.position = player.transform.position + offset;
	}

    public void setConverse1()
    {
        player = converse1;
    }

    public void setConverse2()
    {
        player = converse2;
    }
    public void setsadperson()
    {
        player = sadperson;
    }

    public void setPeter()
    {
        player = Peter;
    }

    public void setDaniel()
    {
        player = Daniel;
    }

}
