using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour, IInputReceiver
{
    public event System.Action<Unit> OnUnitAdd;
    public event System.Action OnPlayerUnitDead;

    [SerializeField] List<UnitMovement> units;
    [SerializeField] float unitRadius;
    [SerializeField] float unitCatchingDistance = 5f;
    [SerializeField] SubjectId movingToCarStateId;

    bool isMoving = false;
    bool movingToCar = false;

    int specialistCount  = 0;

    List<InteractivePointDetection> interactivePointDetections = new List<InteractivePointDetection>();
    List<bool> caughtUpSquad = new List<bool>();
    ReviveController reviveController;


    public List<UnitMovement> deadUnits = new List<UnitMovement>();

    public bool IsMoving => isMoving;
    public List<UnitMovement> Units => units;

    // Start is called before the first frame update
    void Start()
    {
        foreach (UnitMovement unit in units)
        {
            unit.GetComponent<UnitHealth>().OnDead += (x) => RemoveUnit(unit);
            caughtUpSquad.Add(true);
            interactivePointDetections.Add(unit.GetComponent<InteractivePointDetection>());
        }
        units[0].GetComponent<PlayerResources>().CanMoveToResources = false;
        units[0].GetComponent<UnitHealth>().OnDead += Squad_OnDead;
        reviveController = FindObjectOfType<ReviveController>();
        reviveController.OnRevive += Revive;
    }

    private void Squad_OnDead(EventMessage<Empty> obj)
    {
        OnPlayerUnitDead?.Invoke();
    }

    void RemoveUnit(UnitMovement unit)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == unit)
            {
                units.RemoveAt(i);
                interactivePointDetections.RemoveAt(i);
                caughtUpSquad.RemoveAt(i);
                deadUnits.Add(unit);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            units[0].GetComponent<UnitHealth>().TakeDamage(1000f, Vector3.forward);
        }

        if (units.Count > 0)
        {
            transform.position = units[0].transform.position;
        }

        if (movingToCar)
        {
            return;
        }

        for (int i = 1; i < units.Count; i++)
        {

            Vector3 targetPos = units[0].transform.position + GetPosition(i, unitRadius);
            Vector3 direction = targetPos - units[i].transform.position;
            if (direction.magnitude >= unitCatchingDistance && !caughtUpSquad[i])
            {
                interactivePointDetections[i].enabled = false;
                interactivePointDetections[i].NullTarget();
                units[i].MoveTo(targetPos);
            }
            else if (interactivePointDetections[i].enabled == false)
            {
                caughtUpSquad[i] = true;
                interactivePointDetections[i].enabled = true;
                units[i].StopMoving();
            }
            direction = Vector3.ClampMagnitude(direction * 2f, 1f);
            units[i].Input = new Vector2(direction.x, direction.z);
        }
    }


    public ResourcesInfo CollectResources()
    {        
        var backpack = FindObjectOfType<SquadBackpack>();
        return backpack.Resources;
    }

    public void AddSpecialist(Unit unit)
    {
        specialistCount++;
        unit.GetComponent<UnitHealth>().OnDead += (x) => specialistCount--;
        AddUnit(unit);
    }

    public void AddUnit(Unit unit)
    {
        unit.GetComponent<UnitHealth>().OnDead += (x) => RemoveUnit(unit.GetComponent<UnitMovement>());
        units.Add(unit.GetComponent<UnitMovement>());
        interactivePointDetections.Add(unit.GetComponent<InteractivePointDetection>());
        caughtUpSquad.Add(true);
        OnUnitAdd?.Invoke(unit);
    }

    Vector3 GetPosition(int index, float unitRadius)
    {
        int ring = -1;
        int circlesInRing = 1;
        int i = index;

        while (i >= 0)
        {
            ring++;
            circlesInRing = 6 * ring;
            if (ring == 0)
            {
                circlesInRing = 1;
            }
            i -= circlesInRing;
        }
        float angle = ((float)i / circlesInRing) * 360f;
        angle *= Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        pos *= unitRadius * ring * 2f;
        return pos;
    }

    public void GoToPosition(Vector3 position)
    {
        units[0].MoveTo(position);
    }

    public void MoveToCar(SpawnPoint spawnPoint, System.Action inCarCallback)
    {
        movingToCar = true;
        for (int i = 0; i < units.Count; i++)
        {
            if (i > 0)
            {
                units[i].GetComponent<StateController>().ToState(movingToCarStateId);
            }
            units[i].MoveTo(spawnPoint.EscapePoint.position);
        }
        StartCoroutine(ToCarMovement(spawnPoint, inCarCallback));
    }

    IEnumerator ToCarMovement(SpawnPoint spawnPoint, System.Action inCarCallback)
    {      
        List<UnitMovement> tempUnits = new List<UnitMovement>();
        int escapeUnitsCount = units.Count;
        Vector3 target = spawnPoint.EscapePoint.transform.position;
        for (int i = 0; i < units.Count; i++)
        {
            tempUnits.Add(units[i]);
        }

        while (tempUnits.Count > escapeUnitsCount - units.Count)
        {
            for (int i = tempUnits.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(tempUnits[i].transform.position, target) < 1f || tempUnits[i].IsReachDestination)
                {
                    tempUnits[i].gameObject.SetActive(false);
                    tempUnits.RemoveAt(i);
                    spawnPoint.SurvivorInCar();
                }
                else if (!tempUnits[i].VelocityActive)
                {
                    tempUnits[i].MoveTo(spawnPoint.EscapePoint.position);
                }
            }
            yield return null;
        }
        inCarCallback?.Invoke();
    }

    public void Revive()
    {
        CardsInfo activeCards = FindObjectOfType<CardController>().ActiveCards;   
        int unitsCount = units.Count + deadUnits.Count - activeCards.Count - 1;

        for (int i = 0; i < deadUnits.Count; i++)
        {
            Destroy(deadUnits[i].gameObject);
        }
        for (int i = 0; i < units.Count; i++)
        {
            Destroy(units[i].gameObject);
        }
        deadUnits.Clear();
        units.Clear();
        interactivePointDetections.Clear();
        caughtUpSquad.Clear();

        var reviveController = FindObjectOfType<ReviveController>();

        Unit mainDude = Instantiate(reviveController.MainSurvivor, transform.position, transform.rotation);
        AddUnit(mainDude);

        for (int i = 0; i < unitsCount; i++)
        {
            Unit instance = Instantiate(reviveController.PickupSurvivor, transform.position, transform.rotation);
            AddUnit(instance);
        }

       
        for (int i = 0; i < activeCards.cardSlots.Count; i++)
        {

            GameObject instance = Instantiate(activeCards.cardSlots[i].card.RaidUnitPrefab, transform.position, transform.rotation);
            AddSpecialist(instance.GetComponent<Unit>());
        }

        units[0].GetComponent<PlayerResources>().CanMoveToResources = false;
        units[0].GetComponent<UnitHealth>().OnDead += Squad_OnDead;

        Bomb bomb = Instantiate(reviveController.BombPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
        bomb.Detonate();
    }

    public void UpdateInput(Vector2 input)
    {
        input.Normalize();
        if (input.magnitude < 0.1f)
        {
            isMoving = false;
        }
        else
        {
            for (int i = 1; i < caughtUpSquad.Count; i++)
            {
                caughtUpSquad[i] = false;
            }
            isMoving = true;
        }
        if (units.Count > 0)
        {
            units[0].Input = input;
        }
    }
}
    
