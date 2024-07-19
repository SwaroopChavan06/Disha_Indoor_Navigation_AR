using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speretotarget : MonoBehaviour
{
    public GameObject spherePrefab;
    public TMP_Dropdown navigationTargetDropDown; // Dropdown for selecting navigation targets
    public List<Target> navigationTargetObjects = new List<Target>(); // List of navigation target objects

    public void SpawnCubeAheadOfCamera()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
        // Get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Calculate position ahead of the camera
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 1.5f; // Adjust the multiplier as needed

            // Instantiate a cube at the calculated position with camera's rotation
            GameObject sphere = Instantiate(spherePrefab, spawnPosition, mainCamera.transform.rotation);
            sphere.tag = "Marker";
            // Get the script component attached to the spawned sphere
            ObjectTrace sphereScript = sphere.GetComponent<ObjectTrace>();

            // Check if the script component exists
            if (sphereScript != null)
            {
                // Get the value of the dropdown selection
                int dropdownValue = navigationTargetDropDown.value;

                // Pass the value to the script inside the sphere
                sphereScript.SetNavigationTargetValue(dropdownValue);
            }
            else
            {
                Debug.LogError("Script component not found in the spawned sphere.");
            }
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }
}
