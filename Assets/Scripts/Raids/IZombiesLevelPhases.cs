using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZombiesLevelPhases 
{
    void OnLevelStarted();
    void OnLevelEnded();
    void OnLevelFailed();
    void OnHordeDefeated();
}
