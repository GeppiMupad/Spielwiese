using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject spawner;

    public void OnClick()
    {
       GameObject temp = Instantiate(objectToSpawn);
       temp.transform.position = spawner.transform.position;

    }
}
