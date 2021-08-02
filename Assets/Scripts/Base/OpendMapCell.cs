using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class OpendMapCell : MapCell
{
    [SerializeField] protected SellingMapCell sellingMapCellPrefab;
    [SerializeField] protected GameObject content;

    SellingMapCell sellingMapCell;

    public override void Load(JSONNode loadData)
    {
        base.Load(loadData);     
        if (!loadData.HasKey("sold") || !loadData["sold"].AsBool)
        {
            content.SetActive(false);
            sellingMapCell = Instantiate<SellingMapCell>(sellingMapCellPrefab, this.transform);
            sellingMapCell.GridId = GridId;
            sellingMapCell.OnOpening += SellingMapCell_OnOpening;            
            MapController.Instance.ReplaceMapCell(GridIndex, sellingMapCell);
            sellingMapCell.Load(loadData);         
        }  
    }

    protected virtual void SellingMapCell_OnOpening()
    {       
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
