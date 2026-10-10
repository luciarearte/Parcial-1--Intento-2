using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    public float rapidezDesplazamiento = 5.0f; // Bajalo a 5 o 10, la física es más rápida
    private Rigidbody rb;

    void Start()
    {
        Application.targetFrameRate = 60; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>(); // Conecta el script con tu Rigidbody
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // FixedUpdate es el Update exclusivo para las físicas (Rigidbody)
    void FixedUpdate() 
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 movimiento = (transform.right * movX + transform.forward * movZ).normalized;
        
        // Empuja el cuerpo físico (¡ahora sí las paredes te van a frenar!)
        rb.velocity = new Vector3(movimiento.x * rapidezDesplazamiento, rb.velocity.y, movimiento.z * rapidezDesplazamiento);
    }
}