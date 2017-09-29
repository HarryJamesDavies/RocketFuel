using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnchor : MonoBehaviour
{
    public float m_speed = 0.5f;

    private bool m_firstUpdate = true;

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_MovePlayerOnTransition);
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

	void Update ()
    {
        if(!m_firstUpdate)
        {
            m_firstUpdate = true;
            transform.position = LevelManager.Instance.GetSectionSpawn(0);
        }

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + m_speed, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - m_speed, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + m_speed, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - m_speed, transform.position.y, transform.position.z);
        }
    }
}
