using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    public int _superScale = 1;
    public string _name = "Screenshot_";

    private int _number = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot(_name + _number.ToString() + ".png", _superScale);
            _number++;
        }
    }
}
