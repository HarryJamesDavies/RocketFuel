using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;
using System;

public class DaughterActions : MonoBehaviour
{

    public Vector3 jump;
    public float jumpForce = 2.0f;
    public float moveForce = 0.3f;
    public float gravity = 10.0f;
    public bool isGrounded;
    public float speed = 200.0f;
    public float m_minDistance = 100.0f;
    public float targetNextSelectPeriod = 0.5f;

    Rigidbody2D rb;

    public List<GameObject> m_allTargets = new List<GameObject>();
    public List<GameObject> m_inRangeTargets = new List<GameObject>();
    public List<GameObject> m_childPulledTargets = new List<GameObject>();

    private Player player;
    private Vector3 moveDirection = Vector3.zero;
    private int indexCurrent;
    private float targetNextSelectTime = 0.0f;
    private GameObject targetSelectAudio;


    private Color selectColor = Color.cyan;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0); // get the player by id
    }
    // Use this for initialization
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
       // jump = new Vector3(0.0f, 1.0f, 0.0f);
        GetTargets();
        targetSelectAudio = GameObject.FindGameObjectWithTag("TargetSelectAudio");
    }
    void GetTargets()
    {
        m_allTargets.Clear();
        m_allTargets = GameObject.FindGameObjectsWithTag("Dirt").ToList<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        CollectReorderPowerObjects();
        UsePowers();
        if (Time.time > targetNextSelectTime)
        {
            targetNextSelectTime += targetNextSelectPeriod;
            //targetSelectAudio.GetComponent<AudioSource>().Play();
            TargetSelect();
        }

    }

    private void UsePowers()
    {
        if (GetCurrent() && GetCurrent().GetComponent<PullableBlock>())
        {
            if (player.GetButtonDown("Powers"))
            {
                GetCurrent().GetComponent<PullableBlock>().DelayedSpawn();
                ClearColorOfSelected(GetCurrent());
            }
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

    void CycleNext()
    {
        indexCurrent = LoopTargetIndex(++indexCurrent);
        ChangeSelectedColor();
    }

    GameObject GetCurrent()
    {
        if (m_inRangeTargets != null)
        {
            if (m_inRangeTargets.Count != 0)
            {
                return m_inRangeTargets[indexCurrent];
            }
        }
        return null;
    }

    GameObject TargetSelect()
    {
        ClearColorOfSelected();
        CycleNext();
        GetCurrent();

        Debug.Log(string.Format("closest, indexCurrent and name of current index : {0} + {1} + {2}", m_inRangeTargets[0].name, indexCurrent, GetCurrent().name));
        return null;
    }

    void CollectReorderPowerObjects()
    {
        m_inRangeTargets.Clear();

        foreach (GameObject target in m_allTargets)
        {
            if (!target.GetComponent<PullableBlock>().HasClone())
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);

                if (distance <= m_minDistance)
                {
                    m_inRangeTargets.Add(target);
                }
            }
            else
            {
                continue;
            }

        }

        m_inRangeTargets = m_inRangeTargets.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();
    }

    void ClearColorOfSelected(GameObject _object)
    {
        _object.GetComponent<SpriteRenderer>().color = Color.white;
        if (_object.transform.childCount > 0)
        {
            GetCurrent().gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    void ClearColorOfSelected()
    {
        GetCurrent().gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        if (GetCurrent().transform.childCount > 0)
        {
            GetCurrent().gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void ChangeSelectedColor()
    {
        GetCurrent().gameObject.GetComponent<SpriteRenderer>().color = selectColor;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
