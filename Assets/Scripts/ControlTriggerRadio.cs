
using UnityEngine;
using TMPro;
using System.Collections;

public class ControlTriggerRadio : MonoBehaviour
{
    public GestorRecuerdos gestor;
    public GameObject munecaBailando;
    public GameObject textoInteraccion;
    public TMP_Text textoFraseFinal;

    [Header("Duraciones")]
    public float duracionRecuerdo = 8f;
    public float duracionFrase = 4f;
    public float duracionFadeFrase = 1.5f;

    [Header("Audio")]
    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;
    public float volumenAudio = 0.4f;

    [Header("Letra de la canción")]
    public TMP_Text textoLetra;
    [TextArea(2, 4)]
    public string letraRecuerdo = "Escribí acá la letra";
    public float duracionFadeLetra = 0.7f;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (munecaBailando != null)
            munecaBailando.SetActive(false);

        if (textoInteraccion != null)
            textoInteraccion.SetActive(false);

        if (textoFraseFinal != null)
        {
            textoFraseFinal.text =
                "Esta casa ya no guarda nada más de nosotros";

            textoFraseFinal.alpha = 0f;
            textoFraseFinal.gameObject.SetActive(false);
        }

        if (textoLetra != null)
        {
            textoLetra.text = letraRecuerdo;
            textoLetra.alpha = 0f;
            textoLetra.gameObject.SetActive(false);
        }

        if (audioRecuerdo == null)
            audioRecuerdo = GetComponent<AudioSource>();
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

                if (textoInteraccion != null)
                    textoInteraccion.SetActive(false);

                if (munecaBailando != null)
                    munecaBailando.SetActive(true);

                Debug.Log("Recuerdo de la radio activado.");

                StartCoroutine(SecuenciaRecuerdo());
            }
        }
    }

    private IEnumerator SecuenciaRecuerdo()
    {
        float duracionAudio = 0f;

        if (audioRecuerdo != null && audioRecuerdo.clip != null)
        {
            float inicio = Mathf.Clamp(
                inicioAudio, 0f, audioRecuerdo.clip.length);

            float fin = Mathf.Clamp(
                finAudio, inicio, audioRecuerdo.clip.length);

            duracionAudio = fin - inicio;
        }

        yield return StartCoroutine(ReproducirAudio());

        // Mantener la muñeca visible durante el tiempo previsto.
        if (duracionRecuerdo > duracionAudio)
            yield return new WaitForSeconds(
                duracionRecuerdo - duracionAudio);

        if (munecaBailando != null)
            munecaBailando.SetActive(false);

        if (textoFraseFinal != null)
            yield return StartCoroutine(MostrarFrase());
    }

    private IEnumerator ReproducirAudio()
    {
        float duracion = 0f;

        if (audioRecuerdo != null && audioRecuerdo.clip != null)
        {
            float inicio = Mathf.Clamp(
                inicioAudio, 0f, audioRecuerdo.clip.length);

            float fin = Mathf.Clamp(
                finAudio, inicio, audioRecuerdo.clip.length);

            duracion = fin - inicio;
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
                textoLetra.gameObject.SetActive(false);

            yield break;
        }

        float fade = Mathf.Min(duracionFadeLetra, duracion / 2f);

        audioRecuerdo.Stop();
        audioRecuerdo.time = inicioAudio;
        audioRecuerdo.volume = 0f;
        audioRecuerdo.Play();

        float tiempo = 0f;

        // Fade in de la letra y el audio.
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

        // Fade out de la letra y el audio.
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

    private IEnumerator MostrarFrase()
    {
        textoFraseFinal.text =
            "Esta casa ya no guarda nada más de nosotros";

        textoFraseFinal.alpha = 0f;
        textoFraseFinal.gameObject.SetActive(true);

        float tiempo = 0f;

        while (tiempo < duracionFadeFrase)
        {
            tiempo += Time.deltaTime;
            textoFraseFinal.alpha =
                Mathf.Clamp01(tiempo / duracionFadeFrase);

            yield return null;
        }

        textoFraseFinal.alpha = 1f;

        yield return new WaitForSeconds(duracionFrase);

        tiempo = 0f;

        while (tiempo < duracionFadeFrase)
        {
            tiempo += Time.deltaTime;
            textoFraseFinal.alpha =
                1f - Mathf.Clamp01(tiempo / duracionFadeFrase);

            yield return null;
        }

        textoFraseFinal.alpha = 0f;
        textoFraseFinal.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;

            if (textoInteraccion != null && !recuerdoActivado)
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
