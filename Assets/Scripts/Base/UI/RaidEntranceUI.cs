using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class RaidEntranceUI : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] Image timerImage;

    float timeBeforeRaid;
    float timerTime;
    InputPanel inputPanel;

    private void Awake()
    {
        inputPanel = InputPanel.Instance;
    }

    private void OnEnable()
    {
        inputPanel.DisableInput();
    }

    public void RunTimer(float timeBeforeRaid)
    {
        this.timeBeforeRaid = timeBeforeRaid;
        timerTime = timeBeforeRaid;
        StartCoroutine(RunTimerCoroutine());
    }

    IEnumerator RunTimerCoroutine()
    {
        while (timerTime > 0)
        {
            yield return new WaitForSeconds(1f);
            int value = Mathf.RoundToInt(timerTime);
            if (value == 0)
            {
                timerText.text = "GO";
            }
            else
            {
                timerText.text = value.ToString();
            }
            timerText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
            
        }
    }


    private void Update()
    {
        timerTime -= Time.deltaTime;
        timerImage.fillAmount = Mathf.Clamp01(timerTime / timeBeforeRaid);
    }

    private void OnDisable()
    {
        inputPanel.EnableInput();
    }
}
