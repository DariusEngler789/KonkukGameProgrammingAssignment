using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    Vector2 movementInput;
    float rotationInput;
    float dashTime;
    Vector3 dashDirection;
    float forwardTime;
    float backwardTime;
    float leftTime;
    float rightTime;

    Rigidbody rb;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponentInChildren<Rigidbody>();
    }


    void FixedUpdate()
    {
        Vector3 newVelocity = speed * Time.fixedDeltaTime * (transform.forward * movementInput.y + transform.right * movementInput.x).normalized;
        transform.Rotate(0, rotationInput * rotationSpeed * Time.fixedDeltaTime, 0);

        if (Time.time - dashTime < dashDuration)
        {
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
    }

    public void OnMove(InputAction.CallbackContext context)
    {
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
        rotationInput = context.ReadValue<Vector2>().x;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject obj = Instantiate(bulletPrefab, bulletShootPos.position, transform.rotation);
            Vector3 dir = transform.forward;
            dir.y = 0;
            obj.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed);
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
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
