using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    public float updateDirectionRate;
    public Transform baseTransform;
    public ParticleSystem bloodSplashPrefab;

    public float maxHealth;
    public float focusDuration;


    public float health;

    float focusTime;

    PlayerMovement player;
    Rigidbody rb;
    NavMeshAgent navMeshAgent;
    Animator animator;


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
        animator = GetComponentInChildren<Animator>();
    }

    void SetState(EnemyState newState)
    {
        if (newState == EnemyState.MoveToPlayer)
        {
            focusTime = Time.time;
        }
        state = newState;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void DealDamage(float damage)
    {
        if (health == 0)
            return;

        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            health = 0;
            navMeshAgent.enabled = false;
            slider.gameObject.SetActive(false);
            Invoke(nameof(DestroyGameObject), 5.0f);
            // Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Hit");
        }

        SetState(EnemyState.MoveToPlayer);
    }

    public void ReceiveBullet(float damage, Transform bullet)
    {
        if (health == 0)
            return;
        Instantiate(bloodSplashPrefab, bullet.position, bullet.rotation);
        rb.AddForce(bullet.forward * 10.0f);
        DealDamage(damage);
    }

    void Start()
    {
    }


    float speed;
    Vector3 lastPosition;
    void FixedUpdate()
    {
        if (health == 0)
            return;
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f);
        lastPosition = transform.position;
        animator.SetFloat("Speed", speed);
    }

    void Update()
    {
        if (health == 0)
            return;
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
            if (Vector3.Distance(transform.position, player.transform.position) < 2.0f)
            {
                animator.SetTrigger("Attack");
            }
            if (Vector3.Distance(transform.position, player.transform.position) < 1.0f)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("LoseScene");
            }
        }
    }
}
