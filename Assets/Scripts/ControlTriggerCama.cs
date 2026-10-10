
using UnityEngine;
using System.Collections;

public class ControlTriggerCama : MonoBehaviour
{
    public GameObject muneca;
    public float duracionAnimacion = 5f;
    public GestorRecuerdos gestor;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (muneca != null)
            muneca.SetActive(false);
    }

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E) && !recuerdoActivado)
        {
            if (muneca != null && gestor != null)
            {
                if (gestor.CompletarRecuerdo(1))
                {
                    recuerdoActivado = true;
                    muneca.SetActive(true);
                    StartCoroutine(OcultarMuneca());
                }
            }
        }
    }

    private IEnumerator OcultarMuneca()
    {
        yield return new WaitForSeconds(duracionAnimacion);

        muneca.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
            jugadorEnZona = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
            jugadorEnZona = false;
    }
}
