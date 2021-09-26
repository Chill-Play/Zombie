using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowPlayer : MonoBehaviour
{
    [SerializeField] UnitMovement movement;
    [SerializeField] ZombieAgroSequence agroSequence;

    Squad squad;
    float nextAggro = 0.0f;
    float nextUpdateTime;
    UnitMeleeFighting meleeFighting;

    private void Awake()
    {
        meleeFighting = GetComponent<UnitMeleeFighting>();
    }

    private void Start()
    {
        squad = GameplayController.Instance.SquadInstance;
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        SetNextAggro();
    }

    private void SetNextAggro()
    {
        nextAggro = Time.time + Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time > nextAggro)
        //{
        //    if (!agroSequence.IsPlaying)
        //    {
        //        movement.StopMoving();
        //        float rand = Random.Range(0f, 100f);
        //        if (rand < 30f)
        //        {
        //            agroSequence.Play(() =>
        //            {
        //                SetNextAggro();
        //            });
        //        }
        //        else
        //        {
        //            SetNextAggro();
        //        }
        //    }
        //}
        //else
        {
            if (Time.timeSinceLevelLoad > nextUpdateTime)
            {
                if (squad != null && !meleeFighting.Attacking)
                {
                    nextUpdateTime = Time.timeSinceLevelLoad + Random.Range(0.2f, 0.6f);
                    movement.MoveTo(squad.transform.position);
                }
            }
        }
    }
}
