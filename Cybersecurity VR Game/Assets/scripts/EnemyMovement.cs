using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target; // Assign your base or target here
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // If target isn't assigned in the inspector, find it via its tag
        if (target == null)
        {
            GameObject baseObject = GameObject.FindGameObjectWithTag("base");
            if (baseObject != null)
            {
                target = baseObject.transform;
            }
            else
            {
                Debug.LogWarning("No object with the tag 'Base' was found. Make sure the base is tagged correctly.");
            }
        }

        if (navMeshAgent && target)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
    

}
