using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class SetNavTarget : MonoBehaviour
{
    [SerializeField]
   private Camera topDownCamera;
    public Transform target; // The target object to compute the path towards
    private NavMeshPath path; // Reference to the computed path
    private LineRenderer lineRenderer; // Reference to the LineRenderer component

    void Start()
    {
        // Get the LineRenderer component attached to this object
        lineRenderer = GetComponent<LineRenderer>();

        // Initialize the NavMeshPath
        path = new NavMeshPath();

        // Ensure that the target object is assigned
        if (target == null)
        {
            Debug.LogError("Target object not assigned.");
        }
        else
        {
            // Calculate and draw the initial path
            RecalculatePath();
        }
    }

    void Update()
    {
        // Check if the target object has moved
        if (target.hasChanged)
        {
            // Recalculate and redraw the path
            RecalculatePath();
        }
    }

    void RecalculatePath()
    {
        // Compute the path towards the target
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

        // Draw the computed path using LineRenderer
        DrawPath();
    }

    void DrawPath()
    {
        if (path != null && path.corners.Length > 1)
        {
            // Set the number of points to be drawn to the number of corners in the path
            lineRenderer.positionCount = path.corners.Length;

            // Set each point to the position of the corners in the path, but adjust y-position to 1
            for (int i = 0; i < path.corners.Length; i++)
            {
                Vector3 point = path.corners[i];
                point.y = 0; // Set y-position to 1
                lineRenderer.SetPosition(i, point);
            }
        }
    }
}