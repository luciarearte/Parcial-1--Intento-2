
using UnityEngine;
using System.Collections;

public class ControlTrigger : MonoBehaviour
{
    public GameObject indicadorE;
    public GestorRecuerdos gestor;

    public AudioSource audioRecuerdo;
    public float inicioAudio = 0f;
    public float finAudio = 4f;

    private bool jugadorEnZona = false;
    private bool recuerdoActivado = false;

    private void Start()
    {
        if (indicadorE != null)
            indicadorE.SetActive(false);

        if (audioRecuerdo == null)
            audioRecuerdo = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (jugadorEnZona &&
            Input.GetKeyDown(KeyCode.E) &&
            !recuerdoActivado)
        {
            if (gestor == null)
                return;

            if (gestor.CompletarRecuerdo(2))
            {
                recuerdoActivado = true;

                Debug.Log("Recuerdo recogido: " + gameObject.name);

                if (indicadorE != null)
                    indicadorE.SetActive(false);

                // Ocultar el vaso inmediatamente.
                Renderer[] renderers =
                    GetComponentsInChildren<Renderer>();

                foreach (Renderer r in renderers)
                    r.enabled = false;

                Collider[] colliders =
                    GetComponentsInChildren<Collider>();

                foreach (Collider c in colliders)
                    c.enabled = false;

                // Reproducir el audio antes de eliminar el objeto.
                StartCoroutine(ReproducirAudioYEliminar());
            }
        }
    }

    private IEnumerator ReproducirAudioYEliminar()
    {
        if (audioRecuerdo == null || audioRecuerdo.clip == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float duracion = finAudio - inicioAudio;

        if (duracion <= 0f)
        {
            Destroy(gameObject);
            yield break;
        }

        float volumenObjetivo = 0.4f;
        float duracionFadeIn = Mathf.Min(2f, duracion * 0.5f);
        float duracionFadeOut = Mathf.Min(1f, duracion * 0.5f);

        audioRecuerdo.Stop();
        audioRecuerdo.time = inicioAudio;
        audioRecuerdo.volume = 0f;
        audioRecuerdo.Play();

        // Fade in: entrada gradual durante 2 segundos.
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

        // Esperar hasta el momento del fade out.
        float espera = duracion - duracionFadeIn - duracionFadeOut;

        if (espera > 0f)
            yield return new WaitForSeconds(espera);

        // Fade out: salida gradual durante el último segundo.
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

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = true;

            if (indicadorE != null && !recuerdoActivado)
                indicadorE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.name.ToLower() == "jugador")
        {
            jugadorEnZona = false;

            if (indicadorE != null)
                indicadorE.SetActive(false);
        }
    }
}
