using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class ball : MonoBehaviour {
	Rigidbody rid;
	// Use this for initialization
	void Start () {
		rid = GetComponent<Rigidbody>();
	}

	void OnInteractionStart(Transform t)
	{
		rid.isKinematic = true;
		rid.useGravity = false;
	}
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.forward * Time.deltaTime);
        }
	
	}

    public void add_level()
    {
        Application.LoadLevel(0);
    }

	public Node Node_throw(Val<Vector3> direction)
	{
		return new LeafInvoke(
			() => throwball(direction)
			);
	}

	public RunStatus throwball(Val<Vector3> direction)
	{
		transform.parent = null;
		rid.velocity = direction.Value*2.1f;
		rid.isKinematic = false;
		rid.useGravity = true;
		return RunStatus.Success;
	}



}
