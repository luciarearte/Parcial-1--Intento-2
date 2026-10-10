
using UnityEngine;

public class GestorRecuerdos : MonoBehaviour
{
    public int recuerdosEncontrados = 0;

    public bool PuedeActivarRecuerdo(int numero)
    {
        return numero == recuerdosEncontrados + 1;
    }

    public bool CompletarRecuerdo(int numero)
    {
        if (!PuedeActivarRecuerdo(numero))
        {
            Debug.Log("Todavía no es el momento de este recuerdo.");
            return false;
        }

        recuerdosEncontrados++;

        Debug.Log("Recuerdos encontrados: " + recuerdosEncontrados + "/5");

        return true;
    }
}

