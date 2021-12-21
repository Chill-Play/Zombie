using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBounds : MonoBehaviour
{
    public Bounds Bounds { get; private set; }

    public void CalculateBounds()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        Debug.Log(meshRenderers.Length);
        if (meshRenderers.Length > 0)
        {
            var bounds = meshRenderers[0].bounds;
            foreach (var meshRenderer in meshRenderers)
            {
                bounds.Encapsulate(meshRenderer.bounds);
            }
            Bounds = bounds;          
        }
       
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Bounds.center, Bounds.size);
    }
}
