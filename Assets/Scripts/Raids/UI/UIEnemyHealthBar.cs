using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform barTransform;
    [SerializeField] Transform barPivot;
    [SerializeField] Image barFill;
    [SerializeField] Image barBackFill;

    float backFillVelocity;
    EnemyHealthBar enemy;
    float upOffset;


    void Start()
    {
        barFill.fillAmount = 1f;
    }

    private void PlayerInstance_OnTakeDamage(DamageTakenInfo info)
    {
        barFill.fillAmount = info.currentHealth / info.maxHealth;
        barTransform.DOShakePosition(0.2f, 30f, 50);
    }

    void Update()
    {
        if (enemy != null)
        {
            Vector3 playerWorldPosition = enemy.transform.position;
            playerWorldPosition += Vector3.up * upOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
            barPivot.transform.position = screenPosition;
        }
        barBackFill.fillAmount = Mathf.SmoothDamp(barBackFill.fillAmount, barFill.fillAmount, ref backFillVelocity, 0.3f);

    }

    
    public void Setup(EnemyHealthBar enemy, float upOffset)
    {
        this.enemy = enemy;
        enemy.GetComponent<IDamagable>().OnDamage += Enemy_OnDamage;
        this.upOffset = upOffset;
        barFill.fillAmount = 1f;
        barTransform.gameObject.SetActive(false);
    }


    public void Appear()
    {
        barTransform.gameObject.SetActive(true);
    }


    private void Enemy_OnDamage(DamageTakenInfo obj)
    {
        if(!barTransform.gameObject.activeSelf)
        {
            Appear();
        }
        barFill.fillAmount = obj.currentHealth / obj.maxHealth;
        barTransform.DOShakePosition(0.2f, 1f * obj.damage, 50);
    }


    public void Remove()
    {
        enemy.GetComponent<IDamagable>().OnDamage -= Enemy_OnDamage;
        enemy = null;
        barTransform.DOScale(0f, 0.3f).SetEase(Ease.InCirc).OnComplete(() => Destroy(gameObject));
    }
}
