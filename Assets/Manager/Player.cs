using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GetManagerComponents managerComponents;

    private void Awake()
    {
        managerComponents = FindAnyObjectByType<GetManagerComponents>();
    }
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            managerComponents.eventManager.InvokeFirstCanvas();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            managerComponents.eventManager.InvokeSecondCanvas();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            managerComponents.eventManager.InvokeThirdCanvas();
        }
    }
}
