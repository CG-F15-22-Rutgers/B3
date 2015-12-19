using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour
{

    public Vector3 target;
    private NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        target = new Vector3(-1000, -1000, -1000);
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
      
             if (target != new Vector3(-1000, -1000, -1000))
             {
                agent.SetDestination(target);
            }
         
       
    }

  

}
