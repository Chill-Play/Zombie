using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RaidFinishScreen : UIScreen
{
    const int OPPORTUNITY_TO_DOUBLE_MINIMAL_LEVEL = 2;
    const int OPPORTUNITY_TO_DOUBLE_PERIODICITY = 2;

    [SerializeField] Image background;
    [SerializeField] Transform upperPanel;
    [SerializeField] Transform collectedPanel;
    [SerializeField] Transform colletctedContent;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] Transform survivorsPanel;
    [SerializeField] TMP_Text survivorsLabel;
    [SerializeField] Transform doubleButton;
    [SerializeField] Transform continueButton;
    [SerializeField] Transform alternativeContinueButton;

    Squad squad;
    bool tutorialMode = false;

    //const string SURVIVORS_COUNT_PREFS = "M_Survivors_Count";

    private void Awake()
    {
        squad = FindObjectOfType<Squad>();
    }

    void OnEnable()
    {
        var resources = squad.CollectResources();
        Show(resources);
    }

    public void Show(ResourcesInfo resources)
    {
        bool doubleOpportunity = false;// ZombiesLevelController.Instance.LevelsPlayed >= OPPORTUNITY_TO_DOUBLE_MINIMAL_LEVEL && ZombiesLevelController.Instance.LevelsPlayed % OPPORTUNITY_TO_DOUBLE_PERIODICITY == 0 && AdvertisementManager.Instance.RewardedAvailable;

        survivorsLabel.text = "+" + (squad.Units.Count - 1);


        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        upperPanel.transform.localScale = Vector3.zero;
        collectedPanel.transform.localScale = Vector3.zero; 
        survivorsPanel.transform.localScale = Vector3.zero;
        doubleButton.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;
        alternativeContinueButton.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        
        sequence.Append((background as Graphic) .DOFade(0.6f, 0.2f));
        sequence.AppendInterval(0.1f);
        sequence.Append(upperPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.AppendInterval(0.1f);
        sequence.Append(collectedPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.AppendCallback(() => StartCoroutine(ShowCollectedResources(resources)));
        sequence.AppendInterval(resources.Slots.Count * 0.5f + 0.5f);
        sequence.Append(survivorsPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));

        continueButton.gameObject.SetActive(!doubleOpportunity);
        doubleButton.gameObject.SetActive(doubleOpportunity);
        alternativeContinueButton.gameObject.SetActive(doubleOpportunity);

        /*if (doubleOpportunity)
        {
            sequence.Append(doubleButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
            sequence.Append(alternativeContinueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        }
        else
        {          
           
        }*/
        sequence.Append(continueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        tutorialMode = FindObjectOfType<Tutorial>() != null;
    }


    IEnumerator ShowCollectedResources(ResourcesInfo resources)
    {
        foreach (ResourceSlot slot in resources.Slots)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, colletctedContent);
            bar.Setup(slot.type, 0);
            bar.UpdateValue(slot.count);
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void DoubleClicked()
    {
        AdvertisementManager.Instance.ShowRewardedVideo((result) =>
        {
            if (result)
            {
                CollectResources(2);
                ToBase();
            }
        }, "raid_end_double_reward"); 
    }

    public void CollectResources(int multiplier = 1) 
    {
        var resources = squad.CollectResources();
        var resourceController = ResourcesController.Instance;
        for (int i = 0; i < multiplier; i++)
        {
            resourceController.AddResources(resources);
        }
        resourceController.UpdateResources();
        SaveSquad();       
    }

    void ToBase()
    {
        if (!tutorialMode)
        {
            ZombiesLevelController.Instance.RaidFinished();
        }
        ZombiesLevelController.Instance.ToBase();
    }

    public void NoThanksClicked()
    {
        ZombiesLevelController.Instance.ToBase();
        /*CollectResources();
        if (ZombiesLevelController.Instance.LevelsPlayed > 1)
        {
            AdvertisementManager.Instance.TryShowInterstitial("raid_end_no_thanks");
        }
        ToBase();*/
    }

    void SaveSquad()
    {
        PlayerPrefs.SetInt("M_Survivors_Count", squad.Units.Count - 1);        
    }
}
