using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;

public class vIndicator : MonoBehaviour
{
    public UltimateJoystick joystick;

    public float targetLerpSpeed = 10;
    public float noarmlLerpSpeed = 30;

    void Start()
    {
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        if (!joystick)
        {
            if (PlayerController.instance.targetDistributor. target.target)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerController.instance.targetDistributor.target.forward), targetLerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = PlayerController.instance.transform.rotation;
            }

            transform.position = PlayerController.instance.transform.position;
        }

    }

}
