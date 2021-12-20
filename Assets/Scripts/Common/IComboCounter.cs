using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComboCounter
{
    event System.Action<Sprite, int> OnAddingPoints;

}
