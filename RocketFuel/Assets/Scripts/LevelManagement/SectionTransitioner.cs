using UnityEngine;

public class SectionTransitioner : MonoBehaviour
{
    public int m_sectionIndex = 0;

    void Start()
    {
        m_sectionIndex = GetComponent<CellData>().m_sectionIndex;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            SectionTransitionData data = new SectionTransitionData(m_sectionIndex);
            GlobalEventBoard.Instance.AddEvent(Events.Event.LEV_TransitionSection, data);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
