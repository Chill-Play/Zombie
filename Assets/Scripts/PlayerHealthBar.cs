using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Transform barTransform;
    [SerializeField] Transform barPivot;
    [SerializeField] Image barFill;
    [SerializeField] Image barBackFill;

    float backFillVelocity;


    void Start()
    {
        GameplayController.Instance.playerInstance.OnTakeDamage += PlayerInstance_OnTakeDamage;
        barFill.fillAmount = 1f;
    }

    private void PlayerInstance_OnTakeDamage(PlayerDamageInfo info)
    {
        barFill.fillAmount = info.currentHealth / info.maxHealth;
        barTransform.DOShakePosition(0.2f, 30f, 50);
    }

    void Update()
    {
        Vector3 playerWorldPosition = GameplayController.Instance.playerInstance.transform.position;
        playerWorldPosition += Vector3.up * 1.2f;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
        barPivot.transform.position = screenPosition;
        barBackFill.fillAmount = Mathf.SmoothDamp(barBackFill.fillAmount, barFill.fillAmount, ref backFillVelocity, 0.3f);
    }
}
