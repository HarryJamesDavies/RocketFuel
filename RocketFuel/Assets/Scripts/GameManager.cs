using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool m_useFps = false;
    public Text m_fps;

    public int m_score;
    public Text m_scoreText;

    public GameObject m_player;

    private bool m_active = false;
    private bool m_enteredPlayMode = false;

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

            if(SceneManager.GetActiveScene().name == "Harry")
            {
                m_enteredPlayMode = true;
            }

            DontDestroyOnLoad(gameObject);
        }
	}

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_PlayerWon, Ev_WinHandler);
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_PlayerDied, Ev_DeathHandler);
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_EnterMenu, Ev_EnterMenu);
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.GLO_EnterPlay, Ev_EnterPlay);
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_PlayerWon, Ev_WinHandler);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_PlayerDied, Ev_DeathHandler);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_EnterMenu, Ev_EnterMenu);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.GLO_EnterPlay, Ev_EnterPlay);
    } 

    void Update()
    {
        if(m_enteredPlayMode)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_enteredPlayMode = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (m_useFps)
        {
            m_fps.text = (1.0f / Time.deltaTime).ToString("F2");
        }

        if (m_active)
        {
            m_scoreText.text = "" + (int)m_player.transform.position.y;
        }
    }

    public void Ev_WinHandler(object _data = null)
    {
        m_active = false;
        m_score = (int)m_player.transform.position.y;
        Debug.Log("Player Wins!");
        SceneManager.LoadScene("GameOverScene");
    }

    public void Ev_DeathHandler(object _data = null)
    {
        m_active = false;
        m_score = (int)m_player.transform.position.y;
        Debug.Log("Player Dead! \nScore: " + m_score);
        SceneManager.LoadScene("GameOverScene");
    }

    public void Ev_EnterPlay(object _data = null)
    {
        m_active = true;
        m_score = 0;
        SceneManager.LoadScene("Main");
        m_enteredPlayMode = true;
    }

    public void Ev_EnterMenu(object _data = null)
    {
        m_active = false;
    }
}
