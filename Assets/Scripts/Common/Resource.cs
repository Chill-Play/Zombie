using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int count = 1;
    [SerializeField] ResourceType type;

    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }  

    public void Pickup(Transform picker)
    {
        StartCoroutine(PickUpCoroutine(picker));
    }


    IEnumerator PickUpCoroutine(Transform picker)
    {
        yield return new WaitForSeconds(1f);
        body.isKinematic = true;
        Vector3 startPos = transform.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(startPos, picker.transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        PlayerBackpack playerBackpack = picker.GetComponent<PlayerBackpack>();
        if (playerBackpack != null)
        {
            playerBackpack.PickUp(type, count);
        }
        else
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo();
            resourcesInfo.AddSlot(type, count);
            picker.GetComponent<IResourceStore>().OnPickupResource(type, 0, count);
            FindObjectOfType<ResourcesController>().AddResources(resourcesInfo);
            FindObjectOfType<ResourcesController>().UpdateResources();
        }
        Destroy(gameObject);
    }
}
