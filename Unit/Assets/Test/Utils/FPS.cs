using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{

    private float currentTime = 0;
    private float lateTime = 0;

    private float framesNum = 0;
    private float fpsTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;

        framesNum++;

        if (currentTime - lateTime >= 1.0f)
        {
            fpsTime = framesNum / (currentTime - lateTime);

            lateTime = currentTime;

            framesNum = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), "fps:" + fpsTime.ToString());
    }
}
