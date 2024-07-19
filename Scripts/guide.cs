using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guide : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the marker has the "Marker" tag
        if (other.CompareTag("Marker"))
        {
            // Destroy the marker object
            Destroy(other.gameObject);
        }
    }
}
