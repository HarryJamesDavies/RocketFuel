using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSource : Liquid
{
    protected bool m_spreadComplete = false;
    protected bool m_processing = false;

    private bool m_firstFrame = true;

    protected void Awake()
    {
        base.Awake();

        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_ToggleActive);
    }

    protected void Update()
    {
        if (m_firstFrame)
        {
            m_firstFrame = false;
            m_traversableDirections.CheckUnchecked(m_data.m_sectionIndex, m_data.m_coords);

            if(m_data.m_sectionIndex != LevelManager.Instance.GetCurrentSectionIndex())
            {
                m_spreadComplete = true;
            }
        }
    }

    protected void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_ToggleActive);
    }

    public void Ev_ToggleActive(object _data = null)
    {
        if (_data != null)
        {
            SectionTransitionData data = (SectionTransitionData)_data;

            if (m_data.m_sectionIndex == data.SectionIndex)
            {
                m_spreadComplete = false;
            }
            else
            {
                m_spreadComplete = true;
            }
        }
    }
}
