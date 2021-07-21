using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GlobalMapUI : SingletonMono<GlobalMapUI>
{
    [SerializeField] Button moveButton;
    [SerializeField] Image darkImage;

    private void OnEnable()
    {        
        moveButton.onClick.AddListener(() => MoveButton_OnClick());
    }

    void MoveButton_OnClick()
    {
        moveButton.gameObject.SetActive(false);
        GlobalMapGame.Instance.MoveToNextState();
    }

    public void SetDark(float value, float duration, System.Action callback = null)
    {
        darkImage.gameObject.SetActive(true);
        darkImage.DOFade(value, duration).OnComplete(() => { callback(); darkImage.gameObject.SetActive(false); });
    }


    private void OnDisable()
    {
        
    }
}
