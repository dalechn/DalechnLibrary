using Lean.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    //��̬����ʳ���msg
    public class FoodMSG : MessageBase
    {
        [Invector.vEditorToolbar("UI")]
        public Image image;         //����
        public Image imageSlider;   //����slider;
        protected Sprite emojiSprite;

        protected override void Start()
        {
            base.Start();

            if (person)
            {
                MessageCenter.Instance.RegistMSG(person.gameObject, this);
            }
        }

        public void HandleMessage(MessageType emoji, Order order)
        {
            autoHideTime = order.currentFood.foodTime;

            emojiSprite = Resources.Load<Sprite>(order.currentFood.subFoodSpriteLocation);
            image.sprite = emojiSprite;
            imageSlider.sprite = emojiSprite;

            if (imageSlider)
            {
                //Dalechn.bl_UpdateManager.RunAction("", order.currentFood.foodTime, (t, r) =>
                //{
                //    imageSlider.fillAmount = t;
                //});
                imageSlider.fillAmountTransition(0,0).fillAmountTransition(1.0f, order.currentFood.foodTime,LeanEase.Linear);
            }

            Toggle();
        }
    }
}