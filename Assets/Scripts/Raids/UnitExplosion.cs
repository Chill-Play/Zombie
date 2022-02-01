using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UnitExplosion : UnitFighting
{
    [SerializeField] private Color explosionColor;
    [SerializeField] private SkinnedMeshRenderer zombie;
    [SerializeField] UnitTargetDetection unitTargetDetection;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionTime = 1;
    
    private bool explosion = true;
    private Renderer renderer;
    private UnitHealth unitHealth;
    private Vector3 cameraPos;

    private void Awake()
    {
        unitHealth = GetComponent<UnitHealth>();
        unitHealth.OnDead += Stop;
        renderer = GetComponent<Renderer>();
        cameraPos = Camera.main.transform.localPosition;
    }

    private void Update()
    {
        if (unitTargetDetection.Target != null && explosion)
        {
            explosion = false;
            zombie.material.DOColor(explosionColor, "_MainColor", 0.5f);
            StartCoroutine(OnExplosionCoroutine());
        }
    }
    
    IEnumerator OnExplosionCoroutine()
    {
        yield return new WaitForSeconds(explosionTime);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        unitHealth.TakeDamage(10000000, Vector3.zero);
        Camera.main.DOShakePosition(.5f, .75f).OnComplete(() =>
        {
            Camera.main.transform.localPosition = cameraPos;
        });

    }


    void Stop(EventMessage<Empty> empty)
    {
        StopAllCoroutines();
    }
}