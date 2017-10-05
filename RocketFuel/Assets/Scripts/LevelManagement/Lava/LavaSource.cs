using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSource : Liquid
{
    public float m_wait = 1.0f;
    public bool m_spreadComplete = true;

    private bool m_firstFrame = true;
    private bool m_processing = false;

    public List<FlowingLava> m_children = new List<FlowingLava>();

    void Start()
    {
        if(m_data.m_sectionIndex == 0)
        {
            m_spreadComplete = false;
        }

        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_CheckSpread);
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_CheckSpread);
    }

    public void Ev_CheckSpread(object _data = null)
    {
        if (_data != null)
        {
            SectionTransitionData data = (SectionTransitionData)_data;

            if (m_data.m_sectionIndex == data.SectionIndex)
            {
                m_spreadComplete = false;
            }
        }
    }

    void Update()
    {
        if (!m_spreadComplete)
        {
            if (m_firstFrame)
            {
                m_firstFrame = false;
                m_traversableDirections.CheckUnchecked(m_data.m_sectionIndex, m_data.m_coords);
            }
            else if (!m_processing)
            {
                m_processing = true;
                StartCoroutine(SpawnHandler());
            }
        }
    }

    private IEnumerator SpawnHandler()
    {
        yield return new WaitForSeconds(m_wait);

        bool spawned = false;
        spawned = CheckDirection(Direction.Directions.Down);

        if (!spawned)
        {
            spawned = CheckDirection(Direction.Directions.Left);
            bool compare = CheckDirection(Direction.Directions.Right);

            if (compare)
            {
                spawned = compare;
            }
        }

        if(!spawned)
        {
            spawned = CheckDirection(Direction.Directions.Up);
        }

        if (!spawned)
        {
            m_spreadComplete = true;
        }

        m_processing = false;
    }

    private bool CheckDirection(Direction.Directions _direction)
    {
        bool result = CheckChildren(_direction);

        if (!result && _direction != Direction.Directions.Up)
        {
            result = CheckSelf(_direction); ;
        }
        else
        {
            bool compare = CheckSelf(_direction);

            if (compare)
            {
                result = true;
            }
        }

        return result;
    }

    private bool CheckSelf(Direction.Directions _direction)
    {
        if (m_traversableDirections.CheckDirection(_direction, m_data.m_coords) == CellState.Empty)
        {
            m_children.Add(Instantiate(LavaManager.Instance.m_flowingLavaPrefab).GetComponent<FlowingLava>());
            m_children[m_children.Count - 1].m_parent = this;
            LevelManager.Instance.OverrideCell(m_data.m_sectionIndex, m_children[m_children.Count - 1].gameObject, m_data.m_coords.GetDirection(_direction));
            return true;
        }

        return false;
    }

    private bool CheckChildren(Direction.Directions _direction)
    {
        bool result = false;
        foreach(FlowingLava lava in m_children)
        {
            bool compare = lava.CheckDirection(_direction);

            if(compare)
            {
                result = true;
            }
        }
        return result;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            GlobalEventBoard.Instance.AddEvent(Events.Event.GLO_PlayerDied);
        }
    }
}
