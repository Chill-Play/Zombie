using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;

public class OpenedMapCell : MapCell
{
    [SerializeField] List<CostInfo> cost = new List<CostInfo>();
    [SerializeField] protected SellingMapCell sellingMapCellPrefab;
    [SerializeField] protected GameObject content;
    [SerializeField] protected GameObject junkContent;

    SellingMapCell sellingMapCell;

    public override void Load(JSONNode loadData)
    {
        base.Load(loadData);

        junkContent.SetActive(false);
        if (!loadData.HasKey("sold") || !loadData["sold"].AsBool)
        {
            content.SetActive(false);
            junkContent.SetActive(true);
            sellingMapCell = Instantiate<SellingMapCell>(sellingMapCellPrefab, this.transform);
            sellingMapCell.SaveId = SaveId;
            sellingMapCell.SetupCost(cost);            
            sellingMapCell.OnOpening += SellingMapCell_OnOpening;            
            MapController.Instance.ReplaceMapCell(GridIndex, sellingMapCell);
            sellingMapCell.Load(loadData);         
        }  
    }

    protected virtual void SellingMapCell_OnOpening()
    {
        junkContent.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCirc).OnComplete(OnFinishHidingJunk);
    }

    void OnFinishHidingJunk()
    {
        junkContent.SetActive(false);
        sellingMapCell.gameObject.SetActive(false);
        content.SetActive(true);
        MapController.Instance.Save(this);
        Build((x) => MapController.Instance.ReplaceMapCell(GridIndex, x, true));
        sellingMapCell.OnOpening -= SellingMapCell_OnOpening;
    }

    public override JSONNode GetSaveData()
    {     
        JSONNode jsonNode = base.GetSaveData();
        jsonNode.Add("sold", true);
        return jsonNode;
    }
}
