using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

public class TerrainManipulator : MonoBehaviour
{
    public class TargetData
    {
        public TargetData(GameObject _target, int _index)
        {
            Target = _target;
            Index = _index;
        }

        public GameObject Target;
        public int Index;
    }

    public float m_minimumDistance = 100.0f;
    public Color m_highlightColour = Color.green;

    private int m_currentIndex = 0;
    private GameObject m_prevTarget = null;
    private List<int> m_reorderedIndicies = new List<int>();
    private List<GameObject> m_allTargets = new List<GameObject>();

    private Player m_player;

    void Start ()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, GetAllTargets);

        m_player = ReInput.players.GetPlayer(0);

        GetAllTargets();
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, GetAllTargets);
    }

    void GetAllTargets(object _data = null)
    {
        m_allTargets.Clear();
        m_allTargets = GameObject.FindGameObjectsWithTag("Manip").ToList();
    }


    void Update ()
    {
        GetInRangeTargets();

        if (m_reorderedIndicies.Count != 0)
        {
            ScrollTargets();
            //HighlightAllInRange();
            HighlightCurrent();
            ManipulateCurrentTarget();
        }
        else
        {
            m_currentIndex = 0;
        }
    }

    void GetInRangeTargets()
    {
        List<TargetData> inRangeTargets = new List<TargetData>();

        for (int iter = 0; iter <= m_allTargets.Count - 1; iter++)
        {
            float distance = Vector2.Distance(transform.position, m_allTargets[iter].transform.position);

            if (distance <= m_minimumDistance)
            {
                inRangeTargets.Add(new TargetData(m_allTargets[iter], iter));
            }
            else
            {
                continue;
            }
        }

        inRangeTargets = inRangeTargets.OrderBy(x => Vector2.Distance(transform.position, x.Target.transform.position)).ToList();

        m_reorderedIndicies.Clear();
        foreach(TargetData data in inRangeTargets)
        {
            m_reorderedIndicies.Add(data.Index);
        }
    }

    void ScrollTargets()
    {
        if (m_player.GetButtonDown("Target Lock Scroll Right"))
        {
            GetNext();
        }
        else if (m_player.GetButtonDown("Target Lock Scroll Left"))
        {
            GetPrev();
        }
    }

    int LoopTargetIndex(int _index)
    {
        if (m_reorderedIndicies != null)
        {
            if (m_reorderedIndicies.Count != 0)
            {
                if (_index >= m_reorderedIndicies.Count)
                {
                    return _index - m_reorderedIndicies.Count;
                }
                else if (_index < 0)
                {
                    return _index + m_reorderedIndicies.Count;
                }
            }
        }
        return _index;
    }

    void GetNext()
    {
        m_currentIndex = LoopTargetIndex(++m_currentIndex);
    }

    void GetPrev()
    {
        m_currentIndex = LoopTargetIndex(--m_currentIndex);
    }

    void HighlightCurrent()
    {
        if(m_prevTarget)
        {
            if(m_prevTarget != m_allTargets[m_reorderedIndicies[m_currentIndex]])
            {
                m_prevTarget.GetComponent<SpriteRenderer>().color = Color.white;
                m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<SpriteRenderer>().color = m_highlightColour;
                m_prevTarget = m_allTargets[m_reorderedIndicies[m_currentIndex]];
            }
        }
        else
        {
            m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<SpriteRenderer>().color = m_highlightColour;
            m_prevTarget = m_allTargets[m_reorderedIndicies[m_currentIndex]];
        }
    }

    void HighlightAllInRange()
    {
        foreach (GameObject target in m_allTargets)
        {
            target.GetComponent<SpriteRenderer>().color = Color.white;
        }

        foreach (int index in m_reorderedIndicies)
        {
            m_allTargets[index].GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    void ManipulateCurrentTarget()
    {
        if (m_player.GetButtonDown("Powers"))
        {
            m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<SpriteRenderer>().color = Color.white;
            m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<PullableBlock>().SpawnClone();
            m_allTargets.Remove(m_allTargets[m_reorderedIndicies[m_currentIndex]]);
        }
    }
}
