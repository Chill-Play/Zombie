using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewFactory : ResourceFactory
{
    [SerializeField] List<GameObject> screwBoxLod = new List<GameObject>(); 
    [SerializeField] Animator transporter1;
    [SerializeField] Animator transporter2;
    [SerializeField] Animator button;
    [SerializeField] GameObject screwPrefab;
    [SerializeField] int screwMaxCount = 5;
    [SerializeField] float spawnScrewTime = 1f;

    int currentScrewLod = -1;
    List<GameObject> screws = new List<GameObject>();
    Coroutine workCoroutine;

    protected override void Setup()
    {
        base.Setup();

    }

    protected override void StartWork()
    {
        base.StartWork();
        if (workCoroutine != null)
        {
            StopCoroutine(workCoroutine);
        }
        workCoroutine = StartCoroutine(CreateScrews());        
    }

    protected override void StopWork()
    {
        base.StopWork();
        if (workCoroutine != null)
        {
            StopCoroutine(workCoroutine);
        }
        workCoroutine = StartCoroutine(StopWorkAnimation());
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

    protected override void AddResource(int count = 1)
    {
        base.AddResource(count);
        UpdateSwichLod();
    }

    public override void Unload(int count)
    {
        base.Unload(count);
        UpdateSwichLod();
    }

    public void UpdateSwichLod()
    {
        int screwLod = Mathf.FloorToInt((float)currentResourcesCount / (float)resourcesLimit * (float)(screwBoxLod.Count - 1));       
        if (currentScrewLod != -1)
        {
            screwBoxLod[currentScrewLod].SetActive(false);
        }
        screwBoxLod[screwLod].SetActive(true);
        currentScrewLod = screwLod;
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
