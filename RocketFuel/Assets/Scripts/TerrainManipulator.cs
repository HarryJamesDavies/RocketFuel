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

    public float m_targetTime = 0.8f;
    private float m_targetTimer;

    private int m_currentIndex = 0;
    private GameObject m_prevTarget = null;
    private List<int> m_reorderedIndicies = new List<int>();
    private List<GameObject> m_allTargets = new List<GameObject>();

    private GameObject targetSelectAudio;
    private Player m_player;

    private void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, GetAllTargets);
        targetSelectAudio = GameObject.FindGameObjectWithTag("TargetSelectAudio");
        m_player = ReInput.players.GetPlayer(0);

        m_targetTimer = 0.0f;

        GetAllTargets();
    }

    private void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, GetAllTargets);
    }

    private void GetAllTargets(object _data = null)
    {
        m_allTargets.Clear();
        m_allTargets = GameObject.FindGameObjectsWithTag("Manip").ToList();
    }

    private void Update()
    {
        

        GetInRangeTargets();

        m_targetTimer += Time.deltaTime;
        if (m_targetTimer > m_targetTime)
        {

            if (m_reorderedIndicies.Count != 0)
            {
                //TargetSelect() replaced with new method
                CycleNext();
                //Highlight all in range
                HighlightCurrent();
                ManipulateCurrentTarget();
            }
            else
            {
                m_currentIndex = 0;
            }
            m_targetTimer = 0.0f;
        }
    }

    private void GetInRangeTargets()
    {
        List<TargetData> inRangeTargets = new List<TargetData>();

        foreach (GameObject target in m_allTargets)
        {
            for (int iter = 0; iter <= m_allTargets.Count - 1; iter++)
            {
                if (!target.GetComponent<PullableBlock>().HasClone())
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
            }
        }
        inRangeTargets = inRangeTargets.OrderBy(x => Vector2.Distance(transform.position, x.Target.transform.position)).ToList();

        m_reorderedIndicies.Clear();
        foreach (TargetData data in inRangeTargets)
        {
            m_reorderedIndicies.Add(data.Index);
        }
    }

    void CycleNext()
    {
        m_currentIndex = LoopTargetIndex(++m_currentIndex);
        targetSelectAudio.GetComponent<AudioSource>().Play();
    }
   

    private int LoopTargetIndex(int _index)
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



    private void HighlightCurrent()
    {
        if (m_prevTarget)
        {
            if (m_prevTarget != m_allTargets[m_reorderedIndicies[m_currentIndex]])
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

    private void HighlightAllInRange()
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

    private void ManipulateCurrentTarget()
    {
        if (m_player.GetButtonDown("Powers"))
        {
            m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<SpriteRenderer>().color = Color.white;
            m_allTargets[m_reorderedIndicies[m_currentIndex]].GetComponent<PullableBlock>().SpawnClone();
            m_allTargets.Remove(m_allTargets[m_reorderedIndicies[m_currentIndex]]);
        }
    }
}
//private void ScrollTargets()
//{
//    if (m_player.GetButtonDown("Target Lock Scroll Right"))
//    {
//        GetNext();
//    }
//    else if (m_player.GetButtonDown("Target Lock Scroll Left"))
//    {
//        GetPrev();
//    }
//}
//private void GetNext()
//{
//    m_currentIndex = LoopTargetIndex(++m_currentIndex);
//}

//private void GetPrev()
//{
//    m_currentIndex = LoopTargetIndex(--m_currentIndex);
//}