using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timecontroller : MonoBehaviour
{
    public float timeslow = .2f;
    public static Timecontroller Instance_time_Slow;
    [HideInInspector]public bool runSlowMotion;
    [HideInInspector]public bool dontRunSlowMotion;
    private void Awake()
    {
        if(Timecontroller.Instance_time_Slow == null)
        {
            Timecontroller.Instance_time_Slow = this;
        }
    }
    void Update()
    {
        if (runSlowMotion)
        {
            Time.timeScale = timeslow;
            Time.fixedDeltaTime = timeslow * Time.deltaTime;
            runSlowMotion = false;
        }
        if (dontRunSlowMotion){
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.deltaTime;
            dontRunSlowMotion = false;
        }
    }
}