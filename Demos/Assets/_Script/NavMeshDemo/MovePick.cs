using UnityEngine;

public class MovePick : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //obstacle = GetComponent<NavMeshObstacle>();
        //obstacle.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.enabled = true;
                //obstacle.enabled = false;
                agent.SetDestination(hit.point);
            }
        }
    }
}