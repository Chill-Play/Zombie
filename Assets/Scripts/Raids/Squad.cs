using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour, IInputReceiver
{
    public event System.Action<Unit> OnUnitAdd;

    [SerializeField] List<UnitMovement> units;
    [SerializeField] float unitRadius;

    bool isMoving = false;

    public bool IsMoving => isMoving;
    public List<UnitMovement> Units => units;
    // Start is called before the first frame update
    void Start()
    {
        //units = GetComponentsInChildren<UnitMovement>().ToList();
        foreach(UnitMovement unit in units)
        { 
            unit.GetComponent<UnitHealth>().OnDead += (x) => units.Remove(unit.GetComponent<UnitMovement>());
        }
        units[0].GetComponent<PlayerResources>().CanMoveToResources = false;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i < units.Count; i++)
        {
            Vector3 targetPos = units[0].transform.position + GetPosition(i, unitRadius);
            Vector3 direction = targetPos - units[i].transform.position;
            direction = Vector3.ClampMagnitude(direction * 2f, 1f);
            units[i].Input = new Vector2(direction.x, direction.z);
        }
        transform.position = units[0].transform.position;
    }


    public Dictionary<ResourceType, int> CollectResources()
    {
        Dictionary<ResourceType, int> result = new Dictionary<ResourceType, int>();
        foreach(var unit in units)
        {
            var backpack = unit.GetComponent<PlayerBackpack>();
            if(backpack != null)
            {
                foreach (var pair in backpack.Resources)
                {
                    if(result.ContainsKey(pair.Key))
                    {
                        result[pair.Key] += pair.Value;
                    }
                    else
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }
        }
        return result;
    }


    public void SetInput(Vector2 input)
    {
        if (input.magnitude < 0.1f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        units[0].Input = input;
    }


    public void AddUnit(Unit unit)
    {       
        unit.GetComponent<UnitHealth>().OnDead += (x) => units.Remove(unit.GetComponent<UnitMovement>());
        units.Add(unit.GetComponent<UnitMovement>());
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
}
