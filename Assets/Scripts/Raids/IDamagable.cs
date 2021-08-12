using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    event System.Action<EventMessage<Empty>> OnDead;
    event System.Action<DamageTakenInfo> OnDamage;
    void Damage(DamageInfo info);
}
