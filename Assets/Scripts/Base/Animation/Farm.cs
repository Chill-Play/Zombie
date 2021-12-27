using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : ResourceFactory
{
    [SerializeField] List<GameObject> carrotBoxLod = new List<GameObject>();
    [SerializeField] List<Carrot> carrots = new List<Carrot>();
    [SerializeField] Transform toBoxPos;
    [SerializeField] Transform waitPos;
    [SerializeField] Worker worker;

    int currentCarrotIndex = 0;
    int currentCarrotLod = -1;
    bool canWork = false;
    Coroutine workCoroutine;

    protected override void Start()
    {
        base.Start();
        foreach (var item in carrots)
        {
            item.Grow();
        }
        worker.GoToPosition(waitPos.position);
    }

    protected override void StartWork()
    {
        base.StartWork();
        if (workCoroutine != null)
        {
            StopCoroutine(workCoroutine);
        }
        for (int i = 0; i < carrots.Count; i++)
        {
            if (carrots[i].isGrows)
            {
                currentCarrotIndex = i;
            }
        }
        workCoroutine = StartCoroutine(Work());
    }

    [ContextMenu("Restart")]
    public void RestartWork()
    {
        StartWork();
        carrotBoxLod[currentCarrotIndex].SetActive(false);
        currentCarrotLod = -1;
    }

    IEnumerator Work()
    {
        while (true)
        {
            worker.Waving(false);
            while (!carrots[currentCarrotIndex].isGrows)
            {
                yield return new WaitForEndOfFrame();
            }

            worker.GoToPosition(carrots[currentCarrotIndex].transform.position);
            worker.Work(true);

            while (worker.IsMoving())
            {
                yield return new WaitForEndOfFrame();
            }

            carrots[currentCarrotIndex].Pull();
            worker.GoToPosition(toBoxPos.position);
            worker.Carring(true);

            while (worker.IsMoving())
            {
                yield return new WaitForEndOfFrame();
            }
            worker.Carring(false);
            NextCarrot();
        }
    }

    protected override void StopWork()
    {
        base.StopWork();
        if (workCoroutine != null)
        {
            StopCoroutine(workCoroutine);
        }
        workCoroutine =  StartCoroutine(StopWorkCoroutine());
    }

    IEnumerator StopWorkCoroutine()
    {
        worker.GoToPosition(waitPos.position);
        worker.Work(true);    
        while (worker.IsMoving())
        {
            yield return new WaitForEndOfFrame();
        }
        worker.Work(false);
        worker.Waving(true);
    }

    void NextCarrot()
    {
        currentCarrotIndex += currentCarrotIndex < carrots.Count - 1 ? 1 : -currentCarrotIndex;
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
        int screwLod = Mathf.FloorToInt((float)currentResourcesCount / (float)resourcesLimit * (float)(carrotBoxLod.Count - 1));
        if (currentCarrotLod != -1)
        {
            carrotBoxLod[currentCarrotLod].SetActive(false);
        }
        carrotBoxLod[screwLod].SetActive(true);
        currentCarrotLod = screwLod;
    }

}
