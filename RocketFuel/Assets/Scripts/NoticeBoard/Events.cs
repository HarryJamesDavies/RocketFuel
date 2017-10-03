using UnityEngine;
using System.Collections;

/* ####################################### */
//                                         //
//         GLO_ = Global Events            //
//         LEV_ = Level Events             //
//                                         //
/* ####################################### */

public class Events : MonoBehaviour
{
	public enum Event
    {
        GLO_EnterPlay,
        GLO_EnterMenu,
        GLO_PlayerWon,
        GLO_PlayerDied,
        LEV_TransitionSection,
        Count
    }
}
