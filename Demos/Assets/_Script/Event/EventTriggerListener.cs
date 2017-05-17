using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// BabyBus
/// 点击事件的同一处理
/// 注：非UI物体 需在摄像机添加 Physics Raycaster 组件
/// （EventSystem - Force Module Active勾选）
/// 委托的使用
/// </summary>
public class EventTriggerListener : EventTrigger {
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDrag;
    public VoidDelegate onDragEnd;


    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
            listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
            onUp(gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null)
            onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null)
            onUpdateSelect(gameObject);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(gameObject);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onDragEnd != null)
        {
            onDragEnd(gameObject);
        }
    }
}