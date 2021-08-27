using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidFinishScreen : UIScreen
{
    [SerializeField] Image background;
    [SerializeField] Transform upperPanel;
    [SerializeField] Transform collectedPanel;
    [SerializeField] Transform colletctedContent;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] Transform doubleButton;
    [SerializeField] Transform noThanksButton;

    Squad squad;

    private void Awake()
    {
        squad = FindObjectOfType<Squad>();
    }

    void OnEnable()
    {
        var resources = squad.CollectResources();
        Show(resources);
    }

    public void Show(Dictionary<ResourceType, int> resources)
    {
        
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        upperPanel.transform.localScale = Vector3.zero;
        collectedPanel.transform.localScale = Vector3.zero;
        doubleButton.transform.localScale = Vector3.zero;
        noThanksButton.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        
        sequence.Append((background as Graphic) .DOFade(0.6f, 0.2f));
        sequence.AppendInterval(0.1f);
        sequence.Append(upperPanel.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.2f, 0.2f));
        sequence.AppendInterval(0.1f);
        sequence.Append(collectedPanel.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.2f, 0.2f));
        sequence.AppendCallback(() => StartCoroutine(ShowCollectedResources(resources)));
        sequence.AppendInterval(resources.Count * 0.5f + 0.5f);
        sequence.Append(doubleButton.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.2f, 0.2f));
        sequence.AppendInterval(2f);
        sequence.Append(noThanksButton.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.2f, 0.2f));
    }


    IEnumerator ShowCollectedResources(Dictionary<ResourceType, int> resources)
    {
        foreach (KeyValuePair<ResourceType, int> pair in resources)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, colletctedContent);
            bar.Setup(pair.Key, 0);
            bar.UpdateValue(pair.Value);
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void DoubleClicked()
    {
        var info = new ResourcesInfo();
        var resources = FindObjectOfType<SquadBackpack>().Resources;
        var resourceController = ResourcesController.Instance;
        foreach(var pair in resources)
        {
            info.AddSlot(pair.Key, pair.Value);
        }
        resourceController.AddResources(info);
        resourceController.AddResources(info);
        resourceController.UpdateResources();
        LevelController.Instance.ToBase(true);
    }


    public void NoThanksClicked()
    {
        var info = new ResourcesInfo();
        var resources = FindObjectOfType<SquadBackpack>().Resources;
        var resourceController = ResourcesController.Instance;
        foreach (var pair in resources)
        {
            info.AddSlot(pair.Key, pair.Value);
        }
        resourceController.AddResources(info);
        resourceController.UpdateResources();
        LevelController.Instance.ToBase(true);
    }
}
