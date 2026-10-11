
using UnityEngine;
using TMPro;
using System.Collections;

public class ControlTriggerCama : MonoBehaviour
{
    public GameObject muneca;
    public float duracionAnimacion = 5f;
    public GestorRecuerdos gestor;

    [Header("Audio")]
    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;
    public float volumenAudio = 0.4f;

    [Header("Letra de la canción")]
    public TMP_Text textoLetra;
    [TextArea(2, 4)]
    public string letraRecuerdo = "Escribí acá la letra";
    public float duracionFade = 0.7f;

    public GameObject textoInteraccion;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (muneca != null)
            muneca.SetActive(false);

        if (audioRecuerdo == null)
            audioRecuerdo = GetComponent<AudioSource>();

        if (textoInteraccion != null)
            textoInteraccion.SetActive(false);

        if (textoLetra != null)
        {
            textoLetra.text = letraRecuerdo;
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (gestor != null && gestor.CompletarRecuerdo(1))
            {
                recuerdoActivado = true;

                if (textoInteraccion != null)
                    textoInteraccion.SetActive(false);

                if (muneca != null)
                    muneca.SetActive(true);

                StartCoroutine(ReproducirRecuerdo());
                StartCoroutine(OcultarMuneca());
            }
        }
    }

    private IEnumerator ReproducirRecuerdo()
    {
        float duracion = 0f;

        if (audioRecuerdo != null && audioRecuerdo.clip != null)
        {
            float inicio = Mathf.Clamp(
                inicioAudio, 0f, audioRecuerdo.clip.length);

            float fin = Mathf.Clamp(
                finAudio, inicio, audioRecuerdo.clip.length);

            duracion = fin - inicio;

            if (duracion > 0f)
            {
                audioRecuerdo.Stop();
                audioRecuerdo.time = inicio;
                audioRecuerdo.volume = 0f;
                audioRecuerdo.Play();
            }
        }

        if (textoLetra != null)
        {
            textoLetra.text = letraRecuerdo;
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(true);
        }

        if (duracion <= 0f)
        {
            if (textoLetra != null)
            {
                textoLetra.alpha = 1f;
                yield return new WaitForSeconds(duracionAnimacion);
                textoLetra.gameObject.SetActive(false);
            }

            yield break;
        }

        float fade = Mathf.Min(duracionFade, duracion / 2f);
        float tiempo = 0f;

        // Fade in
        while (tiempo < fade)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / fade);

            if (audioRecuerdo != null)
                audioRecuerdo.volume = Mathf.Lerp(0f, volumenAudio, progreso);

            if (textoLetra != null)
                textoLetra.alpha = progreso;

            yield return null;
        }

        if (audioRecuerdo != null)
            audioRecuerdo.volume = volumenAudio;

        if (textoLetra != null)
            textoLetra.alpha = 1f;

        // Mantener la letra visible
        float espera = duracion - fade * 2f;
        if (espera > 0f)
            yield return new WaitForSeconds(espera);

        // Fade out
        tiempo = 0f;

        while (tiempo < fade)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / fade);

            if (audioRecuerdo != null)
                audioRecuerdo.volume = Mathf.Lerp(
                    volumenAudio, 0f, progreso);

            if (textoLetra != null)
                textoLetra.alpha = 1f - progreso;

            yield return null;
        }

        if (audioRecuerdo != null)
        {
            audioRecuerdo.Stop();
            audioRecuerdo.volume = volumenAudio;
        }

        if (textoLetra != null)
        {
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(false);
        }
    }

    private IEnumerator OcultarMuneca()
    {
        yield return new WaitForSeconds(duracionAnimacion);

        if (muneca != null)
            muneca.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;

            if (!recuerdoActivado && textoInteraccion != null)
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
