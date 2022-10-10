using UnityEngine;
using Platinio.TweenEngine;
using Platinio.UI;
using System.Collections;
using UnityEngine.UI;

[Invector.vClassHeader("PopupUI")]
public class PopupUI : vMonoBehaviour
{
    [Invector.vEditorToolbar("default")]
    public RectTransform canvas = null;
    public float time = 0.5f;
    public float autoHideTime = 0;
    public bool autoHide = true;

    [Invector.vEditorToolbar("position")]
    public bool usePosition = true;
    public Vector2 startPosition = Vector2.zero;
    public Vector2 showDesirePosition = Vector2.zero;
    [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
    public Vector2 hideDesirePosition = Vector2.zero;
    public Ease enterEase = Ease.Spring;
    public Ease exitEase = Ease.Spring;

    [Invector.vEditorToolbar("scale")]
    public bool useScale = true;
    public Vector3 startScale = Vector3.zero;
    public Vector3 showDesireScale = Vector3.one;
    [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
    public Vector3 hideDesireScale = Vector3.zero;
    public Ease enterScaleEase = Ease.Spring;
    public Ease exitScaleEase = Ease.Spring;

    [Invector.vEditorToolbar("rotation")]
    public bool useRotation = false;
    public Vector3 startRotation = Vector3.zero;
    [vHelpBox("只有z值有用", vHelpBoxAttribute.MessageType.Info)]
    public Vector3 showDesireRotation = Vector3.zero;
    [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
    public Vector3 hideDesireRotation = Vector3.zero;
    public Ease enterRotationEase = Ease.EaseInOutExpo;
    public Ease exitRotationEase = Ease.EaseInOutExpo;

    private bool isVisible = false;
    private bool isBusy = false;
    private RectTransform thisRect = null;

    private GraphicRaycaster touch;

    protected virtual void Start()
    {
        thisRect = GetComponent<RectTransform>();

        if (usePosition)
        {
            thisRect.anchoredPosition = thisRect.FromAbsolutePositionToAnchoredPosition(startPosition, canvas);
        }
        if (useScale)
        {
            thisRect.localScale = startScale;
        }
        if (useRotation)
        {
            thisRect.rotation = Quaternion.Euler(startRotation);
        }

        touch = canvas.GetComponent<GraphicRaycaster>();
    }

    public void EnableRaycast(bool e)
    {
        touch.enabled = e;
    }

    public float TotalTime()
    {
        return time * 2 + autoHideTime;
    }

    public void Show()
    {
        if (usePosition)
        {
            thisRect.MoveUI(showDesirePosition, canvas, time).SetEase(enterEase).SetOnComplete(delegate
            {
                isBusy = false;
                isVisible = true;
            });
        }
        if (useScale)
        {
            thisRect.ScaleTween(showDesireScale, time).SetEase(enterScaleEase).SetOnComplete(delegate
            {
                //isBusy = false;
                //isVisible = true;
            });
        }
        if (useRotation)
        {
            thisRect.RotateTween(transform.forward, showDesireRotation.z, time).SetEase(enterScaleEase);
        }

        if (autoHide)
        {
            StartCoroutine(CoroutineHide());
        }
    }

    private IEnumerator CoroutineHide()
    {
        yield return new WaitForSeconds(autoHideTime);

        if (!isBusy)
            Hide();
    }

    public void TryHide()
    {
        InvokeRepeating("RepeatingTryHide", 0, Time.deltaTime);
    }

    private void RepeatingTryHide()
    {
        //Debug.Log(isVisible);
        if (isVisible&&!isBusy)
        {
            Hide();
            CancelInvoke("RepeatingTryHide");
        }
    }

    public void Hide()
    {
        if (usePosition)
        {
            thisRect.MoveUI(hideDesirePosition, canvas, time).SetEase(exitEase).SetOnComplete(delegate
            {
                isBusy = false;
                isVisible = false;
            });
        }
        if (useScale)
        {
            thisRect.ScaleTween(hideDesireScale, time).SetEase(exitScaleEase).SetOnComplete(delegate
            {
                //isBusy = false;
                //isVisible = true;
            });
        }
        if (useRotation)
        {
            thisRect.RotateTween(transform.forward, hideDesireRotation.z, time).SetEase(exitRotationEase);
        }
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


