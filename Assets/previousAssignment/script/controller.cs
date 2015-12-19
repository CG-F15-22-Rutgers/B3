using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
       // if (animator.layerCount == 2)
        //    animator.SetLayerWeight(1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        animator.SetFloat("speed", vertical);
        animator.SetFloat("direction", horizontal);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("run", false);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("jump", true);
        }
        else
        {
            animator.SetBool("jump", false);
        }
    }
	
	
}
