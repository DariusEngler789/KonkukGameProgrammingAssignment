using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public float speed;
    public float updateDirectionRate;


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
        transform.forward = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * Time.fixedDeltaTime * speed;
    }
}
