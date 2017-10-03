using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class GameOverManager : MonoBehaviour
{
    public float m_moveTime;
    public float m_flashTime;

    public string m_startSceneName;

    public GameObject m_gameOverCanvas;

    public Text m_distanceText;
    public Text m_gameOverText;

    private int m_playerId = 0;
    private Player m_player;

    public static GameOverManager m_instance;

    // Use this for initialization
    private void Start()
    {
        CreateInstance();
        m_player = ReInput.players.GetPlayer(m_playerId);
        UpdateDistanceText(0);
        StartCoroutine(MoveToStartScreen());
        InvokeRepeating("GameOverFlash", m_flashTime, m_flashTime);
    }

    private void CreateInstance()
    {
        if (!m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_player.GetAnyButtonDown())
        {
            SceneManager.LoadScene(m_startSceneName);
        }
    }

    private IEnumerator MoveToStartScreen()
    {
        yield return new WaitForSeconds(m_moveTime);
        GlobalEventBoard.Instance.AddEvent(Events.Event.GLO_EnterMenu);
        SceneManager.LoadScene(m_startSceneName);
    }

    public void UpdateDistanceText(int _distance)
    {
        m_distanceText.text = "You Reached: " + GameManager.Instance.m_score + "M";
    }

    private void GameOverFlash()
    {
        m_gameOverText.enabled = !m_gameOverText.enabled;
    }
}