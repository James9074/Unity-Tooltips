using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class TooltipTrigger : MonoBehaviour
{
    [SerializeField]
    string mDisplayText;
    
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        AddPointerEnterTrigger(trigger, EventTriggerType.PointerEnter);
        AddEventTrigger(trigger, OnPointerExit, EventTriggerType.PointerExit);
    }

    private void AddPointerEnterTrigger(EventTrigger evTrig, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        AddEventTrigger(evTrig, d => OnPointerEnter(d, evTrig.gameObject), EventTriggerType.PointerEnter);
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }

    private void AddEventTrigger(EventTrigger evTrig, UnityAction action, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        triggerEvent.AddListener((eventData) => action());
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }


    private void AddEventTrigger(EventTrigger evTrig, UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        triggerEvent.AddListener((eventData) => action(eventData));
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }

    private void OnPointerEnter(BaseEventData dataObject, GameObject hovered)
    {
        if (hovered != null)
            Tooltip.Instance.SetTooltip(mDisplayText);
    }

    private void OnPointerExit()
    {
        Tooltip.Instance.HideTooltip();
    }

    public void SetTooltip(string aText)
    {
        mDisplayText = aText;
    }
}