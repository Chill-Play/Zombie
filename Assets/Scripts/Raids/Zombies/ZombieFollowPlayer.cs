using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowPlayer : MonoBehaviour
{
    [SerializeField] UnitMovement movement;
    [SerializeField] ZombieAgroSequence agroSequence;
    
    float nextAggro = 0.0f;
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
        if (Time.time > nextAggro)
        {
            if(!agroSequence.IsPlaying)
            {
                movement.StopMoving();
                agroSequence.Play(() =>
                {
                    SetNextAggro();
                });
            }
        }
        else
        {
            var squad = GameplayController.Instance.SquadInstance;
            if (squad != null)
            {
                GetComponent<UnitMovement>().MoveTo(squad.transform.position);
            }
        }
    }
}
