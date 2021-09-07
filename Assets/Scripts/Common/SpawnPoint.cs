using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnPoint : MonoBehaviour
{
    public event System.Action OnReturnedToBase;

    [SerializeField] Transform carTransform;
    [SerializeField] Transform escapePoint;

    public Transform EscapePoint => escapePoint;

    bool isReturningToBase = false;
    Vector3 scale;
    Vector3 carScale;

    public bool IsReturningToBase { get => isReturningToBase; set { SetIsReturningToBase(value); } }


    private void Awake()
    {
        scale = transform.localScale;
        carScale = carTransform.localScale;
    }

    void SetIsReturningToBase(bool value)
    {
        isReturningToBase = value;
        if (value)
        {
            transform.DOPunchScale(scale * 0.1f, 0.5f, 1, 1f).SetLoops(-1);
        }
        else
        {
            transform.DOKill(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsReturningToBase && (other.GetComponent<SurvivorMovement>() != null))
        {
            OnReturnedToBase?.Invoke();
            //Level.Instance.EndLevel();
        }
    }

    public void SurvivorInCar()
    {
        carTransform.DOKill(true);
        carTransform.DOPunchScale(carScale * 0.1f, 0.5f);
    }
}
