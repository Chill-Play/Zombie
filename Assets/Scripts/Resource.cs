using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int count = 1;
    [SerializeField] ResourceType type;
    


    void Start()
    {
        StartCoroutine(PickUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator PickUp()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        Vector3 startPos = transform.position;
        Player player = GameplayController.Instance.playerInstance;
        float t = 0;
        while(t < 1f)
        {
            t += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(startPos, player.transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        player.GetComponent<PlayerBackpack>().PickUp(type, count);
        Destroy(gameObject);
    }
}
