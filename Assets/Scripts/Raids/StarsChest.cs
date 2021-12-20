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
    [SerializeField] Animator animator;

    Rigidbody body;
    bool spawned = false;   
   [SerializeField] List<RewardStar> rewardStars = new List<RewardStar>();

    private void Awake()
    {
        body = GetComponent<Rigidbody>();        
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= 0f && !spawned)
        {
            transform.position = transform.position.SetY(0f);
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            animator.SetTrigger("Open");
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
            rewardStars.Add(star);
        }
    }

    private void Star_OnStarCollected(RewardStar rewardStar)
    {
        rewardStars.Remove(rewardStar);
        if (rewardStars.Count <= 0)
        {
            OnStarsCollected?.Invoke();
        }
    }

    public void PickupAll(Transform picker)
    {
        for (int i = rewardStars.Count - 1; i >= 0; i--)
        {
            rewardStars[i].PickupStar(picker);
        }
    }
}
