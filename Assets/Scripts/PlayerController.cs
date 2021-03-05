using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 60;
    public float cameraSpeed = 100;
    public float pulseForce = 6;
    public float jumpForce = 5;
    public float pulseCoolDown = 5;
    public float gravityModifier = 9;

    Rigidbody m_Rigidbody;
    GameObject m_FocalPoint;
    Camera m_Camera;
    bool onGround = false;
    float pulseTimer = 0;
    Vector2 inputAxis = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // get components & GameObject Refs
        m_Rigidbody = GetComponent<Rigidbody>();
        m_FocalPoint = GameObject.Find("FocalPoint");
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        GetInput();
        // Get Pulse Power key
        if (Input.GetKey(KeyCode.Q))
        {
            Pulse();
        }
        // get jump key
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        // set pulse timer
        pulseTimer = pulseTimer - Time.deltaTime < 0 ? 0 : pulseTimer - Time.deltaTime;
        // apply gravity mod
        m_Rigidbody.AddForce(Vector3.down * gravityModifier, ForceMode.Acceleration);
    }
    // get collision events
    private void OnCollisionEnter(Collision collision)
    {
        // detect when the player collides with ground objects
        if (collision.gameObject.CompareTag("Ground"))
        {
            // set on ground to true
            onGround = true;
        }
    }
    void GetInput()
    {
        // Get Input Axis
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");

        // rotate the camera
        m_FocalPoint.transform.Rotate(Vector3.up, inputAxis.x * cameraSpeed * Time.deltaTime);
        // apply movement
        m_Rigidbody.AddForce(m_FocalPoint.transform.forward * speed * inputAxis.y * Time.deltaTime);
    }
    // shoot forward quickly
    void Pulse ()
    {
        // check the cooldown on this ability is not active
        if (pulseTimer <= 0)
        {
            Debug.Log("Pulse");
            //apply pulse force
            m_Rigidbody.AddForce(m_FocalPoint.transform.forward * pulseForce, ForceMode.Impulse);

            pulseTimer = pulseCoolDown;
        }
    }
    // make the ball jump
    void Jump()
    {
        // only allow jumping from ground
        if (onGround)
        {
            Debug.Log("Jump");
            //apply pulse force
            //m_Rigidbody.AddForce(Vector3.up * jumpForce + m_FocalPoint.transform.forward * jumpForce * 0.5f, ForceMode.Impulse);
            m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }
    Vector3 PlayerInputAxisToCameraSpace()
    {
        //trasnform input into camera space
        var forward = m_Camera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        var right = Vector3.Cross(Vector3.up, forward);
        // calc movement vector
        Vector3 move = forward * inputAxis.y + right * inputAxis.x;
        move.y = 0; // zero out y
        return move;
    }
}
