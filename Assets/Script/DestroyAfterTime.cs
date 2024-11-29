using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        Invoke(nameof(DestroyGameObject), time);
    }
}
