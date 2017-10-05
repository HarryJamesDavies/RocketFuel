using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticLava : Lava
{
    public List<RealisticLava> m_children = new List<RealisticLava>();
    public int m_spreadCharge = 20;

    void Update()
    {
        base.Update();

        if (!m_spreadComplete && !m_processing)
        {
            m_processing = true;
            StartCoroutine(SpawnHandler());
        }
    }

    private IEnumerator SpawnHandler()
    {
        yield return new WaitForSeconds(LavaManager.Instance.m_wait);

        CheckSelf();
    }

    private void CheckSelf()
    {
        DirectionData data = m_traversableDirections.GetPriority();
        if (data != null)
        {
            DecreaseCharge(data.Direction);

            if (m_spreadCharge > 0)
            {
                m_children.Add(Instantiate(LavaManager.Instance.m_realisticLavaPrefab).GetComponent<RealisticLava>());
                m_children[m_children.Count - 1].m_spreadCharge = m_spreadCharge;
                LevelManager.Instance.OverrideCell(m_data.m_sectionIndex, m_children[m_children.Count - 1].gameObject, data.Coords);

                if (data.Direction == Direction.Directions.Left && m_traversableDirections.PeekPriorityDirection() == Direction.Directions.Right)
                {
                    data = m_traversableDirections.GetPriority();
                    m_children.Add(Instantiate(LavaManager.Instance.m_realisticLavaPrefab).GetComponent<RealisticLava>());
                    m_children[m_children.Count - 1].m_spreadCharge = m_spreadCharge;
                    LevelManager.Instance.OverrideCell(m_data.m_sectionIndex, m_children[m_children.Count - 1].gameObject, data.Coords);
                }
            }
        }
    }

    private void DecreaseCharge(Direction.Directions _dir)
    {
        switch (_dir)
        {
            case Direction.Directions.Left:
                {
                    m_spreadCharge--;
                    break;
                }
            case Direction.Directions.Right:
                {
                    m_spreadCharge--;
                    break;
                }
            case Direction.Directions.Up:
                {
                    m_spreadCharge -= 2;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
