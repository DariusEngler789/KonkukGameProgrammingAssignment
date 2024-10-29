using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    public float speed;
    public float updateDirectionRate;
    public Transform baseTransform;
    public ParticleSystem bloodSplashPrefab;

    public float maxHealth;
    public float focusDuration;


    float health;

    float focusTime;

    PlayerMovement player;
    Rigidbody rb;
    NavMeshAgent navMeshAgent;


    Slider slider;

    enum EnemyState
    {
        MoveToPlayer,
        MoveToBase,
    };

    EnemyState state = EnemyState.MoveToBase;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerMovement>();
        health = maxHealth;
        slider = GetComponentInChildren<Slider>();
    }

    void SetState(EnemyState newState)
    {
        if (newState == EnemyState.MoveToPlayer)
        {
            focusTime = Time.time;
        }
        state = newState;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }

        SetState(EnemyState.MoveToPlayer);
    }

    public void ReceiveBullet(float damage, Transform bullet)
    {
        Instantiate(bloodSplashPrefab, bullet.position, bullet.rotation);
        rb.AddForce(bullet.forward * 10.0f);
        DealDamage(damage);
    }

    void Start()
    {
    }

    void Update()
    {
        slider.maxValue = maxHealth;
        slider.value = health;
        if (state == EnemyState.MoveToBase)
        {
            navMeshAgent.SetDestination(baseTransform.position);
        }
        else if (state == EnemyState.MoveToPlayer)
        {
            if ((Time.time - focusTime) > focusDuration)
            {
                SetState(EnemyState.MoveToBase);
            }
            navMeshAgent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) < 1.0f)
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
    }
}
