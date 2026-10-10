using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrigger : MonoBehaviour
{
    public GameObject textoCancion;
    public GameObject indicadorE; 
    private bool jugadorEnZona = false;

    private void Start()
    {
        if (textoCancion != null) textoCancion.SetActive(false);
        if (indicadorE != null) indicadorE.SetActive(false);
    } // <--- Faltaba esta llave para cerrar el Start()

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            if (textoCancion != null)
            {
                textoCancion.SetActive(!textoCancion.activeSelf);
                
                if (indicadorE != null)
                {
                    indicadorE.SetActive(!textoCancion.activeSelf);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;
            
            if (indicadorE != null && (textoCancion == null || !textoCancion.activeSelf))
            {
                indicadorE.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            jugadorEnZona = false;
            
            if (textoCancion != null) textoCancion.SetActive(false); 
            if (indicadorE != null) indicadorE.SetActive(false); 
        }
    }
}