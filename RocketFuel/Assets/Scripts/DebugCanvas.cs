using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    public Text m_debugText;

    public static DebugCanvas m_instance;

    // Use this for initialization
    private void Start()
    {
        if (!m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        m_debugText.text = "";
    }

    public void UpdateText(string _message)
    {
        m_debugText.text = _message;
    }
}