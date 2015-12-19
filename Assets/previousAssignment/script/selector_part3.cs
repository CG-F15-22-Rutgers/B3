using UnityEngine;
using System.Collections;

public class selector_part3 : MonoBehaviour
{
    GameObject selectagent;
    GameObject preselectagent;
    GameObject preObstacle;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("agent"))
                {
                    if (preselectagent != null)
                        preselectagent.GetComponent<Director_Animation>().isselected = false;
                    selectagent = hit.collider.gameObject;
                    selectagent.GetComponent<Director_Animation>().isselected = true;
                    Debug.Log(hit.collider.gameObject.name);
                    preselectagent = selectagent;
                }
                if (hit.collider.CompareTag("environment"))
                {
                    if (selectagent != null)
                    {
                        selectagent.GetComponent<Director_Animation>().target = hit.point;
                        Debug.Log(hit.collider.gameObject.name);
                    }
                    selectagent = null;
                }
                if (hit.collider.CompareTag("obstacle"))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (preObstacle != null)
                        preObstacle.GetComponent<Obstaclemove>().isselect = false;
                    hit.collider.gameObject.GetComponent<Obstaclemove>().isselect = true;
                    preObstacle = hit.collider.gameObject;
                }
            }

        }
    }
    public void loadlevel0()
    {
        Application.LoadLevel(0);
    }
}
