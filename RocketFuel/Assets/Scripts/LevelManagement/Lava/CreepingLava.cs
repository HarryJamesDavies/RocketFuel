using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepingLava : Lava
{
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
        m_processing = false;
    }

    private void CheckSelf()
    {
        DirectionData data = m_traversableDirections.GetPriority();
        if (data != null)
        {
            LevelManager.Instance.OverrideCell(m_data.m_sectionIndex, Instantiate(LavaManager.Instance.m_creepingLavaPrefab), data.Coords);

            if (data.Direction == Direction.Directions.Left && m_traversableDirections.PeekPriorityDirection() == Direction.Directions.Right)
            {
                data = m_traversableDirections.GetPriority();
                LevelManager.Instance.OverrideCell(m_data.m_sectionIndex, Instantiate(LavaManager.Instance.m_creepingLavaPrefab), data.Coords);
            }
        }
    }
}
