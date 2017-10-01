using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float m_offset;
    public float m_delay;

    private GameObject m_lastObject = null;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Background" && other.gameObject != m_lastObject)
        {
            GameObject t_gameobject = other.gameObject;
            m_lastObject = t_gameobject;
            StartCoroutine(MoveBackground(t_gameobject));
        }
    }

    private IEnumerator MoveBackground(GameObject _background)
    {
        yield return new WaitForSeconds(m_delay);
        print(_background.name);
        print("position: " + _background.transform.position);
        _background.transform.position += new Vector3(0f, m_offset, 0f);
        print("new position: " + _background.transform.position);
    }
}