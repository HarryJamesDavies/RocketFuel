using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingLava : Liquid
{
    public Liquid m_parent = null;
    public List<FlowingLava> m_children = new List<FlowingLava>();

    public bool CheckDirection(Direction.Directions _direction)
    {
        bool result = CheckChildren(_direction);

        if (!result && _direction != Direction.Directions.Up)
        {
            result = CheckSelf(_direction); ;
        }
        else
        {
            bool compare = CheckSelf(_direction);

            if(compare)
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
        foreach (FlowingLava lava in m_children)
        {
            bool compare = lava.CheckDirection(_direction);

            if (compare)
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
