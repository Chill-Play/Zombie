using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHQZone : MonoBehaviour
{
    [SerializeField] SubjectId screenId;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        var screen = (HQUpgradeScreen)FindObjectOfType<UIController>().ShowScreen(screenId);
        screen.Show(FindObjectOfType<ResourcesController>().ResourcesCount, () => FindObjectOfType<UIController>().HideActiveScreen());
    }
}
