using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour, IInputReceiver
{
    [SerializeField] List<UnitMovement> units;

    public List<UnitMovement> Units => units;
    // Start is called before the first frame update
    void Start()
    {
        //units = GetComponentsInChildren<UnitMovement>().ToList();
        foreach(UnitMovement unit in units)
        {
            unit.GetComponent<UnitHealth>().OnDead += (x) => units.Remove(unit.GetComponent<UnitMovement>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i < units.Count; i++)
        {
            Vector3 targetPos = units[0].transform.position + GetPosition(i, 1f);
            Vector3 direction = targetPos - units[i].transform.position;
            direction = Vector3.ClampMagnitude(direction * 2f, 1f);
            units[i].Input = new Vector2(direction.x, direction.z);
        }
        transform.position = units[0].transform.position;
    }


    public void SetInput(Vector2 input)
    {
        if (input.magnitude < 0.1f)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].GetComponent<PlayerResources>().IsSquadStoped = true; // remove GetComponent
            }
        }
        else
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].GetComponent<PlayerResources>().IsSquadStoped = false;
            }
        }
        units[0].Input = input;
    }


    public void AddUnit(Unit unit)
    {
        unit.GetComponent<UnitHealth>().OnDead += (x) => units.Remove(unit.GetComponent<UnitMovement>());
        units.Add(unit.GetComponent<UnitMovement>());
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
