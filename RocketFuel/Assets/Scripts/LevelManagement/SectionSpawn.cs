using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpawn : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Instance.AddSectionSpawn(transform.position);
    }
}
