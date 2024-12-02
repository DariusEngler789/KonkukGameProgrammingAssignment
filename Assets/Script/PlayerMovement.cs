using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashDuration;
    public float dashKeyTimeout;
    public float dashTimeout;
    public float rotationSpeed;
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform bulletShootPos;
    public Camera camera;
    public float bulletSpeed;
    public float grenadeSpeed;
    public Volume pp;
    public GameObject muzzleShot;

    Vector2 movementInput;
    float rotationInput;
    float dashTime;
    Vector3 dashDirection;
    float forwardTime;
    float backwardTime;
    float leftTime;
    float rightTime;

    Rigidbody rb;
    ChromaticAberration chromaticAberration;
    Animator animator;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponentInChildren<Rigidbody>();
        pp.profile.TryGet(out chromaticAberration);
        animator = GetComponentInChildren<Animator>();
    }


    void FixedUpdate()
    {
        Vector3 newVelocity = speed * Time.fixedDeltaTime * (transform.forward * movementInput.y + transform.right * movementInput.x).normalized;
        transform.Rotate(0, rotationInput * rotationSpeed * Time.fixedDeltaTime, 0);

        chromaticAberration.intensity.value = 0.0f;
        if (Time.time - dashTime < dashDuration)
        {
            chromaticAberration.intensity.value = 1.0f;
            float t = (Time.time - dashTime) / dashDuration;
            if (t < 0.3)
                camera.fieldOfView = Mathf.SmoothStep(60.0f, 80.0f, t / 0.3f);
            else if (t > 0.6)
                camera.fieldOfView = Mathf.SmoothStep(80.0f, 60.0f, (t - 0.6f) / 0.3f);
            else
                camera.fieldOfView = 80;

            newVelocity += dashDirection * dashSpeed * Time.fixedDeltaTime;
        }
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;

        animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
            return;
        movementInput = context.ReadValue<Vector2>();
        if (context.started)
        {
            if (Time.time - dashTime > dashTimeout)
            {
                if (movementInput.y > 0)
                {
                    if (Time.time - forwardTime < dashKeyTimeout)
                    {
                        dashTime = Time.time;
                        dashDirection = transform.forward;
                    }
                    forwardTime = Time.time;
                }
                else if (movementInput.y < 0)
                {
                    if (Time.time - backwardTime < dashKeyTimeout)
                    {
                        dashTime = Time.time;
                        dashDirection = -transform.forward;
                    }
                    backwardTime = Time.time;
                }
                else if (movementInput.x > 0)
                {
                    if (Time.time - rightTime < dashKeyTimeout)
                    {
                        dashTime = Time.time;
                        dashDirection = transform.right;
                    }
                    rightTime = Time.time;
                }
                else if (movementInput.x < 0)
                {
                    if (Time.time - leftTime < dashKeyTimeout)
                    {
                        dashTime = Time.time;
                        dashDirection = -transform.right;
                    }
                    leftTime = Time.time;
                }
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
            return;
        rotationInput = context.ReadValue<Vector2>().x;
    }

    void StopMuzzleShot()
    {
        if (Time.time - lastShootTime >= muzzleShotDuration)
        {
            muzzleShot.SetActive(false);
        }
    }

    public float muzzleShotDuration;


    float lastShootTime;
    public void OnFire(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
            return;
        if (context.started)
        {
            animator.SetTrigger("Fire");
            GameObject obj = Instantiate(bulletPrefab, bulletShootPos.position, transform.rotation);
            Vector3 dir = transform.forward;
            dir.y = 0;
            obj.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed);

            if (Time.time - lastShootTime >= muzzleShotDuration)
            {
                muzzleShot.SetActive(true);
                var lightFlicker = muzzleShot.GetComponentInChildren<WFX_LightFlicker>();
                var light = lightFlicker.gameObject;
                Destroy(lightFlicker);
                light.AddComponent<WFX_LightFlicker>();
            }
            lastShootTime = Time.time;
            Invoke(nameof(StopMuzzleShot), muzzleShotDuration);
            Invoke(nameof(StopMuzzleShot), muzzleShotDuration + 0.1f);
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
            return;
        print("Fire Grenade");
        if (context.started)
        {
            float x = UnityEngine.Random.Range(-180.0f, 180.0f);
            float y = UnityEngine.Random.Range(-180.0f, 180.0f);
            float z = UnityEngine.Random.Range(-180.0f, 180.0f);
            GameObject obj = Instantiate(grenadePrefab, bulletShootPos.position, Quaternion.Euler(x, y, z));
            Vector3 dir = transform.forward;
            obj.GetComponent<Rigidbody>().AddForce(dir * grenadeSpeed);
        }
    }
}
