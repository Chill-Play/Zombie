using UnityEngine;

public class ZombieSpawnGameobjectOnDeath : MonoBehaviour
{
    [SerializeField] UnitHealth health;
    [SerializeField] GameObject gameObject;
    
    void OnEnable()
    {
        health.OnDead += Health_OnDead;
    }

    private void OnDisable()
    {
        health.OnDead -= Health_OnDead;
    }
    
    private void Health_OnDead(EventMessage<Empty> obj)
    {
        SpawnGameobject();
    }

    void SpawnGameobject()
    {
        Instantiate(gameObject, transform.position, Quaternion.identity);
    }
}