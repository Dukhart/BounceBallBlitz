using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody m_Rigibody;
    GameObject m_PlayerRef;

    public float speed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        m_Rigibody = GetComponent<Rigidbody>();
        m_PlayerRef = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        //Calculate movement vector
        Vector3 move = (m_PlayerRef.transform.position - transform.position).normalized * speed;
        // apply movement
        m_Rigibody.AddForce(move, ForceMode.Acceleration);
    }
}
