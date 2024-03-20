using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class SteeringWheel : MonoBehaviour
{
    public Graphic UI_Element;
    public DemoPlayer player;

    RectTransform rectT;
    Vector2 centerPoint;

    public float maximumSteeringAngle = 200f;
    public float wheelReleasedSpeed = 200f;

    public float driftThreshold;
    public float deadzoneThreshold;

    float wheelAngle = 0f;
    float wheelPrevAngle = 0f;

    bool wheelBeingHeld = false;

    public float GetClampedValue()
    {
        // returns a value in range [-1,1] similar to GetAxis("Horizontal")
        return wheelAngle / maximumSteeringAngle;
    }

    public float GetAngle()
    {
        // returns the wheel angle itself without clamp operation
        return wheelAngle;
    }

    void Start()
    {
        rectT = UI_Element.rectTransform;
        InitEventsSystem();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<DemoPlayer>();
    }

    void Update()
    {
        // If the wheel is released, reset the rotation
        // to initial (zero) rotation by wheelReleasedSpeed degrees per second
        if (!wheelBeingHeld && !Mathf.Approximately(0f, wheelAngle))
        {
            float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
            if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
                wheelAngle = 0f;
            else if (wheelAngle > 0f)
                wheelAngle -= deltaAngle;
            else
                wheelAngle += deltaAngle;
        }

        // Rotate the wheel image
        rectT.localEulerAngles = Vector3.back * wheelAngle;

        //Debug.Log("Steering Value: " + GetClampedValue());
    }

    void InitEventsSystem()
    {
        // Warning: Be ready to see some extremely boring code here :-/
        // You are warned!
        EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();

        if (events == null)
            events = UI_Element.gameObject.AddComponent<EventTrigger>();

        if (events.triggers == null)
            events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = callback;

        events.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(DragEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.Drag;
        entry.callback = callback;

        events.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(ReleaseEvent);//
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback = callback;

        events.triggers.Add(entry);
    }

    public void PressEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger starts touching the steering wheel
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        wheelBeingHeld = true;
        centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
        wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
    }

    public void DragEvent(BaseEventData eventData)
    {
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);

        // Calculate the angular velocity
        float deltaAngle = wheelNewAngle - wheelPrevAngle;
        float angularVelocity = deltaAngle / Time.deltaTime;

        // Update the wheel angle based on the direction of rotation
        float rotationDirection = Mathf.Sign(pointerPos.x - centerPoint.x);
        wheelAngle += (wheelNewAngle - wheelPrevAngle) * rotationDirection;

        // Apply deadzone
        if (Mathf.Abs(wheelAngle) < deadzoneThreshold)
        {
            player.inputSteer = false;
        }
        else
        {
            player.inputSteer = true;
        }

        // Make sure wheel angle never exceeds maximumSteeringAngle
        wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);

        // Rotate the wheels based on the angular velocity
        RotateWheels(-angularVelocity);

        // Update the previous angle for the next frame
        wheelPrevAngle = wheelNewAngle;

        if (player == null)
        {
            Debug.LogError("Player Not Found");
        }
    }
    void RotateWheels(float angularVelocity)
    {
        // Rotate the wheels based on the angular velocity
        float wheelRotationAmount = angularVelocity * Time.deltaTime;
        // Apply the rotation to your wheels here
        // For example, transform.Rotate(Vector3.forward, wheelRotationAmount);
    }

    public void ReleaseEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger stops touching the steering wheel
        // Performs one last DragEvent, just in case
        DragEvent(eventData);
        player.releaseWheel();
        wheelBeingHeld = false;
    }
}