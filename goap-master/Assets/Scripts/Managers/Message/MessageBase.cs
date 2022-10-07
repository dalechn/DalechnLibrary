using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBase : MonoBehaviour
{
    protected PopupUI pop;
    protected PersonBase person;



    protected virtual void Start()
    {
        person = GetComponentInParent<PersonBase>();
        pop = GetComponent<PopupUI>();

    }

    void Update()
    {

    }

    public virtual void HandleMessage(MessageType emoji, Order messageText)
    {
        pop.Toggle();

    }
}
