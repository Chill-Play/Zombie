using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitHealthBar : MonoBehaviour
{
    const float HIDE_TIME = 2.0f;

    [SerializeField] Transform barTransform;
    [SerializeField] Transform barPivot;
    [SerializeField] Image barFill;
    [SerializeField] Image barBackFill;
    [SerializeField] CanvasGroup group;

    float backFillVelocity;
    UnitHealthBar unit;
    float upOffset;
    float hideTimer;
    bool appeared;

    Tween alphaTween;

    float smoothTime = 0.3f;
    Vector3 smoothVelocity;


    void Start()
    {
        barFill.fillAmount = 1f;
    }

    private void PlayerInstance_OnTakeDamage(DamageTakenInfo info)
    {      
        barFill.fillAmount = info.currentHealth / info.maxHealth;
       // barTransform.DOShakePosition(0.2f, 30f, 50);
    }

    void Update()
    {
        if (appeared)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0.0f)
            {
                Hide();
            }
        }
        if (unit != null)
        {
            Vector3 playerWorldPosition = unit.transform.position;
            playerWorldPosition += Vector3.up * upOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
            barPivot.transform.position = Vector3.SmoothDamp(barPivot.transform.position, screenPosition, ref smoothVelocity, smoothTime);
        }
        barBackFill.fillAmount = Mathf.SmoothDamp(barBackFill.fillAmount, barFill.fillAmount, ref backFillVelocity, 0.3f);

    }

    
    public void Setup(UnitHealthBar unit, float upOffset)
    {
        this.unit = unit;
        unit.GetComponent<IDamagable>().OnDamage += Unit_OnDamage;
        this.upOffset = upOffset;
        barFill.fillAmount = 1f;
        group.alpha = 0.0f;
    }


    public void Appear()
    {
        appeared = true;
        SmoothGroupAlpha(1f);
    }


    void Hide()
    {
        appeared = false;
        SmoothGroupAlpha(0f);
    }


    void SmoothGroupAlpha(float target)
    {

        if(alphaTween != null)
        {
            alphaTween.Complete();
        }
        alphaTween = DOTween.To(() => group.alpha, (x) => group.alpha = x, target, 0.3f);
    }


    private void Unit_OnDamage(DamageTakenInfo obj)
    {      
        if (!appeared)
        {
            appeared = true;
            hideTimer = HIDE_TIME;
            Appear();
        }
        barFill.fillAmount = obj.currentHealth / obj.maxHealth;
        //barTransform.DOShakePosition(0.2f, 1f * obj.damage, 50);
       // barTransform.DOShakePosition(0.2f, 30f, 50);
    }


    public void Remove()
    {
        unit.GetComponent<IDamagable>().OnDamage -= Unit_OnDamage;
        unit = null;
        barTransform.DOScale(0f, 0.3f).SetEase(Ease.InCirc).OnComplete(() => Destroy(gameObject));
    }
}
