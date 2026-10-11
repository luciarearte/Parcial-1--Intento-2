
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlCajaFinal : MonoBehaviour
{
    public GameObject camaraJugador;
    public GameObject camaraFinal;
    public GameObject munecaFinal;
    public Image fundidoFinal;
    public GameObject textoInteraccion;

    [Header("Audio final")]
    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;
    public float volumenAudio = 0.4f;

    public float duracionFundido = 4f;

    private bool jugadorEnZona = false;
    private bool finalActivado = false;

    private void Start()
    {
        if (camaraFinal != null)
            camaraFinal.SetActive(false);

        if (munecaFinal != null)
            munecaFinal.SetActive(false);

        if (textoInteraccion != null)
            textoInteraccion.SetActive(false);

        if (fundidoFinal != null)
        {
            Color color = fundidoFinal.color;
            color.a = 0f;
            fundidoFinal.color = color;
        }

        if (audioRecuerdo == null)
            audioRecuerdo = GetComponent<AudioSource>();

        if (audioRecuerdo != null)
            audioRecuerdo.playOnAwake = false;
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !finalActivado)
        {
            finalActivado = true;

            if (textoInteraccion != null)
                textoInteraccion.SetActive(false);

            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;

            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = false;

            StartCoroutine(ReproducirAudioFinal());

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

    private IEnumerator ReproducirAudioFinal()
    {
        if (audioRecuerdo == null || audioRecuerdo.clip == null)
        {
            Debug.LogWarning(
                "Falta asignar el AudioSource o el AudioClip del final."
            );
            yield break;
        }

        float inicio = Mathf.Clamp(
            inicioAudio, 0f, audioRecuerdo.clip.length
        );

        float fin = Mathf.Clamp(
            finAudio, inicio, audioRecuerdo.clip.length
        );

        float duracion = fin - inicio;

        if (duracion <= 0f)
            yield break;

        float fadeIn = Mathf.Min(2f, duracion * 0.5f);
        float fadeOut = Mathf.Min(1f, duracion * 0.5f);

        audioRecuerdo.Stop();
        audioRecuerdo.time = inicio;
        audioRecuerdo.volume = 0f;
        audioRecuerdo.Play();

        float tiempo = 0f;

        while (tiempo < fadeIn)
        {
            tiempo += Time.deltaTime;
            audioRecuerdo.volume =
                Mathf.Lerp(0f, volumenAudio, tiempo / fadeIn);

            yield return null;
        }

        audioRecuerdo.volume = volumenAudio;

        float espera = duracion - fadeIn - fadeOut;

        if (espera > 0f)
            yield return new WaitForSeconds(espera);

        tiempo = 0f;

        while (tiempo < fadeOut)
        {
            tiempo += Time.deltaTime;
            audioRecuerdo.volume =
                Mathf.Lerp(volumenAudio, 0f, tiempo / fadeOut);

            yield return null;
        }

        audioRecuerdo.Stop();
        audioRecuerdo.volume = volumenAudio;
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

        Debug.Log("Fin de Ghost of You.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;

            if (textoInteraccion != null && !finalActivado)
                textoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = false;

            if (textoInteraccion != null)
                textoInteraccion.SetActive(false);
        }
    }
}
