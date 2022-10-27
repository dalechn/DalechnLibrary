using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueWatcher 
{
    public object value;

    public ValueWatcher(object value)
    {
        this.value = value;
    }

    public void Watch(object val, Action<object> action)
    {
        if(val!=value)
        {
            action.Invoke(val);

            value = val;
        }
    }
}
