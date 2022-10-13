using UnityEngine;
using Platinio.TweenEngine;
using Platinio.UI;
using System.Collections;
using UnityEngine.UI;
using Lean.Transition;
using UnityEngine.Events;

namespace MyShop
{

    [Invector.vClassHeader("PopupUI")]
    public class PopupUI : vMonoBehaviour
    {
        [Invector.vEditorToolbar("default")]
        public RectTransform canvasTr = null;
        public PivotPreset pivot = PivotPreset.MiddleCenter;
        public UnityEvent startOvercall;
        public UnityEvent endOvercall;

        public float time = 0.5f;

        [vToggleOption]                                             //对bool类型加提示
        public bool autoHide = true;
        [Invector.vHideInInspector("autoHide", false)] // 隐藏inspector属性
        public float autoHideTime = 0;

        public LeanPlayer OnTransitions { get { if (onTransitions == null) onTransitions = new LeanPlayer(); return onTransitions; } }
        [SerializeField] private LeanPlayer onTransitions;

        public LeanPlayer OffTransitions { get { if (offTransitions == null) offTransitions = new LeanPlayer(); return offTransitions; } }
        [SerializeField] private LeanPlayer offTransitions;

        [Invector.vEditorToolbar("position")]
        public bool usePosition = true;
        public Vector2 startPosition = new Vector2(0.5f, 0.5f);
        public Vector2 showDesirePosition = new Vector2(0.5f, 0.5f);
        [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
        public Vector2 hideDesirePosition = new Vector2(0.5f, 0.5f);
        public LeanEase enterEase = LeanEase.Spring;
        public LeanEase exitEase = LeanEase.Spring;

        [Invector.vEditorToolbar("scale")]
        public bool useScale = true;
        public Vector3 startScale = Vector3.zero;
        public Vector3 showDesireScale = Vector3.one;
        [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
        public Vector3 hideDesireScale = Vector3.zero;
        public LeanEase enterScaleEase = LeanEase.Spring;
        public LeanEase exitScaleEase = LeanEase.Spring;

        [Invector.vEditorToolbar("rotation")]
        public bool useRotation = false;
        public Vector3 startRotation = Vector3.zero;
        [vHelpBox("只有z值有用", vHelpBoxAttribute.MessageType.Info)]
        public Vector3 showDesireRotation = Vector3.zero;
        [vHelpBox("一般等于start", vHelpBoxAttribute.MessageType.Info)]
        public Vector3 hideDesireRotation = Vector3.zero;
        public LeanEase enterRotationEase = LeanEase.Spring;
        public LeanEase exitRotationEase = LeanEase.Spring;

        protected bool isVisible = false;
        protected bool isBusy = false;
        protected RectTransform thisRect = null;

        protected GraphicRaycaster touch;
        protected Canvas canvas;

        protected virtual void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            touch = canvas.GetComponent<GraphicRaycaster>();

            if (!canvasTr)
            {
                canvasTr = canvas.GetComponent<RectTransform>();
            }

            thisRect = GetComponent<RectTransform>();

            if (usePosition)
            {
                thisRect.anchoredPosition = thisRect.FromAbsolutePositionToAnchoredPosition(startPosition, canvasTr, pivot);
            }

            if (useScale)
            {
                thisRect.localScale = startScale;
            }
            else
            {
                thisRect.localScale = Vector2.zero; //不使用动画的话默认scale = 0;
            }

            if (useRotation)
            {
                thisRect.rotation = Quaternion.Euler(startRotation);
            }

        }

        public void EnableRaycast(bool e)
        {
            touch.enabled = e;
        }

        public float TotalTime()
        {
            return time * 2 + autoHideTime;
        }

        public virtual void Show()
        {
            if (isVisible)
            {
                return;
            }


            if (onTransitions != null)
            {
                onTransitions.Begin();
            }

            if (usePosition)
            {
                Vector2 pos = thisRect.FromAbsolutePositionToAnchoredPosition(showDesirePosition, canvasTr, pivot);
                thisRect.anchoredPositionTransition(pos, time, enterEase).JoinTransition().EventTransition(() =>
                {
                    startOvercall.Invoke();

                    isBusy = false;
                    isVisible = true;
                    //Debug.Log("Hello World!");
                });
                //thisRect.MoveUI(showDesirePosition, canvas, time).SetEase(enterEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = true;
                //});
            }
            if (useScale)
            {
                thisRect.localScaleTransition(showDesireScale, time, enterScaleEase).JoinTransition().EventTransition(() =>
                {
                    isBusy = false;
                    isVisible = true;
                });

                //thisRect.ScaleTween(showDesireScale, time).SetEase(enterScaleEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = true;
                //});
            }
            else
            {
                thisRect.localScale = showDesireScale;
            }
            if (useRotation)
            {
                Quaternion toRotation = Quaternion.Euler(transform.forward * showDesireRotation.z);
                thisRect.rotationTransition(toRotation, time, enterRotationEase).JoinTransition().EventTransition(() =>
                {
                    isBusy = false;
                    isVisible = true;
                });
                //thisRect.RotateTween(transform.forward, showDesireRotation.z, time).SetEase(enterScaleEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = true;
                //});
            }

            if (autoHide && autoHideTime > 0)
            {
                StartCoroutine(CoroutineHide());
            }
        }

        protected IEnumerator CoroutineHide()
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
            if (isVisible && !isBusy)
            {
                Hide();
                CancelInvoke("RepeatingTryHide");
            }
        }

        public virtual void Hide()
        {
            if (!isVisible)
            {
                return;
            }

            if (offTransitions != null)
            {
                offTransitions.Begin();
            }

            if (usePosition)
            {
                //这函数还有问题? 已知需要设置锚点模式为PivotPreset.MiddleCenter,好像stretch也xing?
                Vector2 pos = thisRect.FromAbsolutePositionToAnchoredPosition(hideDesirePosition, canvasTr, PivotPreset.MiddleCenter);
                thisRect.anchoredPositionTransition(pos, time, exitEase).JoinTransition().EventTransition(() =>
                {
                    endOvercall.Invoke();

                    isBusy = false;
                    isVisible = false;

                    if (!useScale)
                    {
                        thisRect.localScale = Vector2.zero;
                    }
                });

                //thisRect.MoveUI(hideDesirePosition, canvas, time).SetEase(exitEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = false;
                //});
            }
            if (useScale)
            {
                thisRect.localScaleTransition(hideDesireScale, time, exitScaleEase).JoinTransition().EventTransition(() =>
                {
                    isBusy = false;
                    isVisible = false;
                });

                //thisRect.ScaleTween(hideDesireScale, time).SetEase(exitScaleEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = false;
                //});
            }
            if (useRotation)
            {
                Quaternion toRotation = Quaternion.Euler(transform.forward * hideDesireRotation.z);
                thisRect.rotationTransition(toRotation, time, exitRotationEase).JoinTransition().EventTransition(() =>
                {
                    if (!useScale)
                    {
                        thisRect.localScale = Vector2.zero;
                    }

                    isBusy = false;
                    isVisible = false;
                });
                //thisRect.RotateTween(transform.forward, hideDesireRotation.z, time).SetEase(exitRotationEase).SetOnComplete(delegate
                //{
                //    isBusy = false;
                //    isVisible = false;
                //});
            }
        }

        public virtual void Toggle()
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


}