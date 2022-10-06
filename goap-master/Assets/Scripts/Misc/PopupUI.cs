using UnityEngine;
using Platinio.TweenEngine;
using Platinio.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition = Vector2.zero;
    [SerializeField] private Vector2 desirePosition = Vector2.zero;
    [SerializeField] private RectTransform canvas = null;
    [SerializeField] private float time = 0.5f;
    [SerializeField] private Ease enterEase = Ease.EaseInOutExpo;
    [SerializeField] private Ease exitEase = Ease.EaseInOutExpo;

    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 desireScale = Vector3.one;
    [SerializeField] private Ease enterScaleEase = Ease.EaseInOutExpo;
    [SerializeField] private Ease exitScaleEase = Ease.EaseInOutExpo;

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
        thisRect.MoveUI(desirePosition, canvas, time).SetEase(enterEase).SetOnComplete(delegate
        {
            isBusy = false;
            isVisible = true;
        });
        thisRect.ScaleTween(desireScale, time).SetEase(enterScaleEase).SetOnComplete(delegate
        {
                //isBusy = false;
                //isVisible = true;
        });
    }

    private void Hide()
    {
        thisRect.MoveUI(startPosition, canvas, time).SetEase(exitEase).SetOnComplete(delegate
        {
            isBusy = false;
            isVisible = false;
        });
        thisRect.ScaleTween(startScale, time).SetEase(exitScaleEase).SetOnComplete(delegate
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


