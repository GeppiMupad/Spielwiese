using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour
{

    [SerializeField] private GameObject holdingPosition;
    private bool canBePicked = false; // if player is in Range, pickup Placeable



    private GetInput myInput;

    private void Awake()
    {
        myInput = GetComponent<GetInput>();
    }

    private void Update()
    {
        if (canBePicked == true ) // if player is in Range & pressed E 
        {
            PickUpPlaceable();
        }
    }


    private void PickUpPlaceable()
    {
       // this.gameObject.transform.position = holdingPosition.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enviroment") || collision.gameObject.CompareTag("Placeable"))
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("is Triggert");
        if (other.gameObject.CompareTag("Player"))
        {
            canBePicked = true;
        }
    }
}
