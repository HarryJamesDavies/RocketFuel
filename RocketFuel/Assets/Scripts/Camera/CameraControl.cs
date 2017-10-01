using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform m_target;
    public DimesionFlags m_snapToOnAwake = new DimesionFlags();

    void Awake()
    {
        transform.position = m_snapToOnAwake.AdjustAToB(transform.position, m_target.position);
    }

	void LateUpdate ()
    {
        transform.position = m_snapToOnAwake.AdjustAToB(transform.position, m_target.position);
    }
}
