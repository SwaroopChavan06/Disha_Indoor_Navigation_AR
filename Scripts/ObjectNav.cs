using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNav : MonoBehaviour
{
   
    public Transform target; // The target object to navigate towards
    private UnityEngine.AI.NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component attached to this object
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this object.");
        }
        else
        {
            // Ensure that the target object is assigned
            if (target == null)
            {
                Debug.LogError("Target object not assigned.");
            }
            else
            {
                // Start navigating towards the target
                agent.SetDestination(target.position);
            }
        }
    }

    void Update()
    {
        // Check if the target object has moved
        if (target.hasChanged)
        {
            // Update the destination to the new position of the target object
            agent.SetDestination(target.position);
        }

        // Optional: You can also continuously check the distance between this object and the target
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log("Distance to target: " + distance);
    }

}
