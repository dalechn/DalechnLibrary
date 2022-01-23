using Invector.vCharacterController.v2_5D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vSetPlayer2_5Path : MonoBehaviour
{
    public v2_5DPath path;

    public void ApplyPath(Collider other)
    {
        ApplyPath(other.gameObject);
    }

    public void ApplyPath(GameObject other)
    {
        var c2_5D = other.GetComponent<v2_5DController>();
        if (c2_5D)
        {
            path.Init();
            c2_5D.path = path;          
            c2_5D.InitPath();
        }
    }
}
