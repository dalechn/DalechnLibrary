using Lean.Touch;
using Lean.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSlot : Slot
{
    private LeanSelectableByFinger select;
    public LeanPlayer player;       //不用这个了效果不好
    public GameObject dampObject;

    protected override void Start()
    {
        base.Start();

        select = GetComponentInChildren<LeanSelectableByFinger>();

        select.OnSelected.AddListener((s) =>
        {
            //player.Begin();

            Dalechn.GameUtils.DampAnimation(dampObject, 0.5f);

            {
                ModeFrame top = UIManager.Instance.GetObj<ModeFrame>(PopType.modeFrame.ToString());
                top.ShowMask(foodList[0].transform);

                //select.enabled = false;

                if (foodList[0].activeInHierarchy)
                {
                    foodList[0].SetActive(false);
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
