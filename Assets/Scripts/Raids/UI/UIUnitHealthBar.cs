using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnitHealthBar : MonoBehaviour
{

    [SerializeField] Transform barTransform;
    [SerializeField] Transform barPivot;
    [SerializeField] Image barFill;
    [SerializeField] Image barBackFill;
    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text healthCountText;
    [SerializeField] float hideTimer = 2f;
    [SerializeField] bool unmovable = false;

    float backFillVelocity;
    UnitHealthBar unit;
    float upOffset;
    float currentHideTimer;
    bool appeared;

    Tween alphaTween;

    float smoothTime = 0.3f;
    Vector3 smoothVelocity;


    void Start()
    {
        barFill.fillAmount = 1f;
    }

    void Update()
    {
        if (appeared)
        {
            currentHideTimer -= Time.deltaTime;
            if (currentHideTimer <= 0.0f)
            {
                Hide();
            }
        }
        if (unit != null)
        {
            Vector3 unitWorldPosition = unit.transform.position;
            unitWorldPosition += Vector3.up * upOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(unitWorldPosition);
            if (unmovable)
            {
                barPivot.transform.position = screenPosition;
            }
            else
            {
                barPivot.transform.position = Vector3.SmoothDamp(barPivot.transform.position, screenPosition, ref smoothVelocity, smoothTime);
            }
        }
        barBackFill.fillAmount = Mathf.SmoothDamp(barBackFill.fillAmount, barFill.fillAmount, ref backFillVelocity, 0.3f);

    }

    
    public void Setup(UnitHealthBar unit, float upOffset)
    {
        this.unit = unit;
        unit.Damagable.GetComponent<IDamagable>().OnDamage += Unit_OnDamage;
        this.upOffset = upOffset;
        barFill.fillAmount = 1f;
        group.alpha = 0.0f;
        Hide();
    }


    public void Appear()
    {
        appeared = true;       
        Vector3 unitWorldPosition = unit.transform.position;
        unitWorldPosition += Vector3.up * upOffset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(unitWorldPosition);
        barPivot.transform.position = screenPosition;
        SmoothGroupAlpha(1f);
    }


    void Hide()
    {
        appeared = false;
        SmoothGroupAlpha(0f);
    }


    void SmoothGroupAlpha(float target)
    {
        group.alpha = target;
        if(alphaTween != null)
        {
            alphaTween.Complete();
        }
        alphaTween = DOTween.To(() => group.alpha, (x) => group.alpha = x, target, 0.3f);
    }


    private void Unit_OnDamage(DamageTakenInfo damageTakenInfo)
    {
        if (unit.enabled)
        {
            if (!appeared)
            {
                appeared = true;
                currentHideTimer = hideTimer;
                Appear();
            }
            barFill.fillAmount = damageTakenInfo.currentHealth / damageTakenInfo.maxHealth;
            if (healthCountText != null)
            {
                healthCountText.text = damageTakenInfo.currentHealth.ToString() + "/" + damageTakenInfo.maxHealth.ToString();
            }
        }
    }


    public void Remove()
    {
        if (unit != null)
        {
            IDamagable damagable = unit.Damagable.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.OnDamage -= Unit_OnDamage;
                unit = null;
                barTransform.DOScale(0f, 0.3f).SetEase(Ease.InCirc).OnComplete(() => Destroy(gameObject));
            }
        }
        
    }
}
