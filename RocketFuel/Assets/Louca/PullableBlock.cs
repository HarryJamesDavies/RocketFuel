using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullableBlock : MonoBehaviour
{
    public enum PullType
    {
        Up,
        Right,
        Down,
        Left,
        Forward
    }

    public PullType m_pullType;

    private GameObject m_clonePrefab;
    private GameObject m_clone;
    public GameObject m_particlePrefab;
    private GameObject m_particle;

    public float m_offset;
    public float m_moveSpeed;
    public float m_delay;
    public float m_particleDelay;

    private Vector3 m_targetPosition;

    // Use this for initialization
    private void Start()
    {
        m_clonePrefab = gameObject;
        StartCoroutine(DelaySpawn());
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_clone && Distance(m_clone.transform.position, m_targetPosition) > 0.05f)
        {
            m_clone.transform.position = Vector3.MoveTowards(m_clone.transform.position, m_targetPosition, Time.deltaTime * m_moveSpeed);
            //float t_angle = Mathf.LerpAngle(-m_angle, m_angle, Time.time);
            //print(t_angle);
            //m_clone.transform.eulerAngles = new Vector3(0f, 0f, t_angle);
        }
        else
        {
        }
    }

    public void SpawnClone()
    {
        Vector3 t_position = transform.position;
        m_targetPosition = GetNewPosition(t_position);
        m_clone = (GameObject)Instantiate(m_clonePrefab, transform.position, Quaternion.identity);
        Destroy(m_clone.GetComponent<PullableBlock>());
        m_clone.transform.parent = gameObject.transform;
        m_particle = (GameObject)Instantiate(m_particlePrefab, transform.position, Quaternion.identity);
        m_particle.transform.parent = m_clone.transform;
    }

    private Vector3 GetNewPosition(Vector3 t_position)
    {
        switch (m_pullType)
        {
            case PullType.Up:
                t_position += Vector3.up * m_offset;
                break;

            case PullType.Right:
                t_position += Vector3.right * m_offset;
                break;

            case PullType.Down:
                t_position += Vector3.down * m_offset;
                break;

            case PullType.Left:
                t_position += Vector3.left * m_offset;
                break;

            case PullType.Forward:
                t_position += Vector3.forward;
                break;
        }

        return t_position;
    }

    private float Distance(Vector3 _posA, Vector3 _posB)
    {
        return Vector3.Distance(_posA, _posB);
    }

    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(m_delay);
        SpawnClone();
    }
}