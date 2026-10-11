
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ControlCajaFinal : MonoBehaviour
{
    public GameObject camaraJugador;
    public GameObject camaraFinal;
    public GameObject munecaFinal;
    public Image fundidoFinal;
    public GameObject textoInteraccion;

    [Header("Pausa antes de mostrar el final")]
    public float esperaAntesDeCambiarCamara = 2f;

    [Header("Audio final")]
    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;
    public float volumenAudio = 0.4f;

    [Header("Primera letra")]
    public TMP_Text textoLetra;
    [TextArea(2, 4)]
    public string letraRecuerdo = "Primera letra";
    public float duracionFadeLetra = 0.7f;

    [Header("Segunda letra para la pantalla negra")]
    public TMP_Text textoLetraFinal;
    [TextArea(2, 5)]
    public string letraFinal = "Escribí acá la segunda letra";

    [Header("Fundido a negro")]
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

        if (textoLetra != null)
        {
            textoLetra.text = letraRecuerdo;
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(false);
        }

        if (textoLetraFinal != null)
        {
            textoLetraFinal.text = letraFinal;
            textoLetraFinal.alpha = 0f;
            textoLetraFinal.gameObject.SetActive(false);
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
            StartCoroutine(SecuenciaFinal());
        }
    }

    private IEnumerator SecuenciaFinal()
    {
        // Espera antes de mostrar la muñeca.
        yield return new WaitForSeconds(
            esperaAntesDeCambiarCamara
        );

        if (camaraJugador != null)
            camaraJugador.SetActive(false);

        if (camaraFinal != null)
            camaraFinal.SetActive(true);

        if (munecaFinal != null)
            munecaFinal.SetActive(true);

        // Fundido a negro.
        if (fundidoFinal != null)
            yield return StartCoroutine(FundirPantalla());

        // Al quedar todo negro, mostrar la segunda letra.
        if (textoLetra != null)
            textoLetra.gameObject.SetActive(false);

        if (textoLetraFinal != null)
        {
            textoLetraFinal.text = letraFinal;
            textoLetraFinal.gameObject.SetActive(true);
            textoLetraFinal.transform.SetAsLastSibling();
            textoLetraFinal.alpha = 1f;
        }

        Debug.Log("Fin de Ghost of You.");
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

        float fade = Mathf.Min(
            duracionFadeLetra, duracion / 2f
        );

        audioRecuerdo.Stop();
        audioRecuerdo.time = inicio;
        audioRecuerdo.volume = 0f;
        audioRecuerdo.Play();

        if (textoLetra != null)
        {
            textoLetra.text = letraRecuerdo;
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(true);
        }

        float tiempo = 0f;

        // Fade in del audio y la primera letra.
        while (tiempo < fade)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / fade);

            audioRecuerdo.volume =
                Mathf.Lerp(0f, volumenAudio, progreso);

            if (textoLetra != null)
                textoLetra.alpha = progreso;

            yield return null;
        }

        audioRecuerdo.volume = volumenAudio;

        if (textoLetra != null)
            textoLetra.alpha = 1f;

        float espera = duracion - fade * 2f;

        if (espera > 0f)
            yield return new WaitForSeconds(espera);

        // Fade out del audio y la primera letra.
        tiempo = 0f;

        while (tiempo < fade)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / fade);

            audioRecuerdo.volume =
                Mathf.Lerp(volumenAudio, 0f, progreso);

            if (textoLetra != null)
                textoLetra.alpha = 1f - progreso;

            yield return null;
        }

        audioRecuerdo.Stop();
        audioRecuerdo.volume = volumenAudio;

        if (textoLetra != null)
        {
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(false);
        }
    }

    private IEnumerator FundirPantalla()
    {
        float tiempo = 0f;

        while (tiempo < duracionFundido)
        {
            tiempo += Time.deltaTime;

            Color color = fundidoFinal.color;
            color.a = Mathf.Clamp01(
                tiempo / duracionFundido
            );
            fundidoFinal.color = color;

            yield return null;
        }

        Color final = fundidoFinal.color;
        final.a = 1f;
        fundidoFinal.color = final;
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
