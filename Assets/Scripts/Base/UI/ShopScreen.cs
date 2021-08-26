using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopProposal
{
    public ResourceType type;
    public int count;
    public int pricePerUnit;
}


public class ShopScreen : UIScreen
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform panel;
    [SerializeField] UIShopSlot slotPrefab;
    [SerializeField] Vector2Int countRange;
    [SerializeField] Vector2Int pricesRange;
    [SerializeField] ResourceType moneyResource;
    [SerializeField] List<ResourceType> tradableResources;
    [SerializeField] Transform content;

    List<UIShopSlot> slots = new List<UIShopSlot>();
    System.Action onClose;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateProposals();
    }


    void OnEnable()
    {
        ShowAnimation();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void GenerateProposals()
    {
        foreach(var resource in tradableResources)
        {
            var proposal = GenerateNewProposal(resource);
            var slot = Instantiate(slotPrefab, content);
            slot.SetProposal(proposal, (x) => {
                TryToBuy(x, slot);        
            });
            slots.Add(slot);
        }
    }


    ShopProposal GenerateNewProposal(ResourceType resource)
    {
        var proposal = new ShopProposal();
        proposal.pricePerUnit = Random.Range(pricesRange.x, pricesRange.y);
        proposal.count = Mathf.Min(ResourcesController.Instance.ResourcesCount.Count(resource), Random.Range(countRange.x, countRange.y));
        proposal.type = resource;
        return proposal;
    }

    void TryToBuy(ShopProposal proposal, UIShopSlot slot)
    {
        var price = proposal.pricePerUnit * proposal.count;
        var playerResources = ResourcesController.Instance.ResourcesCount;
        playerResources.Subtract(proposal.type, proposal.count);
        playerResources.Add(moneyResource, price);
        ResourcesController.Instance.UpdateResources();

        var new_proposal = GenerateNewProposal(proposal.type);
        slot.SetProposal(new_proposal, (x) => {
                TryToBuy(x, slot);        
        });
    }


    void ShowAnimation()
    {
        foreach(var card in slots)
        {
            card.transform.localScale = Vector3.zero;
        }
        canvasGroup.alpha = 0f;
        panel.transform.localScale = Vector3.one * 0.3f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic, 1.1f, 0.4f));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 1f, 0.25f).SetEase(Ease.OutSine));
        foreach (var card in slots)
        {
            sequence.Append(card.transform.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.05f, 0.25f));// = Vector3.zero;
        }

    }


    public void Close()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InCirc));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 0f, 0.25f).SetEase(Ease.InCirc)); 
        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false); //Refactor
            onClose?.Invoke();
        });
    }
}
