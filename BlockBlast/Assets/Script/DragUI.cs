using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 StartPosition;
    private Vector2 ClickOffsetPosition;

    // Start is called before the first frame update
    public void Start()
    {
        StartPosition = transform.position;
    }

    public virtual void OnBaseDrag(PointerEventData EventData) { }
    public virtual void OnBasePointerDown(PointerEventData EventData) { }
    public virtual void OnBasePointerUp(PointerEventData EventData) { }

    public void OnDrag(PointerEventData EventData)
    {
        transform.position = EventData.position - ClickOffsetPosition;

        OnBaseDrag(EventData);
    }

    public void OnPointerDown(PointerEventData EventData)
    {
        ClickOffsetPosition = EventData.position - (Vector2)transform.position;
        OnBasePointerDown(EventData);
    }

    public void OnPointerUp(PointerEventData EventData)
    {
        transform.position = StartPosition;

        OnBasePointerUp(EventData);
    }
}
