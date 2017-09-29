using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            GlobalEventBoard.Instance.AddEvent(Events.Event.GLO_PlayerWon);
        }
    }
}
