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
    //public float powerUpDuration = 5;
    public float gravityModifier = 9;

    Rigidbody m_Rigidbody;
    GameObject m_FocalPoint;
    public GameObject powerUpIndicator;
    public ParticleSystem powerHitParticleSystem;
    Camera m_Camera;
    bool onGround = false;
    bool canPulse = true;
    bool hasPowerUp = false;
    float powerUpStrength;
    Vector2 inputAxis = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // get components & GameObject Refs
        m_Rigidbody = GetComponent<Rigidbody>();
        m_FocalPoint = GameObject.Find("FocalPoint");
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (powerUpIndicator != null) {
            powerUpIndicator.SetActive(false);
        }
        
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
        // apply gravity mod
        m_Rigidbody.AddForce(Vector3.down * gravityModifier, ForceMode.Acceleration);
        UpdatePowerUpIndicator();
    }
    // get collision events
    void OnCollisionEnter(Collision collision)
    {
        // detect when the player collides with ground objects
        if (collision.gameObject.CompareTag("Ground"))
        {
            // set on ground to true
            onGround = true;
        } 
        // if we collide with an enemy while we have a power up
        else if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Debug.Log("PowerHit");
            // get enemy rigidbody
            Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
            powerHitParticleSystem.transform.position = collision.GetContact(0).point;
            powerHitParticleSystem.Play();
            // calculate force vector
            Vector3 force = (collision.gameObject.transform.position - transform.position).normalized;
            //apply force
            otherRB.AddForce(force* powerUpStrength, ForceMode.Impulse);
        }
    }
    // get trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        // check if we hit a power up
        if (other.gameObject.CompareTag("PowerUp"))
        {
            // get the power up
            PowerUp pu = other.gameObject.GetComponent<PowerUp>();
            // apply power up
            hasPowerUp = true;
            powerUpStrength = pu.strength;
            if (powerUpIndicator != null) powerUpIndicator.SetActive(true);
            StartCoroutine(ResetPowerUp_AfterTime(pu.duration));
            // destroy power up
            Destroy(other.gameObject);
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
        if (canPulse)
        {
            Debug.Log("Pulse");
            //apply pulse force
            m_Rigidbody.AddForce(m_FocalPoint.transform.forward * pulseForce, ForceMode.Impulse);
            // set pulse timer
            canPulse = false;
            StartCoroutine(SetPulse_AfterTime(true, pulseCoolDown));
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
    void UpdatePowerUpIndicator()
    {
        // rotates the power up at speed variable to power up strength
        Vector3 currentRotation = powerUpIndicator.transform.rotation.eulerAngles;
        powerUpIndicator.transform.rotation = Quaternion.Euler(0.0f, currentRotation.y, gameObject.transform.rotation.z * -1.0f);

        // rotate the power up at power up strength rate
        powerUpIndicator.transform.Rotate(Vector3.up * Time.deltaTime * powerUpStrength * 50);
    }
    // pulse cooldown timer
    IEnumerator SetPulse_AfterTime (bool value, float time)
    {
        // wait for cooldown
        yield return new WaitForSeconds(time);
        // reset can pulse
        canPulse = value;
    }
    // powerUp cooldown timer
    IEnumerator ResetPowerUp_AfterTime(float time)
    {
        // wait for cooldown
        yield return new WaitForSeconds(time);
        // reset can pulse
        hasPowerUp = false;
        powerUpStrength = 0;
        if (powerUpIndicator != null) powerUpIndicator.SetActive(false);
    }
}
