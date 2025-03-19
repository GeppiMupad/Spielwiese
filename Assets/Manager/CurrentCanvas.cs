using TMPro;
using UnityEngine;

public class CurrentCanvas : MonoBehaviour
{
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
        StartAllCoroutines();
    }

    private void OnEnable()
    {
        StartAllCoroutines();
    }

    private void StartAllCoroutines()
    {
        if (fadeInAndOutTextA != null)
        {
            foreach (TextMeshProUGUI item in fadeInAndOutTextA)
            {
                StartCoroutine(managerComponents.canvasManager.TextFadeInAndOutOverTime(item));
            }
        }
        if (increadeAndDecreaseTextA != null)
        {
            foreach (TextMeshProUGUI item in increadeAndDecreaseTextA)
            {
                StartCoroutine(managerComponents.canvasManager.TextIncreaseAndDecreaseFontSizeOverTime(item));
                Debug.Log("is");   
            }
        }
    }
}
