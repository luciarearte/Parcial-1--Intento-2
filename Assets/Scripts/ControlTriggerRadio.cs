
using UnityEngine;
using System.Collections;

public class ControlTriggerRadio : MonoBehaviour
{
    public GestorRecuerdos gestor;
    public GameObject munecaBailando;
    public GameObject textoInteraccion;
    public float duracionRecuerdo = 8f;

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
                {
                    munecaBailando.SetActive(true);
                    StartCoroutine(OcultarMuneca());
                }

                Debug.Log("Recuerdo de la radio activado.");

                StartCoroutine(ReproducirAudio());
            }
        }
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

    private IEnumerator OcultarMuneca()
    {
        yield return new WaitForSeconds(duracionRecuerdo);

        if (munecaBailando != null)
            munecaBailando.SetActive(false);
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
