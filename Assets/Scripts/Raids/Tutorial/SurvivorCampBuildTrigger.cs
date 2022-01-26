using System;
using UnityEngine;

namespace Raids.Tutorial
{
    public class SurvivorCampBuildTrigger : ConditionTrigger
    {
        [SerializeField] private Construction construction;
        private NoiseController noiseController;
        private void Awake()
        {
            construction.OnBuild += ConstructionBuilded;
            noiseController = NoiseController.Instance;
        }

        void ConstructionBuilded()
        {
            noiseController.AddNoiseLevel(noiseController.MaxNoiseLevel);
            InvokeEvent();
        }
    }
}