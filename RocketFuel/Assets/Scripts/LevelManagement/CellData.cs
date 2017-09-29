using UnityEngine;

public class CellData : MonoBehaviour
{
    public enum CellContent
    {
        Solid,
        Liquid,
        Air
    }

    public int m_sectionIndex = -1;
    public GridCoordinates m_coords = null;
    public CellContent m_content = CellContent.Air;
	
    public void Initialise(int _sectionIndex, GridCoordinates _coords)
    {
        m_sectionIndex = _sectionIndex;
        m_coords = _coords;
    }

    public void Initialise(int _sectionIndex, GridCoordinates _coords, CellContent _content)
    {
        m_sectionIndex = _sectionIndex;
        m_coords = _coords;
        m_content = _content;
    }
}
