using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

class UIPopUpText
{
    public TMP_Text text;
    public Vector3 worldPos;
    public float t;
}

public class UIBuyingPopUpText : SingletonMono<UIBuyingPopUpText>
{
    [SerializeField] string[] textVariants;
    [SerializeField] TMP_Text textPrefab;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve moveUpCurve;
    [SerializeField] float moveUpDistance = 3f;
    [SerializeField] float time = 2f;
    List<UIPopUpText> numbers = new List<UIPopUpText>();
    Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = numbers.Count - 1; i >= 0; i--)
        {
            UIPopUpText number = numbers[i];
            Vector3 upOffset = Vector3.up * moveUpCurve.Evaluate(number.t) * moveUpDistance;
            Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos + upOffset);
            number.text.transform.position = screenPos;
            number.t += Time.deltaTime / time;
            number.text.transform.localScale = Vector3.one * scaleCurve.Evaluate(number.t);
            if (number.t >= scaleCurve.keys[scaleCurve.keys.Length - 1].time)
            {
                Destroy(number.text.gameObject);
                numbers.RemoveAt(i);
            }
        }
    }


    public void SpawnText(Vector3 worldPos)
    {
        UIPopUpText uINumber = new UIPopUpText();
        uINumber.text = Instantiate(textPrefab, transform);
        uINumber.text.transform.SetAsFirstSibling();
        uINumber.text.text = textVariants[Random.Range(0, textVariants.Length)];
        uINumber.worldPos = worldPos;
        numbers.Add(uINumber);
    }
}
