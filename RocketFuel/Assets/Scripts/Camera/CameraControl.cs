using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform m_target;
    public DimesionFlags m_snapToOnAwake = new DimesionFlags();
    public float m_yOffset;

    private void Awake()
    {
        Vector3 t_position = new Vector3(10f, m_target.transform.position.y, transform.position.z);
        transform.position = m_snapToOnAwake.AdjustAToB(transform.position, t_position);
    }

    private void LateUpdate()
    {
        Vector3 t_position = new Vector3(10f, m_target.transform.position.y - m_yOffset, transform.position.z);
        transform.position = m_snapToOnAwake.AdjustAToB(transform.position, t_position);
    }
}