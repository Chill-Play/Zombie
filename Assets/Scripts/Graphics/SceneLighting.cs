using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HSV
{
    public float h;
    public float s;
    public float v;
}

[ExecuteInEditMode]
public class SceneLighting : MonoBehaviour
{
    [SerializeField] Color shadowColor;
    [SerializeField] Color environmentColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_ShadowColor", new Vector4(shadowColor.r, shadowColor.g, shadowColor.b, 0.0f));
        Shader.SetGlobalColor("_RimColor", environmentColor);
    }
}
