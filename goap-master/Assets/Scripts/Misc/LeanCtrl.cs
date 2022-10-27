using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using Lean.Common;

//用来控制topdown ctrl的
public class LeanCtrl : MonoBehaviour
{
    private LeanPitchYaw leanPitchYaw;
    private LeanDragTranslateAlong leanDragTranslateAlong;

    private LeanSelectByFinger finger;
    // Start is called before the first frame update

    private ValueWatcher watcher;
    void Start()
    {
        leanPitchYaw = GetComponent<LeanPitchYaw>();
        leanDragTranslateAlong = GetComponent<LeanDragTranslateAlong>();
        finger = FindObjectOfType<LeanSelectByFinger>();

        watcher = new ValueWatcher(finger.Selectables.Count>0);
    }

    public void Toggle(bool en)
    {
        leanDragTranslateAlong.enabled = en;
        leanPitchYaw.enabled = en;
    }

    // Update is called once per frame
    void Update()
    {
        watcher.Watch(finger.Selectables.Count <= 0, (e) => { Toggle((bool)(e)); });
    }
}
