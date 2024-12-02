using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyBullet), lifeTime);
        Invoke(nameof(DestroyAudioSource), 1f);
    }

    void DestroyAudioSource()
    {
        Destroy(GetComponent<AudioSource>());
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyFSM>();
        if (enemy != null)
        {
            enemy.ReceiveBullet(damage, transform);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
