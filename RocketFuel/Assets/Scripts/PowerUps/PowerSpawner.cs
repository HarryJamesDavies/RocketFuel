using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    public GameObject[] m_powerUps;

    // Use this for initialization
    private void Start()
    {
        int t_rand = Random.Range(0, 100);
        if (t_rand <= 25)
        {
            Instantiate(m_powerUps[Random.Range(0, m_powerUps.Length)], transform.position, Quaternion.identity);
        }
    }
}