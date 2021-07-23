using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StateArea : MonoBehaviour
{
    [System.Serializable]
    public class StateAreaSettings
    {
        public string stateName;
        public Color stateColor = Color.blue;
    }

    [SerializeField] SceneReference stateBase;
    [SerializeField] StateAreaSettings stateAreaSettings;
    [SerializeField] TMP_Text stateText;
    [SerializeField] MeshRenderer meshRenderer;

    private void Awake()
    {
        stateText.text = stateAreaSettings.stateName;
        meshRenderer.material.SetColor("_Color", stateAreaSettings.stateColor);
    }

    public SceneReference StateBase => stateBase;

}
