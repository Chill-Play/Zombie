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
    [SerializeField] UseToolButton useToolButton;
    [SerializeField] ResourcesInfoUIPanel resourcesInfo;
    [SerializeField] BaseIndicatorUI baseIndicator;
    [SerializeField] bool tutorialMode = false;

    State state;
    PlayerTools playerTools;

    // Start is called before the first frame update
    void Start()
    {
        if (Level.Instance != null)
        {
            Level.Instance.OnNoiseLevelChanged += Level_OnNoiseLevelChanged;
            Level.Instance.OnNoiseLevelExceeded += Level_OnNoiseLevelExceeded;
            Level.Instance.OnHordeDefeated += Level_OnHordeDefeated;
            Level.Instance.OnResetNoise += Level_OnResetNoise; ;
        }
        noiseBar.SetValue(0f);
        
        baseIndicator.gameObject.SetActive(false);
        if (tutorialMode)
        {
            noiseBar.gameObject.SetActive(false);
        }

        FindObjectOfType<SquadBackpack>().OnPickupResource += Backpack_OnPickupResource;

        if (tutorialMode)
        {
            FindObjectOfType<TutorialHealper>().OnEscapeTrigger += Tutorial_OnEscapeTrigger;
        }

        BaricadeController baricadeController = FindObjectOfType<BaricadeController>();
        baricadeController.OnBaricadeEnter += BaricadeController_OnBaricadeEnter; 
        baricadeController.OnBaricadeExit += BaricadeController_OnBaricadeExit;

        useToolButton.gameObject.SetActive(false);
        useToolButton.OnToolUsed += UseToolButton_OnToolUsed;

        playerTools = FindObjectOfType<PlayerTools>();
    }

    private void Level_OnResetNoise()
    {
        SwitchState(State.NoiseBar);
    }

    private void UseToolButton_OnToolUsed(ResourceType obj)
    {
        playerTools.UseTool(obj);
        useToolButton.HideButton();
    }

    private void BaricadeController_OnBaricadeEnter(RaidBaricade baricade, bool usable)
    {
        useToolButton.ShowButton(baricade.ResourceTool, usable);
    }

    private void BaricadeController_OnBaricadeExit(RaidBaricade obj)
    {
        useToolButton.HideButton();
    } 

    private void Tutorial_OnEscapeTrigger()
    {
        baseIndicator.gameObject.SetActive(true);
        baseIndicator.UpdateIndicator();
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
        noiseBar.transform.DOKill(true);
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
                timeToRetreat.SetActive(false);
                noiseBar.gameObject.SetActive(true);
                timerGO.gameObject.SetActive(false);
                baseIndicator.gameObject.SetActive(false);
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
