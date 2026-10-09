using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrigger : MonoBehaviour
{
    public GameObject textoCancion;

    private void Start()
    {
        if (textoCancion != null)
        {
            textoCancion.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            if (textoCancion != null)
            {
                textoCancion.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            if (textoCancion != null)
            {
                textoCancion.SetActive(false); 
            }
        }
    }
}
