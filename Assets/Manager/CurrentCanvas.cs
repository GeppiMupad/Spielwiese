using TMPro;
using UnityEngine;

public class CurrentCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fadeInAndOutText;
    [SerializeField] private TextMeshProUGUI increadeAndDecreaseText;

    [SerializeField] private TextMeshProUGUI[] fadeInAndOutTextA;
    [SerializeField] private TextMeshProUGUI[] increadeAndDecreaseTextA;

    [SerializeField] private GetManagerComponents managerComponents;
    void Awake()
    {
        managerComponents = GetComponent<GetManagerComponents>();

        if (managerComponents == null)
        {
            managerComponents = FindAnyObjectByType<GetManagerComponents>();
        }
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        //if(fadeInAndOutTextA != null)
        //{
        //    foreach (TextMeshProUGUI item in fadeInAndOutTextA)
        //    {
        //        StartCoroutine(managerComponents.canvasManager.TextFadeInAndOut(item));
        //    }          
        //}
        //if(increadeAndDecreaseTextA != null)
        //{
        //    foreach (TextMeshProUGUI item in increadeAndDecreaseTextA)
        //    {
        //        StartCoroutine(managerComponents.canvasManager.TextFadeInAndOut(item));
        //    }
        //}   

        StartCoroutine(managerComponents.canvasManager.TextFadeInAndOut(fadeInAndOutText));
        StartCoroutine(managerComponents.canvasManager.TextIncreaseAndDecreaseFontSize(increadeAndDecreaseText));
    }
}
