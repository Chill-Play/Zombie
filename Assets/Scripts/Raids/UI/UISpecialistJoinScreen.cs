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
    [SerializeField] private Button button;
    [SerializeField] private Sprite newSpecialistIcon;

    private void Start()
    {
        header.transform.localScale = Vector3.zero;
        specialist.transform.localScale = Vector3.zero;
        button.transform.localScale = Vector3.zero;
        var seq = DOTween.Sequence();
        seq.Append(header.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.3f);
        seq.Join(specialist.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack)).AppendCallback(() =>
        {
            specialistSprite.sprite = newSpecialistIcon;
            specialist.SetBool("Show", true);
        });
        seq.AppendInterval(0.1f);
        seq.Append(button.transform.DOScale(new Vector3(1,1,1), .4f).SetEase(Ease.OutBack));
    }

    public void Close()
    {
        var seq = DOTween.Sequence();
        seq.Append(button.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.3f);
        seq.Join(specialist.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.1f);
        seq.Append(header.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack)).OnComplete(() =>
        {
            FindObjectOfType<Helicopter>().FlyAway();
            gameObject.SetActive(false);
        });
    }
}