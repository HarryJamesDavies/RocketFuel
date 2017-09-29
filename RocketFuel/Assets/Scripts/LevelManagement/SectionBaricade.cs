using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBaricade : MonoBehaviour
{
    public int m_sectionIndex = 0;
    public float m_delayOffset = 2.0f;

    private PullableBlock m_pb = null;

	void Start ()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TriggerBaricade);

        m_pb = GetComponent<PullableBlock>();
        m_sectionIndex = LevelManager.Instance.GetGenerateSectionCount();
	}

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TriggerBaricade);
    }

    public void Ev_TriggerBaricade(object _data = null)
    {
        if (_data != null && m_pb)
        {
            SectionTransitionData data = _data as SectionTransitionData;
            if (data.SectionIndex == LevelManager.Instance.GetCurrentSectionIndex())
            {
                StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(Mathf.Clamp(LevelManager.Instance.m_wait - m_delayOffset, 0.0f, LevelManager.Instance.m_wait));
        MoveBaricade();
    }

    private void MoveBaricade()
    {
        m_pb.SpawnClone();
    }
}
