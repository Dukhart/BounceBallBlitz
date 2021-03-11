using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IKillable
{
    Rigidbody m_Rigibody;
    GameObject m_PlayerRef;
    public SpawnManager spawnManager;
    public float speed = 1;
    bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        m_Rigibody = GetComponent<Rigidbody>();
        m_PlayerRef = GameObject.Find("Player");
    }
    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        // get movement
        Move();
        // die when falling below level
        if (transform.position.y <= -10)
        {
            Die();
        }
    }
    private void Move()
    {
        //Calculate movement vector
        Vector3 move = (m_PlayerRef.transform.position - transform.position).normalized * speed;
        // apply movement
        m_Rigibody.AddForce(move, ForceMode.Acceleration);
    }
    public void Die()
    {
        if (spawnManager != null)
        {
            spawnManager.RemoveSpawned(gameObject);
        }
        Destroy(gameObject);
    }
    void OnGameOver ()
    {
        isGameOver = true;
        m_Rigibody.velocity = Vector3.zero;
    }
}
