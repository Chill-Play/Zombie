using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    [SerializeField] List<GameObject> carrotBoxLod = new List<GameObject>();
    [SerializeField] List<Carrot> carrots = new List<Carrot>();
    [SerializeField] Transform toBoxPos;
    [SerializeField] Transform waitPos;
    [SerializeField] Worker worker;
    [SerializeField] float workTime = 10f;

    int currentCarrotIndex = 0;
    int currentCarrotLod = -1;
    bool canWork = false;

    void Start()
    {
        foreach (var item in carrots)
        {
            item.Grow();
        }
        worker.GoToPosition(waitPos.position);
    }

    void Update() 
    {
        if(!canWork && carrots[currentCarrotIndex].isGrows)
        {
            canWork = true;
            StartWork();
        }
    }

    public void StartWork()
    {
        StartCoroutine(Work());
    }

    [ContextMenu("Restart")]
    public void RestartWork()
    {
        StartWork();
        carrotBoxLod[currentCarrotIndex].SetActive(false);
        currentCarrotLod = -1;

        // foreach (var item in carrotBoxLod)
        // {
        //     item.SetActive(false);
        // }
    }

    IEnumerator Work()
    {
        float timer = 0;
        float startWorkTime = Time.time;

        while(timer < workTime)
        {
            worker.GoToPosition(carrots[currentCarrotIndex].transform.position);
            worker.Work(true);
            yield return new WaitForEndOfFrame();

            while(worker.IsMoving())
            {
                yield return new WaitForEndOfFrame();
            }

            carrots[currentCarrotIndex].Pull();
            worker.GoToPosition(toBoxPos.position);
            worker.Carring(true);

            while(worker.IsMoving())
            {
                yield return new WaitForEndOfFrame();
            }
            
            if(currentCarrotLod == -1)
            {
                StartCoroutine(SwitchLod());
            }
            NextCarrot();

            timer = Time.time - startWorkTime;
        }

        worker.Work(false);
    }

    void NextCarrot()
    {
        currentCarrotIndex += currentCarrotIndex < carrots.Count - 1 ? 1 : -currentCarrotIndex;
    }

    IEnumerator SwitchLod()
    {
        while(currentCarrotLod < carrotBoxLod.Count - 1)
        {
            if(currentCarrotLod == -1)
            {
                currentCarrotLod = 0;
                carrotBoxLod[(int)currentCarrotLod].SetActive(true);
            }
            else
            {
                carrotBoxLod[(int)currentCarrotLod].SetActive(false);
                currentCarrotLod++;
                carrotBoxLod[(int)currentCarrotLod].SetActive(true);
            }

            yield return new WaitForSeconds(workTime / carrotBoxLod.Count);
        }
    }

}
