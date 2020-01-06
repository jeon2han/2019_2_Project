using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenResolution : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
