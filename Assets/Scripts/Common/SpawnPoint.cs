using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnPoint : MonoBehaviour
{
    public event System.Action OnReturnedToBase;

    [SerializeField] SubjectId sectionId;
    [SerializeField] Transform carTransform;
    [SerializeField] Transform escapePoint;
    [SerializeField] GameObject content;

    public Transform EscapePoint => escapePoint;

    bool isReturningToBase = false;
    Vector3 scale;
    Vector3 carScale;

    public bool IsReturningToBase { get => isReturningToBase; set { SetIsReturningToBase(value); } }

    public SubjectId SectionId => sectionId;

    private void Awake()
    {
        scale = transform.localScale;
        carScale = carTransform.localScale;
    }

    void SetIsReturningToBase(bool value)
    {
        Debug.Log(gameObject.name + " || " + value);
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
        }
    }

    public void SurvivorInCar()
    {
        carTransform.DOKill(true);
        carTransform.DOPunchScale(carScale * 0.1f, 0.5f);
    }

    public void SetVisable(bool value)
    {
        content.SetActive(value);
    }

    public void Hide()
    {
        content.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCirc).OnComplete(()=> content.SetActive(false));        
    }

    public void Show()
    {
        content.SetActive(true);
        content.transform.localScale = Vector3.zero;
        content.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc);
        
    }
}
