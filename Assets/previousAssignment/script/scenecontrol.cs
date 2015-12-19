using UnityEngine;
using System.Collections;

public class scenecontrol : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadlevelpart0()
    {
        Application.LoadLevel(0);
    }

    public void loadlevelpart1()
    {
        Application.LoadLevel(1);
    }
    public void loadlevelpart2()
    {
        Application.LoadLevel(2);
    }
    public void loadlevelpart3()
    {
        Application.LoadLevel(3);
    }
}
