# EventManager
Eventmanager for simple event management. Uses dictionary, which could be replaced with something else.

Is saved to singleton and inits itself.
Contained methods:
- StartListening(string tag, UnityAction method)
- StopListening(string tag, UnityAction method)
- TriggerEvent(string tag)

## Example usage

```csharp

public class HelloWorld : MonoBehaviour
{
    private void Hello() {
        Debug.Log("Hello world!");
    }
    
    void Update() {
        GS.Event.EventManager.TriggerEvent("EventTag");
    }
    
    void OnEnable() {
        GS.Event.EventManager.StartListening("EventTag", Hello);
    }
    void OnDisable() {
        GS.Event.EventManager.StopListening("EventTag", Hello);
    }
}

```

## Known events

Known event name usages:
- Language, used to tell that player changed his default language.
