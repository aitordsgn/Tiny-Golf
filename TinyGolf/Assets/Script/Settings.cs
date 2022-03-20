using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource Sonido, Musica;
    [SerializeField] Image img;
    [SerializeField] Sprite Mute, NMute;
    [SerializeField] GameObject Nmusica, Juego, JuegoUI, Victoria, Menu, BotonAtras;
    [SerializeField] Scr_Pelota pelota;
    [SerializeField]
   
    public void boton()
    {
        audioManager.PlaySonido("Boton");
    }
    public void SonidoV()
    {
        Sonido.mute = !Sonido.mute;
        if(Sonido.mute)
        {
            img.sprite = Mute;

        }
        else
        {
            img.sprite = NMute;

        }
    }
    public void MusicaV()
    {
        Musica.mute = !Musica.mute;
        Nmusica.SetActive(!Nmusica.active);
    }
    public void Atras()
    {
        Juego.SetActive(false);
        JuegoUI.SetActive(false);
        Victoria.SetActive(false);
        Menu.SetActive(true);
        BotonAtras.SetActive(false);
        pelota.SetRecord();
        pelota.tryAgain();
    }
}
