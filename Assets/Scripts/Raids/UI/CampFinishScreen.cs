using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CampFinishScreen : UIScreen
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
    [SerializeField] private Image specialistImage;
    [SerializeField] private TextMeshProUGUI specialistName;
    [SerializeField] private GameObject specialist;

    private Card card;

    Squad squad;
    bool tutorialMode = false;
    bool isCampaign = false; // temp
    Campaign campaign;

    //const string SURVIVORS_COUNT_PREFS = "M_Survivors_Count";

    private void Awake()
    {
        campaign = Campaign.Instance;
        isCampaign = campaign != null;
        squad = Squad.Instance;
    }

    void OnEnable()
    {
        var resources = squad.CollectResources();
        Show(resources);
    }

    public void Show(ResourcesInfo resources)
    {
        bool doubleOpportunity = true;//ZombiesLevelController.Instance.LevelsPlayed >= OPPORTUNITY_TO_DOUBLE_MINIMAL_LEVEL && ZombiesLevelController.Instance.LevelsPlayed % OPPORTUNITY_TO_DOUBLE_PERIODICITY == 0 && AdvertisementManager.Instance.RewardedAvailable;

        SaveSquad();

        ResourceBar bar = Instantiate(resourceBarPrefab, colletctedContent);
        bar.Setup(resources.Slots[0].type, 0);
        bar.transform.localScale = Vector3.zero;
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        upperPanel.transform.localScale = Vector3.zero;
        collectedPanel.transform.localScale = Vector3.zero;
        survivorsPanel.transform.localScale = Vector3.zero;
        doubleButton.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;
        alternativeContinueButton.transform.localScale = Vector3.zero;
        specialist.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();

        sequence.Append((background as Graphic).DOFade(0.6f, 0.2f));
        sequence.AppendInterval(0.1f);
        sequence.Append(upperPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.AppendInterval(0.1f);
        sequence.Append(collectedPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.Append(specialist.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.1f, .3f));
        sequence.Append(bar.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.1f, .3f).OnComplete(() =>
        {
            bar.UpdateValue(resources.Slots[0].count);
        }));



        continueButton.gameObject.SetActive(!doubleOpportunity);
        doubleButton.gameObject.SetActive(doubleOpportunity);
        alternativeContinueButton.gameObject.SetActive(doubleOpportunity);

        if (doubleOpportunity)
        {
            sequence.Append(doubleButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
            sequence.Append(alternativeContinueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        }

        sequence.Append(continueButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        tutorialMode = Tutorial.Instance != null;
    }


    public void DoubleClicked()
    {
        AdvertisementManager.Instance.ShowRewardedVideo((result) =>
        {
            if (result)
            {
                SaveCards();
                CollectResources(2);
                ToBase();
            }
        }, "camp_defense_end_double_reward");
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
    }

    void ToBase()
    {
        ZombiesLevelController.Instance.CampaignFinished();
        ZombiesLevelController.Instance.ToBase();
    }

    public void NoThanksClicked()
    {
        CollectResources();
        SaveCards();
        ToBase();
        AdvertisementManager.Instance.TryShowInterstitial("camp_defense_end_no_thanks");
    }

    void SaveSquad()
    {
        int count = Mathf.Clamp(squad.Units.Count - CardController.Instance.ActiveCards.Count - 1, 0, squad.Units.Count);
        survivorsLabel.text = "+" + count.ToString();
        PlayerPrefs.SetInt("M_Survivors_Count", count);
    }

    void SaveCards()
    {
        CardController.Instance.Save();
    }
}
