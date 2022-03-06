using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour
{
    public Image clock;
    public Text speed;
    private float currentTimeScale;

    void Start()
    {
        currentTimeScale = Time.timeScale;
    }

    void Update()
    {
        if (currentTimeScale != Time.timeScale)
        {
            currentTimeScale = Time.timeScale;

            if (Time.timeScale == 1)
            {
                clock.color = Color.white;
                speed.text = "1x";
            }
            else
            if (Time.timeScale == 3)
            {
                clock.color = Color.green;
                speed.text = "3x";
            }
            else
            {
                clock.color = Color.red;
                speed.text = "paused";
            }
        }
    }
}
