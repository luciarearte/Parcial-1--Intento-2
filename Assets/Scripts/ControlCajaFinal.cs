
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlCajaFinal : MonoBehaviour
{
    public GameObject camaraJugador;
    public GameObject camaraFinal;
    public GameObject munecaFinal;
    public Image fundidoFinal;

    public float duracionFundido = 4f;

    private bool jugadorEnZona = false;
    private bool finalActivado = false;

    private void Start()
    {
        if (camaraFinal != null)
            camaraFinal.SetActive(false);

        if (munecaFinal != null)
            munecaFinal.SetActive(false);

        if (fundidoFinal != null)
        {
            Color color = fundidoFinal.color;
            color.a = 0f;
            fundidoFinal.color = color;
        }
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !finalActivado)
        {
            finalActivado = true;

            // Ocultar la caja sin desactivar este script.
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;

            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = false;

            if (camaraJugador != null)
                camaraJugador.SetActive(false);

            if (camaraFinal != null)
                camaraFinal.SetActive(true);

            if (munecaFinal != null)
                munecaFinal.SetActive(true);

            if (fundidoFinal != null)
                StartCoroutine(FundirPantalla());
        }
    }

    private IEnumerator FundirPantalla()
    {
        float tiempo = 0f;

        while (tiempo < duracionFundido)
        {
            tiempo += Time.deltaTime;

            Color color = fundidoFinal.color;
            color.a = Mathf.Clamp01(tiempo / duracionFundido);
            fundidoFinal.color = color;

            yield return null;
        }

        Color final = fundidoFinal.color;
        final.a = 1f;
        fundidoFinal.color = final;

        Debug.Log("Fin de The Ghost of You.");
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
