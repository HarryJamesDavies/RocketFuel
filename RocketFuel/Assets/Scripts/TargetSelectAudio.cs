using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectAudio : MonoBehaviour {


    public float targetNextSelectPeriod = 0.8f;
    private float targetNextSelectTime = 0.0f;
    

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > targetNextSelectTime)
        {
            targetNextSelectTime += targetNextSelectPeriod;
            GetComponent<AudioSource>().Play();
        }
    }
}
