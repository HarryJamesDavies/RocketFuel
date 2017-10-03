using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnchor : MonoBehaviour
{
    //public float m_speed = 1.0f;

    private bool m_firstUpdate = true;
    //private Vector2 m_force = Vector2.zero;
    //private Rigidbody2D m_rb;

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_MovePlayerOnTransition);

        //m_rb = GetComponent<Rigidbody2D>();
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_MovePlayerOnTransition);
    }

    public void Ev_MovePlayerOnTransition(object _data = null)
    {
        if (_data != null)
        {
            SectionTransitionData data = _data as SectionTransitionData;
            transform.position = LevelManager.Instance.GetSectionSpawn(data.SectionIndex);
        }
    }

    void Update()
    {
        if (!m_firstUpdate)
        {
            m_firstUpdate = true;
            transform.position = LevelManager.Instance.GetSectionSpawn(0);
        }
    }
}

//        m_force = Vector2.zero;
//		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
//        {
//            m_force += Vector2.up * m_speed;
//        }

//        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
//        {
//            m_force += Vector2.down * m_speed;
//        }

//        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
//        {
//            m_force += Vector2.right * m_speed;
//        }

//        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
//        {
//            m_force += Vector2.left * m_speed;
//        }
//    }

//    void FixedUpdate()
//    {
//        m_rb.AddForce(m_force, ForceMode2D.Impulse);
//    }
//}
