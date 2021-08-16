using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int count = 1;
    [SerializeField] ResourceType type;
    //public Transform Picker { get; set; }

    public void PickUp(Transform picker)
    {
        StartCoroutine(PickUpCoroutine(picker));
    }


    IEnumerator PickUpCoroutine(Transform picker)
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        Vector3 startPos = transform.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(startPos, picker.transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        picker.GetComponent<PlayerBackpack>().PickUp(type, count);
        Destroy(gameObject);
    }
}
