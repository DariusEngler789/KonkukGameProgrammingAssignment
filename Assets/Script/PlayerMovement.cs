using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashDuration;
    public float dashKeyTimeout;
    public float dashTimeout;
    public float rotationSpeed;
    public GameObject bulletPrefab;
    public Transform bulletShootPos;

    float dashTime;
    Vector3 dashDirection;
    float forwardTime;
    float backwardTime;
    float leftTime;
    float rightTime;

    void Awake()
    {
        // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - dashTime < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
        }

        float d = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, d);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, -d);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-d, 0, 0);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(d, 0, 0);

        if (Time.time - dashTime > dashTimeout)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (Time.time - forwardTime < dashKeyTimeout)
                {
                    dashTime = Time.time;
                    dashDirection = Vector3.forward;
                }
                forwardTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (Time.time - backwardTime < dashKeyTimeout)
                {
                    dashTime = Time.time;
                    dashDirection = Vector3.back;
                }
                backwardTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - leftTime < dashKeyTimeout)
                {
                    dashTime = Time.time;
                    dashDirection = Vector3.left;
                }
                leftTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - rightTime < dashKeyTimeout)
                {
                    dashTime = Time.time;
                    dashDirection = Vector3.right;
                }
                rightTime = Time.time;
            }
        }



        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);

        if (Input.GetMouseButtonDown(0))
            Instantiate(bulletPrefab, bulletShootPos.position, transform.rotation);
    }
}
