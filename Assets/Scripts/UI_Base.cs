using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected enum UIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        PointerEnter,
        PointerExit
    }

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPressedHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;
    public Action<PointerEventData> OnPointerEnterHandler = null;
    public Action<PointerEventData> OnPointerExitHandler = null;

    protected void BindEvent(Action<PointerEventData> action = null, UIEvent type = UIEvent.Click)
    {
        switch (type)
        {
            case UIEvent.Click:
                OnClickHandler -= action;
                OnClickHandler += action;
                break;
            case UIEvent.Preseed:
                OnPressedHandler -= action;
                OnPressedHandler += action;
                break;
            case UIEvent.PointerDown:
                OnPointerDownHandler -= action;
                OnPointerDownHandler += action;
                break;
            case UIEvent.PointerUp:
                OnPointerUpHandler -= action;
                OnPointerUpHandler += action;
                break;
            case UIEvent.PointerEnter:
                OnPointerEnterHandler -= action;
                OnPointerEnterHandler += action;
                break;
            case UIEvent.PointerExit:
                OnPointerExitHandler -= action;
                OnPointerExitHandler += action;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitHandler?.Invoke(eventData);
    }
}