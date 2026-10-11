
using UnityEngine;
using TMPro;
using System.Collections;

public class ControlTriggerRadio : MonoBehaviour
{
    public GestorRecuerdos gestor;
    public GameObject munecaBailando;
    public GameObject textoInteraccion;
    public TMP_Text textoFraseFinal;

    public float duracionRecuerdo = 8f;
    public float duracionFrase = 4f;
    public float duracionFadeFrase = 1.5f;

    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;

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

            Color color = textoFraseFinal.color;
            color.a = 0f;
            textoFraseFinal.color = color;

            textoFraseFinal.gameObject.SetActive(false);
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
                inicioAudio, 0f, audioRecuerdo.clip.length
            );

            float fin = Mathf.Clamp(
                finAudio, inicio, audioRecuerdo.clip.length
            );

            duracionAudio = fin - inicio;
        }

        StartCoroutine(ReproducirAudio());

        yield return new WaitForSeconds(
            Mathf.Max(duracionRecuerdo, duracionAudio)
        );

        if (munecaBailando != null)
            munecaBailando.SetActive(false);

        if (textoFraseFinal != null)
            yield return StartCoroutine(MostrarFrase());
    }

    private IEnumerator MostrarFrase()
    {
        textoFraseFinal.text =
            "Esta casa ya no guarda nada más de nosotros";

        textoFraseFinal.gameObject.SetActive(true);

        Color color = textoFraseFinal.color;

        // Fade in: aparece lentamente.
        float tiempo = 0f;

        while (tiempo < duracionFadeFrase)
        {
            tiempo += Time.deltaTime;
            color.a = Mathf.Clamp01(tiempo / duracionFadeFrase);
            textoFraseFinal.color = color;
            yield return null;
        }

        color.a = 1f;
        textoFraseFinal.color = color;

        // Mantener la frase visible.
        yield return new WaitForSeconds(duracionFrase);

        // Fade out: desaparece lentamente.
        tiempo = 0f;

        while (tiempo < duracionFadeFrase)
        {
            tiempo += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(tiempo / duracionFadeFrase);
            textoFraseFinal.color = color;
            yield return null;
        }

        color.a = 0f;
        textoFraseFinal.color = color;

        textoFraseFinal.gameObject.SetActive(false);
    }

    private IEnumerator ReproducirAudio()
    {
        if (audioRecuerdo == null || audioRecuerdo.clip == null)
            yield break;

        float inicio = Mathf.Clamp(
            inicioAudio, 0f, audioRecuerdo.clip.length
        );

        float fin = Mathf.Clamp(
            finAudio, inicio, audioRecuerdo.clip.length
        );

        float duracion = fin - inicio;

        if (duracion <= 0f)
            yield break;

        float volumenObjetivo = 0.4f;
        float duracionFadeIn = Mathf.Min(2f, duracion * 0.5f);
        float duracionFadeOut = Mathf.Min(1f, duracion * 0.5f);

        audioRecuerdo.Stop();
        audioRecuerdo.time = inicio;
        audioRecuerdo.volume = 0f;
        audioRecuerdo.Play();

        float tiempo = 0f;

        while (tiempo < duracionFadeIn)
        {
            tiempo += Time.deltaTime;

            audioRecuerdo.volume = Mathf.Lerp(
                0f,
                volumenObjetivo,
                Mathf.Clamp01(tiempo / duracionFadeIn)
            );

            yield return null;
        }

        audioRecuerdo.volume = volumenObjetivo;

        float espera = duracion - duracionFadeIn - duracionFadeOut;

        if (espera > 0f)
            yield return new WaitForSeconds(espera);

        tiempo = 0f;

        while (tiempo < duracionFadeOut)
        {
            tiempo += Time.deltaTime;

            audioRecuerdo.volume = Mathf.Lerp(
                volumenObjetivo,
                0f,
                Mathf.Clamp01(tiempo / duracionFadeOut)
            );

            yield return null;
        }

        audioRecuerdo.Stop();
        audioRecuerdo.volume = volumenObjetivo;
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
