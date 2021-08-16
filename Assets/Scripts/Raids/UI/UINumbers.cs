using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


class UINumber
{
    public TMP_Text text;
    public Vector3 worldPos;
    public Vector3 offset;
    public float t;
}


public class UINumbers : MonoBehaviour
{
    [SerializeField] TMP_Text textPrefab;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float time = 0.4f;
    List<UINumber> numbers = new List<UINumber>();
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
    }

    private void UpdateNumberPosition(UINumber number)
    {
        Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos);
        number.text.transform.position = screenPos + number.offset;
        number.t += Time.deltaTime / time;
        number.text.transform.localScale = Vector3.one * scaleCurve.Evaluate(number.t);
    }

    public void SpawnNumber(Vector3 worldPos, string text, Vector2 offset, float randomAngle, float randomOffset)
    {
        UINumber uINumber = new UINumber();
        uINumber.text = Instantiate(textPrefab, transform);
        uINumber.text.transform.SetAsFirstSibling();
        uINumber.text.transform.localEulerAngles = Vector3.forward * Random.Range(-randomAngle, randomAngle);
        uINumber.offset = offset + (Random.insideUnitCircle * randomOffset);
        uINumber.worldPos = worldPos;
        uINumber.text.text = text;
        numbers.Add(uINumber);
        UpdateNumberPosition(uINumber);
    }
}
