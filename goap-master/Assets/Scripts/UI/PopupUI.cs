using UnityEngine;
using Platinio.TweenEngine;
using Platinio.UI;
using System.Collections;

[Invector.vClassHeader("PopupUI")]
public class PopupUI : vMonoBehaviour
{
    public RectTransform canvas = null;
    public float time = 0.5f;
    [vHelpBox("大于0会自动隐藏", vHelpBoxAttribute.MessageType.Info)]
    public float autoHideTime = 0;

    [Invector.vEditorToolbar("position")]
    public Vector2 startPosition = Vector2.zero;
    public Vector2 showDesirePosition = Vector2.zero;
    [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
    public Vector2 hideDesirePosition = Vector2.zero;
    public Ease enterEase = Ease.EaseInOutExpo;
    public Ease exitEase = Ease.EaseInOutExpo;

    [Invector.vEditorToolbar("scale")]
    public Vector3 startScale = Vector3.zero;
    public Vector3 showDesireScale = Vector3.one;
    [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
    public Vector3 hideDesireScale = Vector3.one;
    public Ease enterScaleEase = Ease.EaseInOutExpo;
    public Ease exitScaleEase = Ease.EaseInOutExpo;

    private bool isVisible = false;
    private bool isBusy = false;
    private RectTransform thisRect = null;

    private void Start()
    {
        thisRect = GetComponent<RectTransform>();

        thisRect.anchoredPosition = thisRect.FromAbsolutePositionToAnchoredPosition(startPosition, canvas);

        thisRect.localScale = startScale;
    }

    private void Show()
    {
        thisRect.MoveUI(showDesirePosition, canvas, time).SetEase(enterEase).SetOnComplete(delegate
        {
            isBusy = false;
            isVisible = true;
        });
        thisRect.ScaleTween(showDesireScale, time).SetEase(enterScaleEase).SetOnComplete(delegate
        {
            //isBusy = false;
            //isVisible = true;
        });

        StopCoroutine(CoroutineHide());

        if(autoHideTime>0)
        {
            StartCoroutine(CoroutineHide());
        }
    }

    private IEnumerator CoroutineHide()
    {
        yield return new WaitForSeconds(autoHideTime);

        Hide();
    }

    private void Hide()
    {
        thisRect.MoveUI(hideDesirePosition, canvas, time).SetEase(exitEase).SetOnComplete(delegate
        {
            isBusy = false;
            isVisible = false;
        });
        thisRect.ScaleTween(hideDesireScale, time).SetEase(exitScaleEase).SetOnComplete(delegate
        {
            //isBusy = false;
            //isVisible = true;
        });
    }

    public void Toggle()
    {
        if (isBusy)
            return;

        isBusy = true;

        if (isVisible)
            Hide();
        else
            Show();
    }
}


