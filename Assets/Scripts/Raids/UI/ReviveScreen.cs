using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ReviveScreen : UIScreen
{
    public event System.Action OnTimerEnd;

    [SerializeField] float waitTime = 5.0f;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Image timerImage;

    float timerTime;
    bool stopTimer;

    ReviveController reviveController;

    private void Awake()
    {
        reviveController = ReviveController.Instance;
    }

    void OnEnable()
    {
        timerTime = waitTime;
        StartCoroutine(RunTimerCoroutine());
    }

    IEnumerator RunTimerCoroutine()
    {
        while (timerTime > 0)
        {
            int value = Mathf.RoundToInt(timerTime);
            timerText.text = value.ToString();
            timerText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
            yield return new WaitForSeconds(1f);
        }
    }


    private void Update()
    {

        if (timerTime > 0.0f && !stopTimer)
        {
            timerTime -= Time.deltaTime;
            timerImage.fillAmount = Mathf.Clamp01(timerTime / waitTime);
        }
        else
        {
            OnTimerEnd?.Invoke();
        }
    }



    public void Revive()
    {
        var available = reviveController.ReviveClicked();
        if(available)
        {
            stopTimer = true;
        }
    }
}
