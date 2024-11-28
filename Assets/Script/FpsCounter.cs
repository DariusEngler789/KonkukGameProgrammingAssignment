using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    int frameCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(UpdateCounter), 0.0f, 1.0f);
    }

    void UpdateCounter()
    {
        text.text = $"{frameCount} FPS";
        frameCount = 0;
    }

    void Update()
    {
        if (Time.timeScale != 0)
            frameCount++;
    }
}
