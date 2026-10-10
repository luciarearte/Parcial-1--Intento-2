using System.Collections;
using UnityEngine;
using TMPro;

public class ControlIntroCinematica : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject camaraIntro;      
    public GameObject camaraJugador;    

    [Header("Jugador")]
    public GameObject objetoJugador;    

    [Header("Fachadas")]
    public GameObject fachadaNueva;     
    public GameObject fachadaVieja;    

    [Header("Skyboxes")]
    public Material skyboxSoleado;     
    public Material skyboxLluvioso;  
    public Light luzDireccional;   

    [Header("UI")]
    public TMP_Text textoCapitulo;      

    [Header("Tiempos")]
    public float tiempoAntesDeCambio = 3f; 
    public float tiempoDeTransformacion = 5f; 



    void Start()
    {
        if (camaraIntro != null) camaraIntro.SetActive(true);
        if (camaraJugador != null) camaraJugador.SetActive(false);
        if (objetoJugador != null) objetoJugador.SetActive(false);

        if (fachadaNueva != null) fachadaNueva.SetActive(true);
        if (fachadaVieja != null) fachadaVieja.SetActive(false);

        if (textoCapitulo != null) textoCapitulo.gameObject.SetActive(true);

        if (skyboxSoleado != null) 
        {
            RenderSettings.skybox = skyboxSoleado;
        }

        StartCoroutine(SecuenciaIntro());
    }

    IEnumerator SecuenciaIntro()
    {
        yield return new WaitForSeconds(2f);
        
        if (textoCapitulo != null) textoCapitulo.gameObject.SetActive(false);

        yield return new WaitForSeconds(tiempoAntesDeCambio - 2f);

        if (fachadaNueva != null) fachadaNueva.SetActive(false);
        if (fachadaVieja != null) fachadaVieja.SetActive(true);

        if (skyboxLluvioso != null) 
        {
            RenderSettings.skybox = skyboxLluvioso;
            DynamicGI.UpdateEnvironment();
           } 
        {
            if (luzDireccional != null)
            luzDireccional.intensity=0.5f;
            luzDireccional.color = Color.black;    
            }

        yield return new WaitForSeconds(tiempoDeTransformacion - tiempoAntesDeCambio);

        if (camaraIntro != null) camaraIntro.SetActive(false);
        if (objetoJugador != null) objetoJugador.SetActive(true);
        if (camaraJugador != null) camaraJugador.SetActive(true);

        gameObject.SetActive(false);
    }
}