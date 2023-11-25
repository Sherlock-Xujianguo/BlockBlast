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


    public void OnDrag(PointerEventData EventData)
    {
        transform.position = EventData.position - ClickOffsetPosition;
    }

    public void OnPointerDown(PointerEventData EventData)
    {
        ClickOffsetPosition = EventData.position - (Vector2)transform.position;
    }

    public void OnPointerUp(PointerEventData EventData)
    {
        transform.position = StartPosition;
    }
}
