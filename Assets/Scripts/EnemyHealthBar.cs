using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform barTransform;
    [SerializeField] Transform barPivot;
    [SerializeField] Image barFill;
    [SerializeField] Image barBackFill;

    float backFillVelocity;
    Enemy enemy;
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


    public void Appear(Enemy enemy, float upOffset)
    {
        this.enemy = enemy;
        this.upOffset = upOffset;
        enemy.GetComponent<IDamagable>().OnDamage += Enemy_OnDamage;
        barFill.fillAmount = 1f;
    }


    private void Enemy_OnDamage(DamageTakenInfo obj)
    {
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
