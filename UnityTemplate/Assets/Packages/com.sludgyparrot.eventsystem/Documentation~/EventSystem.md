# Event System API

By using the Event System data provider API, the user can subscribe/unsubscribe and publish events.

## How to use the Event Bus instance

Below are some examples of how to implement the Event System using the Event Bus API. The Event Bus sub class is located inside the [Parrot Code Event System](ParrotCodeEventSystem) 

### Event Publishing

```C#
using UnityEngine;
using ParrotCode.EventSystem;

public sealed class InputActionEvent
{
    private readonly int value;

    public InputActionEvent(int value)
    {
        this.value = value;
    }

    public string ToString()
    {
        return value.ToString();
    }
}

public sealed class InputEventsPublisher: MonoBehaviour
{
    private void Start()
    {
        EventBus.TriggerEvent(new InputActionEvent(100));
    }
}
```

### Event Subscriptions

```C#

using UnityEngine;
using ParrotCode.EventSystem;

public sealed class InputEventsSubscriber: MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.AddListener(RecievedInputEvent);
    }

    private void RecievedInputEvent(InputActionEvent inputEvent)
    {
        Debug.Log(inputEvent.ToString());
    }
}
```

## Additional resources

- Full list of other available data providers is available in the package's [Scripting API section](xref:UnityEditor.U2D.Sprites).