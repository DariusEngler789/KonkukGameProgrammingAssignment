using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{
    public float explosionRadius;
    public float explosionTimer;
    public float damage;
    public AudioSource impactAudioSource;
    public AudioSource explodeAudioSource;
    public GameObject explosionPrefab;

    float startTime;

    bool exploded = false;
    GameObject instantiatedExplosion;


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
            if (!exploded)
            {
                instantiatedExplosion = Instantiate(explosionPrefab, transform);
                explodeAudioSource.Play();
                exploded = true;
                var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                foreach (var collider in colliders)
                {
                    var enemy = collider.gameObject.GetComponent<EnemyFSM>();
                    if (enemy != null)
                    {
                        enemy.DealDamage(damage);
                    }
                }

                var renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in renderers)
                    renderer.enabled = false;
            }
        }
        if (Time.time - startTime > explosionTimer + 5 && exploded)
        {
            Destroy(instantiatedExplosion);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!exploded)
            impactAudioSource.Play();
    }
}
