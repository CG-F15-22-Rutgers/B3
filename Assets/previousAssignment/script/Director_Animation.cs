using UnityEngine;
using System.Collections;

public class Director_Animation : MonoBehaviour {
    public Vector3 target;
    private NavMeshAgent agent;
    private bool climb = false;
    private Animator anim;
    public bool isselected = false;
    private bool run = false;
    // Use this for initialization
    void Start()
    {
        target = new Vector3(-1000, -1000, -1000);
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != new Vector3(-1000, -1000, -1000))
        {
           agent.SetDestination(target);
        }
       
        if(isselected)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("run");
                run = !run;
            }
        }
        setAnimation();

    }

    private bool isreached()
    {
        bool isReach = false;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
               // if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
               // {
                    isReach = true;
               // }
            }
        }
        else isReach = false;



        return isReach;
    }
    private void setAnimation()
    {
        OffMeshLinkData link;
        anim.SetBool("walk", !isreached());
        anim.SetBool("run", run&&!isreached());
        if (agent.isOnOffMeshLink)
        {
            Debug.Log("is on off");
            link = agent.currentOffMeshLinkData;
            if (link.linkType == OffMeshLinkType.LinkTypeManual)
            {
                
                anim.SetBool("climbup", true);
                agent.autoTraverseOffMeshLink = false;
               // if (Mathf.Abs(link.startPos.y - transform.position.y) > Mathf.Abs(link.endPos.y - transform.position.y))
                 //   anim.SetBool("climbup", false);
                if (((transform.position.y > link.endPos.y) && (link.endPos.y > link.startPos.y)) || ((link.endPos.y - transform.position.y < 0.1) && (link.endPos.y < link.startPos.y)))
                {
                    agent.CompleteOffMeshLink();
                }
            }
            else
            {
                agent.autoTraverseOffMeshLink = true;
                anim.SetBool("jump", true);
               
            }
        }
        else
        {
            anim.SetBool("jump", false);
            anim.SetBool("climbup", false);
        }

    } 
        
 
}
