using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class TooltipTrigger : MonoBehaviour
{
    [SerializeField]
    string mDisplayText;

    // Use this for initialization
    void Start()
    {
        EventTrigger trig = GetComponent<EventTrigger>();
        AddPointerEnterTrigger(trig, EventTriggerType.PointerEnter);
        AddEventTrigger(trig, OnPointerExit, EventTriggerType.PointerExit);
    }

    private void AddPointerEnterTrigger(EventTrigger evTrig, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        AddEventTrigger(evTrig, d => OnPointerEnter(d, evTrig.gameObject), EventTriggerType.PointerEnter);
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }

    private void AddEventTrigger(EventTrigger evTrig, UnityAction action, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action());
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }


    private void AddEventTrigger(EventTrigger evTrig, UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action(eventData));
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
        evTrig.triggers.Add(entry);
    }

    private void OnPointerEnter(BaseEventData dataObject, GameObject hovered)
    {
        if (hovered != null)
        {
                Tooltip.Instance.SetTooltip(mDisplayText);
        }
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