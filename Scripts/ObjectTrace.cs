using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectTrace : MonoBehaviour
{
    
    public TMP_Dropdown navigationTargetDropDown;        // Dropdown UI element for selecting navigation targets
    public List<Target> navigationTargetObjects = new List<Target>();  // List of Target objects representing possible navigation targets
    public GameObject markerPrefab;                        // Prefab of the marker object that will be instantiated to trace movement
    public float markerInterval = 0.5f;                  // Time interval between instantiating markers
    public Vector3 markerRotation = new Vector3(45f, 0f, 0f);     // Rotation applied to each marker
    private NavMeshAgent agent;                         // Reference to the NavMeshAgent component attached to the same GameObject
    private Vector3 targetPosition = Vector3.zero;     // Position of the current navigation target
    private int navigationTargetValue;                  // Index of the selected navigation target in the dropdown


    // Start is called before the first frame update
    void Start()
    {
       
        agent = GetComponent<NavMeshAgent>(); // Initialize the NavMeshAgent reference

        // Check if NavMeshAgent component is attached
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this object.");
        }
        else
        {
           
            if (navigationTargetDropDown == null) // Check if navigationTargetDropDown is assigned
            {
                Debug.LogError("Navigation Target Dropdown not assigned.");
            }
            else
            {
                // Set listener for dropdown value changed event
                navigationTargetDropDown.onValueChanged.AddListener(delegate { SetCurrentNavigationTarget(); });

                // Set initial navigation target and start coroutine to create markers
                SetCurrentNavigationTarget(); 
                StartCoroutine(InstantiateMarkers());
            }
        }
    }

    // Coroutine to continuously instantiate markers
    IEnumerator InstantiateMarkers()
    {
        while (true)
        {
            // Wait for markerInterval seconds
            yield return new WaitForSeconds(markerInterval);

            // If a navigation target is selected, create a marker
            if (targetPosition != Vector3.zero)
            {
                CreateMarker();
            }
        }
    }

    // Method to create a marker at the current position
    void CreateMarker()
    {
        // Instantiate a marker prefab at the current position
        GameObject marker = Instantiate(markerPrefab, transform.position, Quaternion.identity);

        // Set marker tag
        marker.tag = "Marker";

        // Calculate rotation for the marker based on agent's velocity
        Vector3 movementDirection = agent.velocity.normalized;
        Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up) * Quaternion.Euler(markerRotation);
        marker.transform.rotation = rotation;
    }

    // Method to set navigation target value
    public void SetNavigationTargetValue(int value)
    {
        navigationTargetValue = value;
    }

    // Method to set the current navigation target
    public void SetCurrentNavigationTarget()
    {
        // Check if dropdown and options are not null and count > 0
        if (navigationTargetDropDown != null && navigationTargetDropDown.options != null && navigationTargetDropDown.options.Count > 0)
        {
            // Get selected value and text from dropdown
            int selectedValue = navigationTargetValue;
            string selectedText = navigationTargetDropDown.options[selectedValue].text;

            // Check if selectedText is not null or empty
            if (!string.IsNullOrEmpty(selectedText))
            {
                // Find the corresponding Target object from navigationTargetObjects list
                Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(selectedText.ToLower()));

                // If target found, set targetPosition and agent destination
                if (currentTarget != null)
                {
                    targetPosition = currentTarget.PositionObject.transform.position;
                    agent.SetDestination(targetPosition);

                    // If agent is stopped, start it
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }
                }
            }
        }
    }
}
