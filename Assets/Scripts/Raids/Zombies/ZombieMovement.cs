using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : UnitMovement
{
    [SerializeField] float jumpHeightMod = 0.5f;
    Coroutine offMeshCoroutine;

    public Vector3 Velocity { get; set; }

    private void OnDisable()
    {
        if (offMeshCoroutine != null)
        {
            StopCoroutine(offMeshCoroutine);
        }
    }


    public override void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);      
    }

    public override void StopMoving()
    {
        if (agent.enabled)
        {
            agent.destination = transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.autoTraverseOffMeshLink = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.isOnOffMeshLink)
        {
            if (offMeshCoroutine == null)
            {
                var offMeshData = agent.currentOffMeshLinkData;
                if (offMeshData.offMeshLink != null && offMeshData.offMeshLink.gameObject.TryGetComponent<ZombieClimbPoint>(out var climbPoint))
                {
                    offMeshCoroutine = StartCoroutine(Climb());
                }
                else
                {
                    offMeshCoroutine = StartCoroutine(JumpFrom());
                }
            }
        }
        else
        {
            Velocity = agent.velocity;
        }
    }


    IEnumerator JumpFrom()
    {
        var offMeshData = agent.currentOffMeshLinkData;
        var start = offMeshData.startPos;
        var end = offMeshData.endPos;
        var distance = Vector3.Distance(start, end);
        while(Vector3.Distance(agent.transform.position, start) > 0.1f)
        {
            yield return new WaitForFixedUpdate();
            var lookDirection = end - agent.transform.position;
            lookDirection.y = 0.0f;
            lookDirection.Normalize();
            Velocity = lookDirection * agent.speed;
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(lookDirection), agent.angularSpeed * Time.fixedDeltaTime);
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, start, agent.speed * Time.fixedDeltaTime);
        }
        agent.GetComponentInChildren<Animator>().SetTrigger("Jump");
        var t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            var lerpedPos = Vector3.Lerp(start, end, t);
            var offset = Mathf.Sin(t * Mathf.PI) * distance/2f * jumpHeightMod;
            agent.transform.position = lerpedPos + Vector3.up * offset;
        }
        agent.GetComponentInChildren<Animator>().SetTrigger("Land");
        yield return new WaitForSeconds(2f);
        agent.CompleteOffMeshLink();
        offMeshCoroutine = null;
    }


    IEnumerator Climb()
    {
        var offMeshData = agent.currentOffMeshLinkData;
        var start = offMeshData.startPos;
        var end = offMeshData.endPos;
        var distance = Vector3.Distance(start, end);
        while (Vector3.Distance(agent.transform.position, start) > 0.1f)
        {
            yield return new WaitForFixedUpdate();
            var lookDirection = end - agent.transform.position;
            lookDirection.y = 0.0f;
            lookDirection.Normalize();
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(lookDirection), agent.angularSpeed * Time.fixedDeltaTime);
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, start, agent.speed * Time.fixedDeltaTime);
        }
        //agent.GetComponentInChildren<Animator>().SetTrigger("Jump");
        var t = 0.0f;
        var targetPos = start;
        targetPos.y = end.y;
        while (t < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            var lerpedPos = Vector3.Lerp(start, targetPos, t);
            agent.transform.position = lerpedPos;
        }
        t = 0;
        start = agent.transform.position;
        while (t < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            var lerpedPos = Vector3.Lerp(start, end, t);
            agent.transform.position = lerpedPos;
        }
        //agent.GetComponentInChildren<Animator>().SetTrigger("Land");
        agent.CompleteOffMeshLink();
        offMeshCoroutine = null;
    }

    public override bool CanReachDestination(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(destination, path);
    }
}
