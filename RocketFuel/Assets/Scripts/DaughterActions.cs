using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;
using System;

public class DaughterActions : MonoBehaviour {

    public Vector3 jump;
    public float jumpForce = 2.0f;
    public float gravity = 10.0f;
    public bool isGrounded;
    public float speed = 200.0f;
    public float m_minDistance = 1000.0f;

    Rigidbody2D rb;

    private Player player;
    private Vector3 moveDirection = Vector3.zero;
    private int indexCurrent;
    public List<GameObject> m_allTargets = new List<GameObject>();
    public List<GameObject> m_inRangeTargets = new List<GameObject>();

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0); // get the player by id
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);
        GetTargets();
    }

    void GetTargets()
    {
        m_allTargets.Clear();
        m_allTargets = GameObject.FindGameObjectsWithTag("Dirt").ToList<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        PlayerMovement();
        TargetSelect();
        UsePowers();

    }



    private void PlayerMovement() {
        if (player.GetAxis("Move Horizontal") != 0.0f)
        {
            if (player.GetAxis("Move Horizontal") < 0.0f)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;

            } else if (player.GetAxis("Move Horizontal") > 0.0f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }

        if (player.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
                Debug.Log("Jump!");
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
    }

    private void UsePowers()
    {
        if (player.GetButtonDown("Powers"))
        {
            Debug.Log("Powers!");
        }
    }

    int LoopTargetIndex(int _index)
    {
        if (m_inRangeTargets != null)
        {
            if (m_inRangeTargets.Count != 0)
            {
                if (_index >= m_inRangeTargets.Count)
                {
                    return _index - m_inRangeTargets.Count;
                }
                else if (_index < 0)
                {
                    return _index + m_inRangeTargets.Count;
                }
            }
        }
        return _index;
    }

    void GetNext()
    {
        indexCurrent = LoopTargetIndex(++indexCurrent);
    }

    void GetPrev()
    {
        indexCurrent = LoopTargetIndex(--indexCurrent);
    }

    GameObject GetCurrent()
    {
        if (m_inRangeTargets != null)
        {
            if (m_inRangeTargets.Count != 0)
            {
                return m_inRangeTargets[indexCurrent];
            }
        }return null;
    }

    GameObject TargetSelect()
    {
        
        CollectReorderPowerObjects();
        
        if (player.GetButtonDown("Target Lock Scroll Right"))
        {
            GetNext();
            Debug.Log(indexCurrent);
        }
        if (player.GetButtonDown("Target Lock Scroll Left"))
        {
            GetPrev();
            Debug.Log(indexCurrent);
        }
        return null;
    }

    void CollectReorderPowerObjects()
    {
        m_inRangeTargets.Clear();
 
        foreach (GameObject target in m_allTargets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance <= m_minDistance)
            {
                m_inRangeTargets.Add(target);

            }
            else
            {
                continue;
            }
        }

        m_inRangeTargets = m_inRangeTargets.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();

        if (m_inRangeTargets.Count > 0)
        {
            Debug.Log(string.Format("Item 0: {0}", m_inRangeTargets[0].name));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
