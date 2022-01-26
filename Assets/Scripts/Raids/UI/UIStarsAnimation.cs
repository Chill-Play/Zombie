using System;
using UnityEngine;

public class UIStarsAnimation : MonoBehaviour
{
    [SerializeField] private Animator[] stars;
    [SerializeField] private float duration;
    [SerializeField] private int count;
    private float timer;
    private int currentStar;

    private void Start()
    {
        timer = duration;
    }

    private void Update()
    {
        LaunchStars();
    }

    public void LaunchStars()
    {
        if (timer <= 0)
        {
            for (int i = 0; i < count; i++)
                stars[(i + currentStar) % stars.Length].gameObject.SetActive(true);
            timer = duration;
            currentStar += count;
        }
        else
            timer -= Time.deltaTime;
    }
}