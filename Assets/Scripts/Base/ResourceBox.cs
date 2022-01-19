using UnityEngine;


public class ResourceBox : MonoBehaviour
{
    [SerializeField] private GameObject[] resources;
    
    public void ShowResource(int index)
    {
        resources[index].SetActive(true);
    }

    public void HideResource(int index)
    {
        resources[index].SetActive(false);
    }

    public void HideAllResources()
    {
        foreach (var resource in resources)
        {
            resource.SetActive(false);
        }
    }
}