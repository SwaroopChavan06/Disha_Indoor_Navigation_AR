using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    private NavMeshPath path; // current calculated path
    private Vector3 targetPosition = Vector3.zero; // current target position

    private void Start()
    {
        path = new NavMeshPath();
        SetNavigationTargetDropDownOptions(); // Update dropdown options
    }

    private void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(selectedText.ToLower()));
        if (currentTarget != null)
        {
            targetPosition = currentTarget.PositionObject.transform.position;
        }
    }

    private void SetNavigationTargetDropDownOptions()
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        foreach (var target in navigationTargetObjects)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData(target.Name));
        }
    }
}
