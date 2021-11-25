using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : MonoBehaviour
{
    [SerializeField] List<GameObject> batteryBoxLod = new List<GameObject>();
    [SerializeField] float workTime = 20f;
    [SerializeField] float batteryFillSpeed = 0.5f;
    [SerializeField] Animator generator;
    [SerializeField] Animator plant;

    int currentBatteryLod = -1;

    void Start()
    {
        plant.SetFloat("FillSpeed", batteryFillSpeed);
        StartWork();
    }

    public void StartWork()
    {
        plant.SetBool("Work", true);
        generator.SetBool("Work", true);
    }

    public void StopWork()
    {
        generator.SetBool("Work", false);
        plant.SetBool("Work", false);
    }

    [ContextMenu("Restart")]
    public void RestartWork()
    {
        batteryBoxLod[currentBatteryLod].SetActive(false);
        currentBatteryLod = -1;
        StartWork();
    }

    IEnumerator SwitchLod()
    {
        while(currentBatteryLod < batteryBoxLod.Count - 1)
        {
            if(currentBatteryLod == -1)
            {
                currentBatteryLod = 0;
                batteryBoxLod[currentBatteryLod].SetActive(true);
            } 
            else
            {
                batteryBoxLod[currentBatteryLod].SetActive(false);
                currentBatteryLod++;
                batteryBoxLod[currentBatteryLod].SetActive(true);
            }

            yield return new WaitForSeconds(workTime / batteryBoxLod.Count);
        }

        StopWork();
    }

    public void SwitchBatteryBoxLod()
    {
        if(currentBatteryLod == -1)
        {
            StartCoroutine(SwitchLod());
        }
    }

}
