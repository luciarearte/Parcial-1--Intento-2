
using UnityEngine;

public class ControlTriggerCuadro : MonoBehaviour
{
    public GestorRecuerdos gestor;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (gestor != null && gestor.CompletarRecuerdo(3))
            {
                recuerdoActivado = true;
                Debug.Log("Recuerdo del cuadro activado.");
            }
        }
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
