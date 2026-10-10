
using UnityEngine;
using System.Collections;

public class ControlTriggerCuadro : MonoBehaviour
{
    public GestorRecuerdos gestor;
    public GameObject munecaSentada;
    public float duracionRecuerdo = 5f;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (munecaSentada != null)
            munecaSentada.SetActive(false);
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (gestor != null && gestor.CompletarRecuerdo(3))
            {
                recuerdoActivado = true;

                if (munecaSentada != null)
                {
                    munecaSentada.SetActive(true);
                    StartCoroutine(OcultarMuneca());
                }

                Debug.Log("Recuerdo del cuadro activado.");
            }
        }
    }

    private IEnumerator OcultarMuneca()
    {
        yield return new WaitForSeconds(duracionRecuerdo);
        munecaSentada.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = false;
        }
    }
}
