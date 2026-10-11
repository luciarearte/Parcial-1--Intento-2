
using UnityEngine;
using System.Collections;

public class ControlTriggerCama : MonoBehaviour
{
    public GameObject muneca;
    public float duracionAnimacion = 5f;
    public GestorRecuerdos gestor;

    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (muneca != null)
            muneca.SetActive(false);

        if (audioRecuerdo == null)
            audioRecuerdo = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (muneca != null && gestor != null)
            {
                if (gestor.CompletarRecuerdo(1))
                {
                    recuerdoActivado = true;
                    muneca.SetActive(true);

                    if (audioRecuerdo != null)
                    {
                        audioRecuerdo.Stop();
                        audioRecuerdo.time = inicioAudio;
                        audioRecuerdo.volume = 0f;
                        audioRecuerdo.Play();

                        StartCoroutine(FadeInAudio());
                        StartCoroutine(DetenerAudio());
                    }

                    StartCoroutine(OcultarMuneca());
                }
            }
        }
    }

    private IEnumerator FadeInAudio()
    {
        float duracionFadeIn = 2f;
        float tiempo = 0f;

        while (tiempo < duracionFadeIn)
        {
            tiempo += Time.deltaTime;

            if (audioRecuerdo == null)
                yield break;

            audioRecuerdo.volume = Mathf.Lerp(
                0f,
                0.4f,
                tiempo / duracionFadeIn
            );

            yield return null;
        }

        if (audioRecuerdo != null)
            audioRecuerdo.volume = 0.4f;
    }

    private IEnumerator DetenerAudio()
    {
        float duracionFadeOut = 1f;
        float tiempoTotal = finAudio - inicioAudio;

        yield return new WaitForSeconds(
            Mathf.Max(0f, tiempoTotal - duracionFadeOut)
        );

        if (audioRecuerdo == null)
            yield break;

        float volumenOriginal = audioRecuerdo.volume;
        float tiempo = 0f;

        while (tiempo < duracionFadeOut)
        {
            tiempo += Time.deltaTime;

            audioRecuerdo.volume = Mathf.Lerp(
                volumenOriginal,
                0f,
                tiempo / duracionFadeOut
            );

            yield return null;
        }

        audioRecuerdo.Stop();
        audioRecuerdo.volume = 1f;
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
