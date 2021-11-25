using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewFactory : MonoBehaviour
{
    [SerializeField] List<GameObject> screwBoxLod = new List<GameObject>();
    [SerializeField] float workTime = 20f;
    [SerializeField] Animator transporter1;
    [SerializeField] Animator transporter2;
    [SerializeField] Animator button;
    [SerializeField] GameObject screwPrefab;
    [SerializeField] int screwMaxCount = 5;
    [SerializeField] float spawnScrewTime = 1f;

    int currentScrewLod = -1;
    List<GameObject> screws = new List<GameObject>();

    void Start()
    {
        StartWork();
    }

    public void StartWork()
    {
        StartCoroutine(CreateScrews());
        //StartCoroutine(SwitchLod());
    }

    public void StopWork()
    {
        StartCoroutine(StopWorkAnimation());
    }

    [ContextMenu("Restart")]
    public void Restat()
    {
        screwBoxLod[currentScrewLod].SetActive(false);
        currentScrewLod = -1;
        StartWork();
    }

    IEnumerator CreateScrews()
    {
        transporter1.SetBool("Work", true);
        transporter2.SetBool("Work", true);
        button.SetBool("Work", true);

        if(screws.Count < screwMaxCount)
        {
            while(screws.Count < screwMaxCount)
            {
                yield return new WaitForSeconds(spawnScrewTime);
                screws.Add(Instantiate(screwPrefab, transform));
            }
        }
        else
        {
            foreach (var item in screws)
            {
                yield return new WaitForSeconds(spawnScrewTime);
                item.GetComponent<Animator>().SetBool("Work", true);
            }
        }
    }

    public void StartSwichLod()
    {
        if(currentScrewLod == -1)
        {
            StartCoroutine(SwitchLod());
        }
    }

    IEnumerator SwitchLod()
    {
        while(currentScrewLod < screwBoxLod.Count - 1)
        {
            if(currentScrewLod == -1)
            {
                currentScrewLod = 0;
                screwBoxLod[currentScrewLod].SetActive(true);
            }
            else
            {
                screwBoxLod[currentScrewLod].SetActive(false);
                currentScrewLod++;
                screwBoxLod[currentScrewLod].SetActive(true);
            }
            yield return new WaitForSeconds(workTime / screwBoxLod.Count);
        }

        StopWork();
    }

    IEnumerator StopWorkAnimation()
    {
        button.SetBool("Work", false);

        foreach (var item in screws)
        {
            item.GetComponent<Animator>().SetBool("Work", false);
        }

        yield return new WaitForSeconds(9f);

        transporter1.SetBool("Work", false);
        transporter2.SetBool("Work", false);
    }
}
