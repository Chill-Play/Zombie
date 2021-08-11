using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : UIScreen
{
    enum State
    {
        NoiseBar,
        ComingTimer,
        Coming,
        TimeToRetreat
    }

    [SerializeField] Bar noiseBar;
    [SerializeField] GameObject timerGO;
    [SerializeField] Bar timerFill;
    [SerializeField] GameObject timeToRetreat;
    [SerializeField] ResourcesInfoUIPanel resourcesInfo;
    [SerializeField] BaseIndicatorUI baseIndicator;

    State state;

    // Start is called before the first frame update
    void Start()
    {
        if (Level.Instance != null)
        {
            Level.Instance.OnNoiseLevelChanged += Level_OnNoiseLevelChanged;
            Level.Instance.OnNoiseLevelExceeded += Level_OnNoiseLevelExceeded;
            Level.Instance.OnHordeDefeated += Level_OnHordeDefeated;
        }
        noiseBar.SetValue(0f);

        PlayerBackpack backpack = FindObjectOfType<PlayerBackpack>();
        backpack.OnPickupResource += Backpack_OnPickupResource;
        baseIndicator.gameObject.SetActive(false);
    }

    private void Backpack_OnPickupResource(ResourceType type, int total, int added)
    {
        resourcesInfo.UpdateBar(type, total);
    }

    private void Level_OnHordeDefeated()
    {
        SwitchState(State.TimeToRetreat);
    }

    private void Level_OnNoiseLevelExceeded()
    {
        SwitchState(State.ComingTimer);
    }

    private void Level_OnNoiseLevelChanged(float value)
    {
        noiseBar.SetValue(value / Level.Instance.MaxNoiseLevel);
        noiseBar.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 3);
    }


    void Update()
    {
        switch (state)
        {
            case State.NoiseBar:
                break;
            case State.ComingTimer:
                timerFill.SetValue(Level.Instance.ComingTimerValue);
                if (Level.Instance.ComingTimerValue <= Mathf.Epsilon)
                {
                    SwitchState(State.Coming);
                }
                break;
            case State.TimeToRetreat:
                break;
            case State.Coming:
                break;
        }
    }


    void SwitchState(State targetState)
    {
        switch (targetState)
        {
            case State.NoiseBar:
                break;
            case State.ComingTimer:
                Sequence sequence = DOTween.Sequence();
                sequence.Append(noiseBar.transform.DOScale(0.5f, 0.3f).SetEase(Ease.InCirc));
                sequence.AppendCallback(() =>
                {
                    noiseBar.gameObject.SetActive(false);
                    timerGO.transform.localScale = Vector3.one * 0.5f;
                    timerGO.gameObject.SetActive(true);
                });
                sequence.Append(timerGO.transform.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.2f, 0.25f));
                break;
            case State.TimeToRetreat:
                timeToRetreat.transform.localScale = Vector3.zero;
                Sequence sequence1 = DOTween.Sequence();
                sequence1.Append(timerGO.transform.DOScale(0f, 0.3f).SetEase(Ease.InCirc).OnComplete(() => timerGO.gameObject.SetActive(false)));
                sequence1.AppendCallback(() => timeToRetreat.SetActive(true));
                sequence1.Append(timeToRetreat.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f));
                baseIndicator.gameObject.SetActive(true);
                baseIndicator.UpdateIndicator();
                break;
            case State.Coming:
                timerFill.transform.DOScale(0f, 0.4f).SetEase(Ease.InCirc).OnComplete(() => timerFill.gameObject.SetActive(false));
                break;
        }
        state = targetState;
    }
}
