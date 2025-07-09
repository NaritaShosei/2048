using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public bool IsClicked { get; private set; } = false;
    public Action OnClick;
    public Action OnRightClick;
    public Action OnMouseDown;
    public Action OnMouseUp;
    public Action OnMouseEnter;
    public Action OnMouseExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsClicked) { return; }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick?.Invoke();
            IsClicked = true;
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke();
            IsClicked = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp?.Invoke();
    }
}
