using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LoucaMovement : MonoBehaviour
{
    private Rigidbody2D m_rb2D;

    private Player m_player;

    public float m_moveSpeed;
    public float m_jumpHeight;
    public float m_fallMultiplier;
    public float m_rayDist;
    public float m_rayDistHor;
    private float m_horMovement;

    private Vector2 m_velocity;

    private bool m_jumping;

    private LayerMask m_layerMask;

    public int m_maxJumps;
    private int m_jumpCount;

    // Use this for initialization
    private void Start()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_player = ReInput.players.GetPlayer(0);
        m_jumping = false;
        m_layerMask = 1;
        m_jumpCount = m_maxJumps;
    }

    // Update is called once per frame
    private void Update()
    {
        m_velocity = Vector2.zero;
        m_horMovement = m_player.GetAxis("Move Horizontal") * m_moveSpeed * Time.deltaTime;
        m_velocity = new Vector2(m_horMovement, 0f);

        if (m_player.GetButtonDown("Jump"))
        {
            RaycastHit2D t_hit = Physics2D.Raycast(transform.position, Vector2.down, m_rayDist, m_layerMask);
            RaycastHit2D t_hitLeft = Physics2D.Raycast(transform.position, Vector2.left, m_rayDistHor, m_layerMask);
            RaycastHit2D t_hitRight = Physics2D.Raycast(transform.position, Vector2.right, m_rayDistHor, m_layerMask);
            if (t_hit)
            {
                if (t_hit.collider.tag == "Ground" || t_hit.collider.tag == "Manip")
                {
                    m_jumping = true;
                    m_jumpCount--;
                }
            }
            if (t_hitLeft)
            {
                if (t_hitLeft.collider.tag == "Ground" || t_hitLeft.collider.tag == "Manip")
                {
                    m_jumping = true;
                }
            }
            if (t_hitRight)
            {
                if (t_hitRight.collider.tag == "Ground" || t_hitRight.collider.tag == "Manip")
                {
                    m_jumping = true;
                }
            }
            if (!t_hit && m_jumpCount > 0)
            {
                m_jumping = true;
                m_jumpCount--;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_jumping)
        {
            m_jumping = false;
            m_velocity.y = m_jumpHeight;
        }
        m_rb2D.velocity += m_velocity;
        if (m_rb2D.velocity.y == 0f)
        {
            if (m_jumpCount != m_maxJumps)
            {
                RaycastHit2D t_hit = Physics2D.Raycast(transform.position, Vector2.down, m_rayDist, m_layerMask);
                if (t_hit)
                {
                    if (t_hit.collider.tag == "Ground" || t_hit.collider.tag == "Manip")
                    {
                        m_jumpCount = m_maxJumps;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Power" && other.GetComponent<PowerUp>())
        {
            PowerUp t_powerUpScript = other.GetComponent<PowerUp>();
            switch (t_powerUpScript.GetPowerType())
            {
                case PowerUp.PowerType.TripleJump:
                    m_jumpCount = m_maxJumps++;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}