using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool m_useFps = false;
    public Text m_fps;

    public Text m_score;
    public Transform m_player;

    void Awake ()
    {
		if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;

            if (m_useFps && !Debug.isDebugBuild)
            {
                m_useFps = false;
            }
            else
            {
                m_fps.gameObject.SetActive(m_useFps);
            }
        }
	}

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_PlayerWon, Ev_WinHandler);
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_PlayerDied, Ev_DeathHandler);
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_PlayerWon, Ev_WinHandler);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_PlayerDied, Ev_DeathHandler);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        m_score.text = "" + (int)m_player.position.y;

        if (m_useFps)
        {
            m_fps.text = (1.0f / Time.deltaTime).ToString("F2");
        }
    }

    public void Ev_WinHandler(object _data = null)
    {
        Debug.Log("Player Wins!");
    }

    public void Ev_DeathHandler(object _data = null)
    {
        Debug.Log("Player Dead! \nScore: " + (int)m_player.position.y);
    }
}
