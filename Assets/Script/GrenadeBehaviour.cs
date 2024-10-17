using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{
    public float explosionRadius;
    public float explosionTimer;

    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > explosionTimer - 0.1f)
        {
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Color.red;
            }
        }
        if (Time.time - startTime > explosionTimer)
        {
            Destroy(gameObject);
            var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var collider in colliders)
            {
                var enemy = collider.gameObject.GetComponent<EnemyFSM>();
                if (enemy != null)
                    Destroy(enemy.gameObject);
            }
        }
    }
}
