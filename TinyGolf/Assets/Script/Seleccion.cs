using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccion : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    public Vector3 PosicionFinal, PosicionInicial;
    [SerializeField] float DistanciaMaxima;
    [SerializeField] Vector3[] Posiciones;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PosicionInicial = Input.mousePosition; //Guardamos la posicion inicial cuando el dedo toca la pantalla
        }

        if(Input.GetMouseButton(0))
        {
            PosicionFinal = Input.mousePosition;
            Vector3 diferenecia = new Vector3(PosicionFinal.x - PosicionInicial.x, 0, 0);
            Menu.transform.position = Vector3.MoveTowards(Menu.transform.position, Menu.transform.position - diferenecia, DistanciaMaxima);
            
        }
        
    }
}
