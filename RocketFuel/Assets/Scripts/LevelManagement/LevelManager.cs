using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;

    public int m_sections = 3;
    public int m_chunks = 10;
    public float m_cellWidth = 20;
    public float m_cellHeight = 100;
    public Vector3 m_origin;

    private int m_currentSection = 0;
    private Level m_level;
    public float m_wait = 3.0f;

    private List<Vector3> m_sectionSpawns = new List<Vector3>();

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TransitionSection);

        InitialiseLevel(); 
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TransitionSection);
    }

    public bool InitialiseLevel()
    {
        m_level = GetComponent<Level>();
        m_level.InitaliseLevel(m_chunks, m_cellWidth, m_cellHeight, m_origin);
        return true;
    }

    public void Ev_TransitionSection(object _data = null)
    {
        if (!CheckSectionCapHit())
        {
            StartCoroutine(Delay());
        }
        else
        {
            m_level.SpawnFinishLine();
        }
        m_currentSection++;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(m_wait);
        m_level.TransitionSection();
    }

    public int GetCurrentSectionIndex()
    {
        return m_currentSection;
    }

    public int GetGenerateSectionCount()
    {
        return m_level.m_sectionsGenerated;
    }

    public void AddSectionSpawn(Vector3 _spawnPosition)
    {
        m_sectionSpawns.Add(_spawnPosition);
    }

    public Vector3 GetSectionSpawn(int _sectionIndex)
    {
        return m_sectionSpawns[_sectionIndex];
    }

    public bool CheckSectionCapHit()
    {
        return m_level.m_sectionsGenerated == m_sections;
    }

    public CellData.CellContent CheckCellContent(GridCoordinates _coords)
    {
        return m_level.CheckCellContent(_coords);
    }

    public CellData.CellContent CheckCellContent(int _sectionIndex, GridCoordinates _coords)
    {
        return m_level.CheckCellContent(_sectionIndex, _coords);
    }

    public void OverrideCell(int _sectionIndex, GameObject _cell, GridCoordinates _coords)
    {
        m_level.OverrideCell(_sectionIndex, _cell, _coords);
    }
}