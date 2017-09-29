using UnityEngine;
using System;
using System.Collections.Generic;

public class LocalEventBoard : MonoBehaviour
{
    public delegate void EventTrigger(object data = null);

    [Serializable]
    public class EventList
    {
        public EventTrigger m_eventTrigger;
        public string m_eventName;
    }

    [Serializable]
    public class EventQueue
    {
        public string m_eventName;
        public object m_data;
    }

    public List<EventList> m_eventList = new List<EventList>();
    public List<EventQueue> m_eventBuffer = new List<EventQueue>();
    public List<EventQueue> m_tempBuffer = new List<EventQueue>();

    void Update()
    {
        if (m_tempBuffer != null)
        {
            foreach (EventQueue events in m_tempBuffer)
            {
                m_eventBuffer.Add(events);
            }

            m_tempBuffer.Clear();
        }
    }

    void LateUpdate()
    {
        BufferHandler();
    }

    void BufferHandler()
    {
        foreach (EventQueue iterEvent in m_eventBuffer)
        {
            TriggerEvent(iterEvent);
        }

        if (m_eventBuffer != null)
        {
            m_eventBuffer.Clear();
        }
    }

    /// <summary>Calls all functions subscribe to this event, using the EventQueue struct </summary>
    void TriggerEvent(EventQueue _queue)
    {
        EventList current = GetEventList(_queue.m_eventName);
        if (current != null)
        {
            if (_queue.m_data == null)
            {
                current.m_eventTrigger();
            }
            else
            {
                current.m_eventTrigger(_queue.m_data);
            }
        }
    }

    EventList GetEventList(string _eventName)
    {
        EventList result = null;
        m_eventList.ForEach(delegate (EventList list)
        {
            if (list.m_eventName == _eventName)
            {
                result = list;
            }
        });

        return result;
    }

    void SetEventList(string _eventName, EventTrigger _trigger, bool _add)
    {
        for (int iter = 0; iter <= m_eventList.Count - 1; iter++)
        {
            if (m_eventList[iter].m_eventName == _eventName)
            {
                if (_add)
                {
                    m_eventList[iter].m_eventTrigger += _trigger;
                }
                else
                {
                    m_eventList[iter].m_eventTrigger -= _trigger;
                }
            }
        }
    }

    public void CreateEvent(string _eventName)
    {
        EventList list = new EventList();
        list.m_eventName = _eventName;
        list.m_eventTrigger = null;
        m_eventList.Add(list);
    }

    public void AddEvent(string _event, object _data = null)
    {
        EventQueue queue = new EventQueue();
        queue.m_eventName = _event;
        queue.m_data = _data;
        m_tempBuffer.Add(queue);
    }

    /// <summary>Adds function that requires a data object to call list when this event is triggered</summary>
    public void SubscribeToEvent(string _event, EventTrigger _trigger)
    {
        SetEventList(_event, _trigger, true);
    }

    /// <summary>Removes function that requires a data object from call list of this event </summary>
    public void UnsubscribeToEvent(string _event, EventTrigger _trigger)
    {
        SetEventList(_event, _trigger, false);
    }
}