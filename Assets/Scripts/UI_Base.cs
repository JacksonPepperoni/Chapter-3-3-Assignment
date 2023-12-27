using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    protected enum UIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPressedHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnEndDragHandler = null;


    protected void BindEvent(Action<PointerEventData> action = null, UIEvent type = UIEvent.Click)  // BindEvent(함수명, Event종류);
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
            case UIEvent.Drag:
                OnDragHandler -= action;
                OnDragHandler += action;
                break;
            case UIEvent.BeginDrag:
                OnBeginDragHandler -= action;
                OnBeginDragHandler += action;
                break;
            case UIEvent.EndDrag:
                OnEndDragHandler -= action;
                OnEndDragHandler += action;
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

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragHandler?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragHandler?.Invoke(eventData);
    }

}