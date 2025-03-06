
#region Summary

//Awake -> Get Components of all manager needed

#endregion
using UnityEngine;

public class GetManagerComponents : MonoBehaviour
{
    [SerializeField] public EventManager eventManager;
    [SerializeField] public AudioManager audioManager;
    [SerializeField] public CanvasManager canvasManager;
    

    void Awake()
    {
        eventManager = GetComponent<EventManager>();

        if(eventManager == null)
        {
            eventManager = FindAnyObjectByType<EventManager>();
        }

        audioManager = GetComponent<AudioManager>();

        if (audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        canvasManager = GetComponent<CanvasManager>();

        if (canvasManager == null)
        {
            canvasManager = FindAnyObjectByType<CanvasManager>();
        }
    }
}
