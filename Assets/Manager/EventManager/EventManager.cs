using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action eventOpenPauseCanvas;
    public event Action eventOpenSettingsCanvas;
    public event Action eventOpenAudioCanvas;

    public void InvokeFirstCanvas()
    {
        eventOpenPauseCanvas?.Invoke();
    }

    public void InvokeSecondCanvas()
    {
        eventOpenSettingsCanvas?.Invoke();
    }

    public void InvokeThirdCanvas()
    {
        eventOpenAudioCanvas?.Invoke();
    }
}
