using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBase : PopupUI
{
    protected PersonBase person;

    protected override void Start()
    {
        base.Start();
        person = GetComponentInParent<PersonBase>();
    }

    public void ToggleCanvas(bool en)
    {
        canvas.enabled = en;
    }
}
