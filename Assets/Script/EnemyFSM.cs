using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public float speed;
    public float updateDirectionRate;
    public Transform baseTransform;


    Rigidbody rb;

    Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        InvokeRepeating(nameof(ChangeTargetDirection), 0, updateDirectionRate);
    }

    void ChangeTargetDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        Vector3 randomDir = new Vector3(x, 0, z);
        Vector3 baseDir = baseTransform.position - transform.position;
        baseDir.y = 0;

        transform.forward = (randomDir.normalized + baseDir.normalized * 4.0f).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * Time.fixedDeltaTime * speed;
    }
}
