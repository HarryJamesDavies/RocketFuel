using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TransitionScene : MonoBehaviour
{
    private Player m_player = null;

    private void Start()
    {
        m_player = ReInput.players.GetPlayer(0);
    }

	void Update ()
    {
		if(m_player.GetAnyButtonDown())
        {
            GlobalEventBoard.Instance.AddEvent(Events.Event.GLO_EnterPlay);
        }
	}
}
