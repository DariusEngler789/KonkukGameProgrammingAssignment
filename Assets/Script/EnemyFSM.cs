using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public float speed;
    public float updateDirectionRate;


    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ChangeTargetDirection), 0, updateDirectionRate);
    }

    void ChangeTargetDirection()
    {
        moveDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime * speed);
    }
}
