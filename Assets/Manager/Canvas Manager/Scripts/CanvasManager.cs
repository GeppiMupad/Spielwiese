
using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject firstCanvas;
    [SerializeField] private GameObject secondCanvas;
    [SerializeField] private GameObject thirdCanvas;

    [Space(10)]
    [SerializeField] private GetManagerComponents managerComponents;

    // fade In & Out

    [Space(10)]
    [SerializeField] private float fadeRate;

    [SerializeField, Range(0.1f, 0.8f)] private float lowestAlpha;
    [SerializeField, Range(0.4f, 1f)]   private float highestAlpha;

    private bool fullAlpha = false;


    // Increase & Decrease FontSize

    [SerializeField] private float fontSizeRate;

    [SerializeField] private float lowestFontSize;
    [SerializeField] private float highestFontSize;

    private bool maxFontSize = false;

    private void Awake()
    {
        managerComponents = GetComponent<GetManagerComponents>();

        if (managerComponents == null)
        {
            managerComponents = FindAnyObjectByType<GetManagerComponents>();
        }     
    }

    void Start()
    {
        managerComponents.eventManager.eventOpenPauseCanvas += ManageFirstCanvas;
        managerComponents.eventManager.eventOpenSettingsCanvas += ManageSecondCanvas;
        managerComponents.eventManager.eventOpenAudioCanvas += ManageThirdCanvas;
    }

    public IEnumerator TextFadeInAndOut(TextMeshProUGUI _text)
    {
        if(fullAlpha == true)
        {
            _text.alpha -= Time.deltaTime / fadeRate;

            if (_text.alpha <= lowestAlpha)
            {
                fullAlpha = false;
                yield return 0;
            }
            yield return 0;
        }
       
        if(fullAlpha == false)
        {
            _text.alpha += Time.deltaTime / fadeRate;

            if (_text.alpha >= highestAlpha)
            {
                fullAlpha = true;
                yield return 0;
            }
        }

        yield return 0;
    }

    public IEnumerator TextIncreaseAndDecreaseFontSize(TextMeshProUGUI _text)
    {
        _text.alignment = TextAlignmentOptions.Center;
        if (maxFontSize == true)
        {
            _text.fontSize -= Time.deltaTime / fontSizeRate;

            if (_text.fontSize <= lowestFontSize)
            {
                maxFontSize = false;
                yield return 0;
            }
            yield return 0;
        }

        if (maxFontSize == false)
        {
            _text.fontSize += Time.deltaTime / fontSizeRate;

            if (_text.fontSize >= highestFontSize)
            {
                maxFontSize = true;
                yield return 0;
            }
        }

        yield return 0;
    }

    private void ManageFirstCanvas()
    {
        if (firstCanvas.activeSelf == true)
        {
            firstCanvas.SetActive(false);
        }
        else 
        {
            secondCanvas.SetActive(false);
            thirdCanvas.SetActive(false);

            firstCanvas.SetActive(true);
        }
    }
    private void ManageSecondCanvas()
    {
        if (secondCanvas.activeSelf == true)
        {
            secondCanvas.SetActive(false);
        }
        else 
        {
            thirdCanvas.SetActive(false);
            firstCanvas.SetActive(false);

            secondCanvas.SetActive(true);
        }
    }
    private void ManageThirdCanvas()
    {
        if (thirdCanvas.activeSelf == true)
        {
            thirdCanvas.SetActive(false);
        }
        else 
        {
            secondCanvas.SetActive(false);
            firstCanvas.SetActive(false);

            thirdCanvas.SetActive(true);
        }
    }

}
