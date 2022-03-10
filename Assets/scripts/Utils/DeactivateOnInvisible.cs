using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnInvisible : MonoBehaviour
{
    CanvasGroup cg;
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        ((RectTransform)gameObject.transform).sizeDelta = new Vector2(0, cg.alpha == 0 ? 0 : 116.5f); //this is expensive
    }
}
