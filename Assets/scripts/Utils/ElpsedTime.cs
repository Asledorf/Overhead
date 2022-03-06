using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElpsedTime : MonoBehaviour
{
    [SerializeField] Text TEXT;

    float dt = 0;

    void Update()
    {
        dt += Time.deltaTime;
        int _ = (int)dt;
        int ss = _%60;
        int mm = (_/60)%60;
        int hh = (_/3600)%60;

        TEXT.text = $"{hh:D2}:{mm:D2}:{ss:D2}";
    }
}
