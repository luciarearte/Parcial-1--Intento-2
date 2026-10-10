
using UnityEngine;
using System.Collections;

public class ControlTriggerRadio : MonoBehaviour
{
    public GestorRecuerdos gestor;
    public GameObject munecaBailando;
    public float duracionRecuerdo = 8f;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (munecaBailando != null)
            munecaBailando.SetActive(false);
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (gestor != null && gestor.CompletarRecuerdo(4))
            {
                recuerdoActivado = true;

                if (munecaBailando != null)
                {
                    munecaBailando.SetActive(true);
                    StartCoroutine(OcultarMuneca());
                }

                Debug.Log("Recuerdo de la radio activado.");
            }
        }
    }

    private IEnumerator OcultarMuneca()
    {
        yield return new WaitForSeconds(duracionRecuerdo);
        munecaBailando.SetActive(false);
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
