using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopSlot : MonoBehaviour
{
    [SerializeField] Image resourceIcon;
    [SerializeField] TMP_Text countLabel;
    [SerializeField] TMP_Text costLabel;

    System.Action<ShopProposal> onBuy;
    ShopProposal proposal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetProposal(ShopProposal proposal, System.Action<ShopProposal> onBuy)
    {
        resourceIcon.sprite = proposal.type.icon;
        costLabel.text = (proposal.pricePerUnit * proposal.count).ToString();
        countLabel.text = proposal.count.ToString();

        this.onBuy = onBuy;
        this.proposal = proposal;
    }


    public void Buy()
    {
        onBuy.Invoke(proposal);
    }
}
