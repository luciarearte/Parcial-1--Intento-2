
using UnityEngine;

public class ControlTrigger : MonoBehaviour
{
    public GameObject indicadorE;
    public GestorRecuerdos gestor;

    private bool jugadorEnZona = false;

    private void Start()
    {
        if (indicadorE != null)
            indicadorE.SetActive(false);
    }

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            if (gestor == null)
                return;

            if (gestor.CompletarRecuerdo(2))
            {
                Debug.Log("Recuerdo recogido: " + gameObject.name);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;

            if (indicadorE != null)
                indicadorE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower() == "jugador")
        {
            jugadorEnZona = false;

            if (indicadorE != null)
                indicadorE.SetActive(false);
        }
    }
}
