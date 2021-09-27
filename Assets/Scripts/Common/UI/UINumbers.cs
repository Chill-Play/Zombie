using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UINumber
{
    public TMP_Text text;
    public Vector3 worldPos;
    public Vector3 offset;
    public float lifeTime;
    public float t;

    public event System.Action OnEnd;

    public void End()
    {
        OnEnd?.Invoke();      
    }
}


public class UINumbers : MonoBehaviour
{
    [SerializeField] TMP_Text textPrefab;
    [SerializeField] Image imagePrefab;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float time = 0.4f;
    List<UINumber> numbers = new List<UINumber>();
    List<UINumber> positionUpdateNumbers = new List<UINumber>();
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = numbers.Count - 1; i >= 0; i--)
        {
            UINumber number = numbers[i];
            UpdateNumberPosition(number);
            if (number.t >= scaleCurve.keys[scaleCurve.keys.Length - 1].time)
            {
                Destroy(number.text.gameObject);
                numbers.RemoveAt(i);
            }
        }

        for (int i = 0; i < positionUpdateNumbers.Count; i++)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(positionUpdateNumbers[i].worldPos);
            positionUpdateNumbers[i].text.transform.position = screenPos + positionUpdateNumbers[i].offset;
        }
    }

    private void UpdateNumberPosition(UINumber number)
    {
        Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos);
        number.text.transform.position = screenPos + number.offset;
        number.t += Time.deltaTime / number.lifeTime;
        number.text.transform.localScale = Vector3.one * scaleCurve.Evaluate(number.t);
    }

    public void SpawnNumber(Vector3 worldPos, string text, Vector2 offset, float randomAngle, float randomOffset, float lifeTime = 0.4f)
    {
        UINumber uINumber = CreateNumberPrefab(worldPos, text, offset, randomAngle, randomOffset);
        uINumber.lifeTime = lifeTime;
        numbers.Add(uINumber);
        UpdateNumberPosition(uINumber);
    }


    public UINumber GetNumber(Vector3 worldPos, string text, Vector2 offset, float randomAngle, float randomOffset, bool updatePosition)
    {

        UINumber uINumber = CreateNumberPrefab(worldPos, text, offset, randomAngle, randomOffset);
        Vector3 screenPos = camera.WorldToScreenPoint(uINumber.worldPos);
        uINumber.text.transform.position = screenPos + uINumber.offset;
        if (updatePosition)
        {
            positionUpdateNumbers.Add(uINumber);
            uINumber.OnEnd += () => positionUpdateNumbers.Remove(uINumber);
        }
        return uINumber;
    }


    private UINumber CreateNumberPrefab(Vector3 worldPos, string text, Vector2 offset, float randomAngle, float randomOffset)
    {
        UINumber uINumber = new UINumber();
        uINumber.text = Instantiate(textPrefab, transform);
        uINumber.text.transform.SetAsFirstSibling();
        uINumber.text.transform.localEulerAngles = Vector3.forward * Random.Range(-randomAngle, randomAngle);
        uINumber.offset = offset + (Random.insideUnitCircle * randomOffset);
        uINumber.worldPos = worldPos;
        uINumber.text.text = text;
        return uINumber;
    }


    public void PunchScaleNumber(UINumber uINumber, float punch, float duration)
    {
        uINumber.text.transform.DOKill(true);
        uINumber.text.transform.DOPunchScale(uINumber.text.transform.localScale * punch, duration);
    }

    public void FollowPosition(UINumber number, Vector3 position)
    {
        number.worldPos = position;
        Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos);
        number.text.transform.position = screenPos + number.offset;
    }

    public void ScaleToZeroAndDestroy(UINumber uINumber, float duration)
    {
        uINumber.text.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InCirc).OnComplete(() => { uINumber.End(); Destroy(uINumber.text.gameObject); });
    }

    public void AttachImage(UINumber uINumber, Sprite sprite)
    {
        Image image =  Instantiate<Image>(imagePrefab, uINumber.text.transform);
        image.sprite = sprite;
    }

}
