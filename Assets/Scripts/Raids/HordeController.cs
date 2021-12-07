using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordeController : MonoBehaviour
{
    [System.Serializable]
    public struct HordeMovementSettings
    {
        public HordeMovementSettings(bool active, float distance, float power)
        {
            this.active = active;
            this.distance = distance;
            this.power = power;
        }

        public bool active;
        public float distance;
        public float power;
    }

    [SerializeField] HordeMovementSettings flocking = new HordeMovementSettings(true, 5f, 0.9f);
    [SerializeField] HordeMovementSettings alignment = new HordeMovementSettings(true, 5f, 0.3f);
    [SerializeField] HordeMovementSettings avoidance = new HordeMovementSettings(true, 2f, 0.5f);
    [SerializeField] bool alignmentAlternative = false;
    [SerializeField] bool correctSpeed = false;

    List<ZombieMovement> zombieMovements = new List<ZombieMovement>();


    public void AddAgent(Enemy enemy)
    {
        ZombieMovement zombieMovement = enemy.GetComponent<ZombieMovement>();
        zombieMovements.Add(zombieMovement);
        enemy.GetComponent<UnitHealth>().OnDead += (x) => OnAgentDead(zombieMovement);
    }

    void OnAgentDead(ZombieMovement zombieMovement)
    {
        zombieMovements.Remove(zombieMovement);
    }

    private void Update()
    {
        for (int i = 0; i < zombieMovements.Count; i++)
        {
            Vector3 flockVel = flocking.active ? Flock(zombieMovements[i], flocking.distance, flocking.power) : Vector3.zero;
            Vector3 alignVel = alignment.active ? Align(zombieMovements[i], alignment.distance, alignment.power) : Vector3.zero;
            Vector3 avoidVel = avoidance.active ? Avoid(zombieMovements[i], avoidance.distance, avoidance.power) : Vector3.zero;
            Vector3 newVel = zombieMovements[i].Agent.desiredVelocity + flockVel + alignVel + avoidVel;
            zombieMovements[i].Agent.velocity = newVel;
            if (correctSpeed)
            {
                zombieMovements[i].Agent.velocity = CorrectSpeed(zombieMovements[i].Agent.speed / 2f, zombieMovements[i].Agent.speed, newVel);
            }
        }
    }

    Vector3 Flock(ZombieMovement zombieMovement, float distance, float power)
    {
        Vector3 center = Vector3.zero;
        int count = 0;
        for (int i = 0; i < zombieMovements.Count; i++)
        {
            if (Vector3.Distance(zombieMovement.transform.position, zombieMovements[i].transform.position) < distance)
            {
                center += zombieMovements[i].transform.position;
                count++;
            }
        }
        center /= (float)count;
        Vector3 dir = center - zombieMovement.transform.position;
        return dir.SetY(0f) * power;
    }

    Vector3 Align(ZombieMovement zombieMovement, float distance, float power)
    {
        Vector3 sumVel = Vector3.zero;
        int count = 0;
        for (int i = 0; i < zombieMovements.Count; i++)
        {
            if (Vector3.Distance(zombieMovement.transform.position, zombieMovements[i].transform.position) < distance)
            {
                sumVel += zombieMovements[i].Agent.desiredVelocity;
                count++;
            }
        }
        sumVel /= (float)count;
        if (alignmentAlternative)
        {
            sumVel -= zombieMovement.Agent.desiredVelocity;
        }
        return sumVel.SetY(0f) * power;
    }

    Vector3 Avoid(ZombieMovement zombieMovement, float distance, float power)
    {
        Vector3 sumCloseness = Vector3.zero;

        for (int i = 0; i < zombieMovements.Count; i++)
        {
            float dist = Vector3.Distance(zombieMovement.transform.position, zombieMovements[i].transform.position);
            if (dist < distance)
            {
                float closeness = distance - dist;
                Vector3 dir = zombieMovement.transform.position - zombieMovements[i].transform.position;
                sumCloseness += dir * closeness;
            }
        }
        return sumCloseness.SetY(0f) * power;
    }

    Vector3 CorrectSpeed(float minSpeed, float maxSpeed, Vector3 velocity)
    {
        Vector3 result =  velocity;
        float speed = velocity.magnitude;
        if (speed > maxSpeed)
        {
            result = (velocity / speed) * maxSpeed;
        }
        else if (speed < minSpeed)
        {
            result = (velocity / speed) * minSpeed;
        }
       
        result = result.SetY(0f);
        if (float.IsNaN(result.x))
        {
            result.x = 0f;
        }
        if (float.IsNaN(result.z))
        {
            result.z = 0f;
        }
        return result;
    }
}
