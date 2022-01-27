using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class UISpecialistJoinScreen : MonoBehaviour
{
    [SerializeField] private GameObject header;
    [SerializeField] private Animator specialist;
    [SerializeField] private Image specialistSprite;
    [SerializeField] private Image hideSpecialistSprite;
    [SerializeField] private Image background;
    [SerializeField] private Button button;
    [SerializeField] private Campaign campaign;
    [SerializeField] private GameObject noiseCanvas;
    
    private void Start()
    {
        noiseCanvas.SetActive(false);
        header.transform.localScale = Vector3.zero;
        specialist.transform.localScale = Vector3.zero;
        button.transform.localScale = Vector3.zero;
        float a = background.color.a;
        background.color = new Color(0, 0, 0, 0);
        header.SetActive(true);
        background.gameObject.SetActive(true);
        specialist.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        hideSpecialistSprite.sprite = campaign.RewardCards[0].card.HideNewSpecialistIcon;
        var seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(DOTween.ToAlpha(() => background.color, x => background.color = x, a, .25f));
        seq.AppendInterval(0.5f);
        seq.Append(header.transform.DOScale(new Vector3(1, 1, 1), .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.25f);
        seq.Append(specialist.transform.DOScale(new Vector3(1, 1, 1), .3f).SetEase(Ease.OutBack)).AppendCallback(() =>
          {
              specialistSprite.sprite = campaign.RewardCards[0].card.NewSpecialistIcon;
              specialist.SetBool("Show", true);
          });
        seq.AppendInterval(3f);
        seq.Append(button.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutBack));
    }

    public void Close()
    {
        Helicopter.Instance.FlyAway();
        var seq = DOTween.Sequence();
        seq.Append(button.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InBack));
        seq.AppendInterval(0.3f);
        seq.Join(specialist.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InBack));
        seq.Join(header.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InBack));
        seq.Append(DOTween.ToAlpha(() => background.color, x => background.color = x, 0, .25f).OnComplete(() =>
        {
            noiseCanvas.SetActive(true);
            gameObject.SetActive(false);
        }));
    }
}