using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandomiser : MonoBehaviour
{
    private GameObject m_background;
    private SpriteRenderer m_spriteRenderer;
    private Sprite m_sprite;

    public Vector2 m_offsetMinMax;

    // Use this for initialization
    private void Start()
    {
        GetData();

        FlipX();
        FlipY();

        OffsetBackground();
    }

    private void OffsetBackground()
    {
        float t_xPosOffset = Random.Range(-m_offsetMinMax.x, m_offsetMinMax.x);
        float t_yPosOffset = Random.Range(-m_offsetMinMax.y, m_offsetMinMax.y);
        gameObject.transform.position += new Vector3(t_xPosOffset, t_yPosOffset, 0f);
    }

    private void FlipX()
    {
        int t_rand = Random.Range(0, 100);
        if (t_rand < 25)
        {
            m_spriteRenderer.flipX = true;
        }
        else
        {
            m_spriteRenderer.flipX = false;
        }
    }

    private void FlipY()
    {
        int t_rand = Random.Range(0, 100);
        if (t_rand < 25)
        {
            m_spriteRenderer.flipY = true;
        }
        else
        {
            m_spriteRenderer.flipY = false;
        }
    }

    private void GetData()
    {
        m_background = this.gameObject;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_sprite = m_spriteRenderer.sprite;
    }

    public void Randomiser()
    {
        FlipX();
        FlipY();
        OffsetBackground();
    }
}