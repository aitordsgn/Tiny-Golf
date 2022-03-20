using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    /// <summary>
    /// Script que controla el movimiento de las plataformas
    /// </summary>
    public float Velocidad; // Velocidad de movimiento de la plataforma
    public int PuntoComienzo; // El index del punto de comienzo dentro del array
    public Transform[] puntos; // Array de las posiciones de los puntos


    private int i; //Index utilizado
    // Start is called before the first frame update
    void Start()
    {
        transform.position = puntos[PuntoComienzo].position; //Al comenzar elegimos el punto de comienzo
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, puntos[i].position) < 0.02f)
        {
            i++; //Subimos el index
            if(i == puntos.Length)
            {
                i = 0; //Reseteamos index
            }

        }
        transform.position = Vector2.MoveTowards(transform.position, puntos[i].position, Velocidad * Time.deltaTime); //Mover hacia la posicion de el punto en i

    }


}
