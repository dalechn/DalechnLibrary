using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using UnityEngine.UI;

public class MyToggle : LeanToggle
{
    public Image bg;
    public Image handle;
    public Sprite handleOn;
    public Sprite handleOff;
    public Sprite onImage;
    public Sprite offImage;

    protected override void TurnOnNow()
    {
        base.TurnOnNow();
        handle.sprite = handleOn;
        bg.sprite = onImage;
    }

    protected override void TurnOffNow()
    {
        base.TurnOffNow();
        handle.sprite = handleOff;
        bg.sprite = offImage;
    }

}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using CW.Common;
    using UnityEditor;
	using TARGET = MyToggle;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanToggle_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            // *ADD
            Draw("bg", "");
            Draw("onImage", "");
            Draw("offImage", "");
            Draw("handle", "");
            Draw("handleOn", "");
            Draw("handleOff", "");

            if (Draw("on", "This lets you change the current toggle state of this UI element.") == true)
			{
				Each(tgts, t => t.On = serializedObject.FindProperty("on").boolValue, true);
			}

			if (Draw("turnOffSiblings", "If you enable this, then any sibling GameObjects (i.e. same parent GameObject) will automatically be turned off. This allows you to make radio boxes, or force only one panel to show at a time, etc.") == true)
			{
				Each(tgts, t => t.TurnOffSiblings = serializedObject.FindProperty("turnOffSiblings").boolValue, true);
			}

			Separator();

			Draw("onTransitions", "This allows you to perform a transition when this toggle turns on. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.\n\nNOTE: Any transitions you perform here should be reverted in the <b>Off Transitions</b> setting using a matching transition component.");
			Draw("offTransitions", "This allows you to perform a transition when this toggle turns off. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.");

			Separator();

			Draw("onOn");
			Draw("onOff");
		}
	}
}
#endif