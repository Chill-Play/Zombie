using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsChest : MonoBehaviour
{
    public event System.Action OnStarsCollected;

    [SerializeField] int starCount = 10;
    [SerializeField] RewardStar starPrefab;
    [SerializeField] Vector2 radius;
    [SerializeField] float starsSpeed = 10f;

    Rigidbody body;
    bool spawned = false;
    int currentStarCount;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        currentStarCount = starCount;
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= 0f && !spawned)
        {
            transform.position = transform.position.SetY(0f);
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            SpawnStars();
            spawned = true;
        }
    }

    void SpawnStars()
    {
        for (int i = 0; i < starCount; i++)
        {
            RewardStar star = Instantiate<RewardStar>(starPrefab, transform.position, transform.rotation);
            float randomX = Random.Range(radius.x, radius.y);
            float randomY = Random.Range(radius.x, radius.y);
            float angle = i * Mathf.PI * 2f / starCount;
            Vector3 newPos = transform.position + new Vector3(Mathf.Cos(angle) * randomX, 0f, Mathf.Sin(angle) * randomY);
            Vector3 vel = BallisticHelper.CalculateVelocity(transform.position, newPos, starsSpeed);
            star.GetComponent<Rigidbody>().velocity = vel;
            star.OnStarCollected += Star_OnStarCollected;
        }
    }

    private void Star_OnStarCollected()
    {
        currentStarCount--;
        if (currentStarCount == 0)
        {
            OnStarsCollected?.Invoke();
        }
    }
}
