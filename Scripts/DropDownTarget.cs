using UnityEngine;
using UnityEngine.UI;

public class DropdownNavigation : MonoBehaviour
{
    public Dropdown dropdown; // Reference to the dropdown menu
    public GameObject sphere; // Reference to the sphere object
    private UnityEngine.AI.NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component attached to the sphere object
        agent = sphere.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the sphere object.");
        }
        else
        {
            // Ensure that the dropdown is assigned
            if (dropdown == null)
            {
                Debug.LogError("Dropdown menu not assigned.");
            }
            else
            {
                // Register a callback for when a dropdown value changes
                dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
            }
        }
    }

    // Callback function for dropdown value change
    void DropdownValueChanged()
    {
        // Ensure that the selected option index is valid
        if (dropdown.value >= 0 && dropdown.value < dropdown.options.Count)
        {
            // Get the name of the selected option
            string selectedOption = dropdown.options[dropdown.value].text;

            // Find the GameObject with the same name as the selected option
            GameObject targetObject = GameObject.Find(selectedOption);

            // Ensure that the target object is found
            if (targetObject != null)
            {
                // Set the position of the target object as the destination for the sphere
                agent.SetDestination(targetObject.transform.position);
            }
            else
            {
                Debug.LogWarning("Target object not found for the selected option: " + selectedOption);
            }
        }
        else
        {
            Debug.LogWarning("Invalid dropdown value: " + dropdown.value);
        }
    }
}
