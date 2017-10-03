using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Movement : MonoBehaviour
{
    public float m_moveSpeed = 1.0f;
    public float m_jumpSpeed = 10.0f;

    private Rigidbody2D m_rb;
    private Player m_player;
    private Vector2 m_force;

    private int m_remainingJumps = 1;

	void Start ()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = ReInput.players.GetPlayer(0);
    }
	
	void Update ()
    {
        m_force = Vector2.zero;
        m_force += m_player.GetAxis("Move Horizontal") * m_moveSpeed * Vector2.right;
        
        if((m_player.GetButtonDown("Jump")) && (m_remainingJumps != 0))
        {
            m_remainingJumps--;
            m_force += m_jumpSpeed * Vector2.up;
        }
    }

    void FixedUpdate()
    {
        m_rb.AddForce(m_force, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            m_remainingJumps = 1;
        }
    }
}
