using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Construction : MonoBehaviour , IZombiesLevelPhases
{   
    public event System.Action OnBuild;
    public event System.Action OnBreak;

    [SerializeField] MeshBounds ruins;
    [SerializeField] MeshBounds content;
    [SerializeField] MeshBounds destroyed;
    [SerializeField] int starsCount = 2;
    [SerializeField] bool constructed = false;

    UINumbers uiNumbers;
    ConstructionHealth constructionHealth;
    InteractivePoint interactivePoint;
    Constructive constructive;
    Repairable repairable;
    Sprite constructionIcon;


    public bool Constructed => constructed;

    public bool LockConstruction { get; set; } = false; 

    private void Awake()
    {       
        MeshBounds[] meshBounds = GetComponentsInChildren<MeshBounds>(true);
        foreach (var meshBound in meshBounds)
        {
            meshBound.CalculateBounds();
        }

        destroyed.gameObject.SetActive(false);
        content.gameObject.SetActive(constructed);
        ruins.gameObject.SetActive(!constructed);

        uiNumbers = FindObjectOfType<UINumbers>();        
        constructionHealth = content.GetComponent<ConstructionHealth>();
        if (constructed)
        {
            constructionHealth.SetHealth(constructionHealth.Health);
        }    
        constructionHealth.OnConstructed += ConstructionHealth_OnConstructed;
        constructionHealth.OnDead += ConstructionHealth_OnDead;

        interactivePoint = GetComponent<InteractivePoint>();
        constructive = GetComponent<Constructive>();
        repairable = GetComponent<Repairable>();
        constructionHealth.OnDamage += ConstructionHealth_OnDamage;
        constructionIcon = FindObjectOfType<ConstructionManager>().ConstructionIcon;
    }

    private void ConstructionHealth_OnDamage(DamageTakenInfo obj)
    {
        bool canInteract = (constructive != null && constructive.CanConstruct) || (repairable != null && repairable.CanRepair);
        interactivePoint.enabled = canInteract;
    }

    private void ConstructionHealth_OnDead(EventMessage<Empty> obj)
    {     
        constructed = false;
        LockConstruction = true;
        Sequence sequence = DOTween.Sequence();    
        sequence.Append(content.transform.DOLocalMoveY(content.transform.position.y - content.Bounds.size.y, 0.3f).SetEase(Ease.InCirc));
        sequence.AppendCallback(() =>
        {
            content.gameObject.SetActive(false);
            destroyed.transform.position = destroyed.transform.position.SetY(destroyed.transform.position.y - destroyed.Bounds.size.y);
            destroyed.gameObject.SetActive(true);
            OnBreak?.Invoke();
        });
        sequence.Append(destroyed.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.OutCirc));
    }

    private void ConstructionHealth_OnConstructed()
    {       
        constructed = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ruins.transform.DOLocalMoveY(ruins.transform.position.y - ruins.Bounds.size.y, 0.3f).SetEase(Ease.InCirc));
        sequence.AppendCallback(() =>
        {
            ruins.gameObject.SetActive(false);
            content.transform.position = content.transform.position.SetY(content.transform.position.y - content.Bounds.size.y);
            content.gameObject.SetActive(true);
            OnBuild?.Invoke();
        });
        sequence.Append(content.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.OutCirc)).OnComplete(() =>
        {
            ConstructionManager constructionManager = FindObjectOfType<ConstructionManager>();
            Squad squad = FindObjectOfType<Squad>();
            for (int i = 0; i < starsCount; i++)
            {
                PickupableResource star = Instantiate<PickupableResource>(constructionManager.RewardStarPrefab, transform.position + Vector3.up * (content.Bounds.max.y + 1f), transform.rotation);
                star.Pickup(squad.Units[0].transform);
            }
        }
        );
    }

    public float Construct(float value)
    {
        if (!constructed && !LockConstruction)
        {
            float dH = constructionHealth.AddHealth(value, true);  
            UINumber number = uiNumbers.GetNumber(transform.position + Vector3.up * 2f, "+" + dH, Vector2.zero, 0f, 0f, true);
            uiNumbers.AttachImage(number, constructionIcon);
            uiNumbers.MoveUpNumber(number, 40f, 0.8f, () => uiNumbers.End(number));
            return dH;
        }
        return 0f;
    }

    public float Repair(float value)
    {
        if (constructed && !LockConstruction)
        {
            float dH = constructionHealth.AddHealth(value);
            UINumber number = uiNumbers.GetNumber(transform.position + Vector3.up * 2f, "+" + dH, Vector2.zero, 0f, 0f, true);
            uiNumbers.AttachImage(number, constructionIcon);
            uiNumbers.MoveUpNumber(number, 40f, 0.8f, () => uiNumbers.End(number));
            return dH;
        }
        return 0f;
    }

    public void OnLevelStarted()
    {
        
    }

    public void OnLevelEnded()
    {
        
    }

    public void OnLevelFailed()
    {
        
    }

    public void OnHordeDefeated()
    {
        LockConstruction = true;
    }
}
