using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if (UNITY_IOS || UNITY_ANDROID)
using UnityStandardAssets.CrossPlatformInput;

public class v2_5MoveCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected RectTransform touchPointer;
    public int currentPointerID { get; protected set; }
    public bool isPressed { get; protected set; }

    void Start()
    {
        if (touchPointer) touchPointer.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleTouchDirection();
    }

    void HandleTouchDirection()
    {
        if (isPressed)
        {
            if (currentPointerID >= 0 && currentPointerID < Input.touches.Length)
            {
                Vector2 touchPos = Input.touches[currentPointerID].position;
                var mobileScreenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                vMousePositionHandler.Instance.SetMousePosition(touchPos - mobileScreenCenter);
                if (touchPointer) touchPointer.position = touchPos;
            }
            else
            {
                Vector2 touchPos = Input.mousePosition;
                var mobileScreenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                vMousePositionHandler.Instance.SetMousePosition(touchPos - mobileScreenCenter);
                if (touchPointer) touchPointer.position = touchPos;
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (touchPointer)
        {
            touchPointer.position = data.position;
            touchPointer.gameObject.SetActive(true);
        }
        isPressed = true;
        currentPointerID = data.pointerId;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (touchPointer)
        {
            touchPointer.position = data.position;
            touchPointer.gameObject.SetActive(false);
        }
        isPressed = false;
    }
}

#endif