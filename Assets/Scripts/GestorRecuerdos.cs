
using UnityEngine;

public class GestorRecuerdos : MonoBehaviour
{
    public int recuerdosEncontrados = 0;

    public void CompletarRecuerdo()
    {
        recuerdosEncontrados++;

        Debug.Log("Recuerdos encontrados: " + recuerdosEncontrados + "/5");
    }
}
