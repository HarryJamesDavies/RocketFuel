using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaManager : MonoBehaviour
{
    public static LavaManager Instance = null;

    public GameObject m_creepingLavaPrefab;
    public GameObject m_realisticLavaPrefab;
    public float m_wait = 1.0f;

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
