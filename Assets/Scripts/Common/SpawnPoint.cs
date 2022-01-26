using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnPoint : SingletonMono<SpawnPoint>
{
    public event System.Action OnReturnedToBase;

    [SerializeField] Transform carTransform;
    [SerializeField] Transform escapePoint;
    [SerializeField] Transform escapeSquare;
    [SerializeField] Collider collider;

    public Transform EscapePoint => escapePoint;

    bool isReturningToBase = false;
    Vector3 scale;
    Vector3 carScale;

    public bool IsReturningToBase { get => isReturningToBase; set { SetIsReturningToBase(value); } }


    private void Awake()
    {
        escapeSquare.gameObject.SetActive(false);
        collider.enabled = false;
        scale = transform.localScale;
        carScale = carTransform.localScale;
    }

    void SetIsReturningToBase(bool value)
    {
        isReturningToBase = value;
        collider.enabled = value;
        escapeSquare.gameObject.SetActive(true);
        if (value)
        {
            escapeSquare.DOPunchScale(scale * 0.1f, 0.5f, 1, 1f).SetLoops(-1);
        }
        else
        {
            escapeSquare.DOKill(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SurvivorMovement>() != null)
        {            
            OnReturnedToBase?.Invoke();          
        }
    }

    public void SurvivorInCar()
    {
        carTransform.DOKill(true);
        carTransform.DOPunchScale(carScale * 0.1f, 0.5f);
    } 
}
