
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
    [Header("FadeRate")]
    [Space(10)]
    [SerializeField] private float fadeRate;

    [SerializeField, Range(0.1f, 0.8f)] private float lowestAlpha;
    [SerializeField, Range(0.4f, 1f)] private float highestAlpha;



    // Increase & Decrease FontSize
    [Header("FontSize")]
    [Space(10)]

    [SerializeField] private float fontSizeRate;

    [SerializeField] private float lowestFontSize;
    [SerializeField] private float highestFontSize;



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

    public IEnumerator TextFadeInAndOutOverTime(TextMeshProUGUI _text)
    {
        bool fullAlpha = false;

        while (true)
        {
            if (fullAlpha == true)
            {
                _text.alpha -= Time.deltaTime / fadeRate;

                if (_text.alpha <= lowestAlpha)
                {
                    fullAlpha = false;
                    yield return null;
                }
                yield return null;
            }

            if (fullAlpha == false)
            {
                _text.alpha += Time.deltaTime / fadeRate;

                if (_text.alpha >= highestAlpha)
                {
                    fullAlpha = true;
                    yield return null;
                }
            }
            yield return null;
        }
    }
    public IEnumerator TextIncreaseAndDecreaseFontSizeOverTime(TextMeshProUGUI _text)
    {
        bool maxFontSize = false;
        _text.alignment = TextAlignmentOptions.Center;

        while(true)
        {
            if (maxFontSize == true)
            {
                _text.fontSize -= ( _text.fontSize * Time.deltaTime ) / fontSizeRate;

                if (_text.fontSize <= lowestFontSize)
                {
                    maxFontSize = false;
                    yield return null;
                }
                yield return null;
            }

            if (maxFontSize == false)
            {
                _text.fontSize += (_text.fontSize * Time.deltaTime) / fontSizeRate;

                if (_text.fontSize >= highestFontSize)
                {
                    maxFontSize = true;
                    yield return null;
                }
            }

            yield return null;
        }
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
