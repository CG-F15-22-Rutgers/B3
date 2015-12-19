using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {
    public Transform[] spawnposition;
    public GameObject  agent;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < spawnposition.Length; i++)
        {
            GameObject clone = (GameObject)Instantiate(agent, spawnposition[i].position, spawnposition[i].rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {

        
	
	}
}
