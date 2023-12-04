User
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                FindAndFollowPath(hit.point);
            }
        }
    }

    void FindAndFollowPath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            List<Vector3> waypoints = new List<Vector3>(path.corners); // Convert array to List
            StartCoroutine(FollowPath(waypoints));
        }
    }

    IEnumerator FollowPath(List<Vector3> waypoints)
    {
        foreach (Vector3 waypoint in waypoints)
        {
            agent.SetDestination(waypoint);

            while (agent.pathPending)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            while (agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }
        }
    }
}


