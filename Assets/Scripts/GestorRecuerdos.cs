
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GestorRecuerdos : MonoBehaviour
{
    public int recuerdosEncontrados = 0;
    public Text textoContador;
    public Text textoAviso;
    public float duracionAviso = 2.5f;

    private Coroutine corrutinaAviso;

    private void Start()
    {
        ActualizarContador();

        if (textoAviso != null)
            textoAviso.gameObject.SetActive(false);
    }

    public bool PuedeActivarRecuerdo(int numero)
    {
        return numero == recuerdosEncontrados + 1;
    }

    public bool CompletarRecuerdo(int numero)
    {
        if (!PuedeActivarRecuerdo(numero))
        {
            MostrarAviso("Todavía no es el momento de este recuerdo.");
            Debug.Log("Todavía no es el momento de este recuerdo.");
            return false;
        }

        recuerdosEncontrados++;

        ActualizarContador();

        Debug.Log("Recuerdos encontrados: " + recuerdosEncontrados + "/5");

        return true;
    }

    private void ActualizarContador()
    {
        if (textoContador != null)
        {
            textoContador.text =
                "RECUERDO " + recuerdosEncontrados + "/5";
        }
    }

    private void MostrarAviso(string mensaje)
    {
        if (textoAviso == null)
            return;

        textoAviso.text = mensaje;
        textoAviso.gameObject.SetActive(true);

        if (corrutinaAviso != null)
            StopCoroutine(corrutinaAviso);

        corrutinaAviso = StartCoroutine(OcultarAviso());
    }

    private IEnumerator OcultarAviso()
    {
        yield return new WaitForSeconds(duracionAviso);

        if (textoAviso != null)
            textoAviso.gameObject.SetActive(false);

        corrutinaAviso = null;
    }
}
