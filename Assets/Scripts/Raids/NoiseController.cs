using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : SingletonMono<NoiseController>, INoiseListener
{
    public event System.Action<float> OnNoiseLevelChanged;
    public event System.Action OnNoiseLevelExceeded;

    [SerializeField] float maxNoiseLevel = 100f;

    float noiseLevel;
    bool noiseLevelExceeded = false;

    public float MaxNoiseLevel => maxNoiseLevel;

    public void AddNoiseLevel(float noise)
    {
        if (noiseLevelExceeded) return;

        noiseLevel += noise;
        if (noiseLevel >= maxNoiseLevel)
        {
            noiseLevelExceeded = true;
            noiseLevel = maxNoiseLevel;
            OnNoiseLevelExceeded?.Invoke();
        }
        OnNoiseLevelChanged?.Invoke(noiseLevel);
    }
}
