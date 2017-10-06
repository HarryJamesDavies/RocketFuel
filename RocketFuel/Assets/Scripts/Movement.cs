using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Movement : MonoBehaviour
{
    public float m_moveSpeed = 1.0f;
    public float m_jumpSpeed = 10.0f;
    private float m_jumpOrigSpeed;

    private Rigidbody2D m_rb;
    private Player m_player;
    private Vector2 m_force;

    public int m_maxJumpCount;
    private int m_remainingJumps;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = ReInput.players.GetPlayer(0);
        ResetRemainingJumps();
        m_jumpOrigSpeed = m_jumpSpeed;
    }

    private void Update()
    {
        m_force = Vector2.zero;
        m_force += m_player.GetAxis("Move Horizontal") * m_moveSpeed * Vector2.right;

        if ((m_player.GetButtonDown("Jump")) && (m_remainingJumps != 0))
        {
            m_remainingJumps--;
            m_force += m_jumpSpeed * Vector2.up;
        }
    }

    private void FixedUpdate()
    {
        m_rb.AddForce(m_force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            ResetRemainingJumps();
        }
    }

    private void ResetRemainingJumps()
    {
        m_remainingJumps = m_maxJumpCount;
    }

    public void SetRemainingJumps(int _count)
    {
        m_remainingJumps = _count;
    }

    public void ResetJumpSpeed()
    {
        m_jumpSpeed = m_jumpOrigSpeed;
    }

    public void MultiplyJumpSpeed(int _multiplier)
    {
        m_jumpSpeed *= _multiplier;
    }
}