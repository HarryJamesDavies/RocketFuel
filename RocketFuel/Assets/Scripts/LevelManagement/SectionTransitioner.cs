using UnityEngine;

public class SectionTransitioner : MonoBehaviour
{
    public int m_sectionIndex = 0;

    private void Awake()
    {
        m_sectionIndex = LevelManager.Instance.GetGenerateSectionCount();
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        Debug.Log("Triggered");
        if (_other.tag == "Player")
        {
            SectionTransitionData data = new SectionTransitionData(m_sectionIndex);
            GlobalEventBoard.Instance.AddEvent(Events.Event.LEV_TransitionSection, data);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
