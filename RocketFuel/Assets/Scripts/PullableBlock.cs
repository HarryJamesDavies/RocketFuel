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
        Forward,
        Normal
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
    public float m_maxAngle;
    private float m_minAngle;
    public float m_smoothness;
    private static float m_lerpTime;

    private Vector3 m_targetPosition;

    private bool m_hasMoved;

    //private SpriteRenderer m_spriteRenderer;

    //public Sprite[] m_sprites;

    // Use this for initialization
    private void Start()
    {
        m_clonePrefab = gameObject;
        m_minAngle = -m_maxAngle;
        m_lerpTime = 0f;
        //m_spriteRenderer = GetComponent<SpriteRenderer>();
        //m_spriteRenderer.sprite = m_sprites[(int)m_pullType];
        //StartCoroutine(DelaySpawn()); //test function call leave commmented
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_clone && Distance(m_clone.transform.position, m_targetPosition) > 0.01f)
        {
            m_clone.transform.position = Vector3.MoveTowards(m_clone.transform.position, m_targetPosition, Time.deltaTime * m_moveSpeed);
            float t_angle = Mathf.Lerp(m_minAngle, m_maxAngle, m_lerpTime);
            m_clone.transform.eulerAngles = new Vector3(0f, 0f, t_angle);

            m_lerpTime += m_smoothness * Time.deltaTime;
            if (m_lerpTime > 1f)
            {
                float t_tempAngle = m_maxAngle;
                m_maxAngle = m_minAngle;
                m_minAngle = t_tempAngle;
                m_lerpTime = 0f;
            }
        }
        else if (m_clone) //if the clone has finished moving reset rotation
        {
            if (m_clone.transform.rotation != Quaternion.identity)
            {
                m_clone.transform.rotation = Quaternion.identity;
            }
        }
    }

    public void SpawnClone()
    {
        if (!m_clone)
        {
            Vector3 t_position = transform.position;
            m_targetPosition = GetNewPosition(t_position);
            m_clone = (GameObject)Instantiate(m_clonePrefab, transform.position, Quaternion.identity);
            //m_clone.GetComponent<SpriteRenderer>().sprite = m_sprites[5];
            Destroy(m_clone.GetComponent<PullableBlock>());
            m_clone.transform.parent = gameObject.transform;
            m_particle = (GameObject)Instantiate(m_particlePrefab, transform.position, Quaternion.identity);
            m_particle.transform.parent = m_clone.transform;
        }
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

    public void DelayedSpawn()
    {
        StartCoroutine(DelaySpawn());
    }

    //Getters
    public PullType GetPullType()
    {
        return m_pullType;
    }

    public GameObject GetClone()
    {
        if (m_clone)
        {
            return m_clone;
        }
        else
        {
            return null;
        }
    }

    public bool HasClone()
    {
        if (m_clone)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Setters
    public void CloneSetUp(PullType _pullType, GameObject _clone, GameObject _particle, float _offset, float _moveSpeed, float _delay, float _particleDelay, float _angle, float _smoothness)
    {
        m_pullType = _pullType;
        m_clonePrefab = _clone;
        m_particlePrefab = _particle;
        m_offset = _offset;
        m_moveSpeed = _moveSpeed;
        m_delay = _delay;
        m_particleDelay = _particleDelay;
        m_maxAngle = _angle;
        m_minAngle = -m_maxAngle;
        m_smoothness = _smoothness;
        //m_spriteRenderer.sprite = m_sprites[(int)m_pullType];
    }

    public void CloneSetUp(int _pullType, GameObject _clone, GameObject _particle, float _offset, float _moveSpeed, float _delay, float _particleDelay, float _angle, float _smoothness)
    {
        m_pullType = (PullType)_pullType;
        m_clonePrefab = _clone;
        m_particlePrefab = _particle;
        m_offset = _offset;
        m_moveSpeed = _moveSpeed;
        m_delay = _delay;
        m_particleDelay = _particleDelay;
        m_maxAngle = _angle;
        m_minAngle = -m_maxAngle;
        m_smoothness = _smoothness;
        //m_spriteRenderer.sprite = m_sprites[(int)m_pullType];
    }
}