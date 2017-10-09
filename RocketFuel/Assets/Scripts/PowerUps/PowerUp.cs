using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerType
    {
        TripleJump,
        SlowLava,
    }

    public PowerType m_powerType;

    public PowerType GetPowerType()
    {
        return m_powerType;
    }
}