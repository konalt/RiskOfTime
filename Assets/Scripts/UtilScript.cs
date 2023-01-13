using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilScript : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        #endif
    }
}
