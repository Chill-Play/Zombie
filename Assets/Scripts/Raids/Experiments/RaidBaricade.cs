using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBaricade : MonoBehaviour, IDamagable
{
    public event System.Action<RaidBaricade> OnBaricadeEnter;
    public event System.Action<RaidBaricade> OnBaricadeExit;
    
    public event Action<EventMessage<Empty>> OnDead;
    public event Action<DamageTakenInfo> OnDamage;

   
    [SerializeField] SubjectId sectionId;
    [SerializeField] SubjectId nextSectionId;
    [SerializeField] ResourceType resourceTool;
    [SerializeField] float health = 200f;
    [SerializeField] GameObject content;

    public ResourceType ResourceTool => resourceTool;
    public SubjectId SectionId => sectionId;
    public SubjectId NextSectionId => nextSectionId;

    private void OnTriggerEnter(Collider other)
    {
        PlayerTools playerTools = other.GetComponent<PlayerTools>();
        if (playerTools != null)
        {
            OnBaricadeEnter?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTools playerTools = other.GetComponent<PlayerTools>();
        if (playerTools != null)
        {
            OnBaricadeExit?.Invoke(this);
        }
    }

    public void Damage(DamageInfo info)
    {
        health -= info.damage;
        if (health <= 0f)
        {
            OnDead?.Invoke(new EventMessage<Empty>(new Empty(),this));
            Destroy(gameObject);
        }
    }

    public void HideContent()
    {
        content.SetActive(false);
    }
}
