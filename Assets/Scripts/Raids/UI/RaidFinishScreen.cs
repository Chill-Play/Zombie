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

    public void Show(Dictionary<ResourceType, int> resources)
    {
        LevelInfo levelInfo = Level.Instance.GetLevelInfo();
        bool doubleOpportunity = levelInfo.levelNumber >= OPPORTUNITY_TO_DOUBLE_MINIMAL_LEVEL && levelInfo.levelNumber % OPPORTUNITY_TO_DOUBLE_PERIODICITY == 0 && AdvertisementManager.Instance.RewardedAvailable;

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
        sequence.AppendInterval(resources.Count * 0.5f + 0.5f);
        sequence.Append(survivorsPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));

        continueButton.gameObject.SetActive(!doubleOpportunity);
        doubleButton.gameObject.SetActive(doubleOpportunity);
        alternativeContinueButton.gameObject.SetActive(doubleOpportunity);

        if (doubleOpportunity)
        {
            sequence.Append(doubleButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
            sequence.Append(alternativeContinueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        }
        else
        {          
            sequence.Append(continueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        }

        tutorialMode = FindObjectOfType<Tutorial>() != null;
    }


    IEnumerator ShowCollectedResources(Dictionary<ResourceType, int> resources)
    {
        var copiedResources = resources.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
        foreach (KeyValuePair<ResourceType, int> pair in copiedResources)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, colletctedContent);
            bar.Setup(pair.Key, 0);
            bar.UpdateValue(pair.Value);
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
            }
        }, "raid_end_double_reward"); 
    }

    public void CollectResources(int multiplier = 1) //bad method, does more that its called
    {
        var info = new ResourcesInfo();
        var resources = FindObjectOfType<SquadBackpack>().Resources;
        var resourceController = ResourcesController.Instance;
        foreach (var pair in resources)
        {
            info.AddSlot(pair.Key, pair.Value);
        }
        for (int i = 0; i < multiplier; i++)
        {
            resourceController.AddResources(info);
        }       
        resourceController.UpdateResources();
        SaveSquad();
        if (!tutorialMode)
        {
            AnalyticsManager.Instance.OnLevelCompleted(Level.Instance.GetLevelInfo(), Level.Instance.Tries);
        }
        LevelController.Instance.ToBase(true);
    }


    public void NoThanksClicked()
    {
        CollectResources();
        if (LevelController.Instance.CurrentLevel > 1)
        {
            AdvertisementManager.Instance.TryShowInterstitial("raid_end_no_thanks");
        }
    }

    void SaveSquad()
    {
        PlayerPrefs.SetInt("M_Survivors_Count", squad.Units.Count - 1);        
    }
}
