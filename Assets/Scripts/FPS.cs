using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {

    public int fps;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        SetFPS(fps);
    }

    void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
