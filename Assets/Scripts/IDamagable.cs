using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public event System.Action<EventMessage<Empty>> OnDead;
    public event System.Action<DamageTakenInfo> OnDamage;
    void Damage(DamageInfo info);
}
