using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public GameObject scrollbar,BotonDcha,BotonIzda, Juego, JuegoUI,Principal;
    public float scroll_pos = 0;
    public float[] pos;
    public int Posicion;
    [SerializeField] Scr_Pelota pelota;
    // Update is called once per frame
    void Update()
    {
        if(Posicion==0)
        {
            BotonIzda.SetActive(false);
            BotonDcha.SetActive(true);

        }
        if (Posicion==1)
        {
            BotonDcha.SetActive(false);
            BotonIzda.SetActive(true);

        }
        
    pos = new float[transform.childCount];
    float distance = 1f / (pos.Length - 1f);
    for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
            if (Input.GetMouseButton(0) )
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int j = 0; j < pos.Length; j++)
                {
                    if (scroll_pos < pos[j] + (distance / 2) && scroll_pos > pos[j] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[j], 0.1f);

                        Posicion = j;
                    }
                }
            }
        }
    }
    public void Play()
    {
        pelota.CambiarPosicion(Posicion);
        Juego.SetActive(true);
        JuegoUI.SetActive(true);
        Principal.SetActive(false);
    }
}