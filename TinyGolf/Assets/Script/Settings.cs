using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource Sonido, Musica;
    

    public void boton()
    {
        audioManager.PlaySonido("Boton");
    }
    public void SonidoV()
    {
        Sonido.mute = !Sonido.mute;
    }
    public void MusicaV()
    {
        Musica.mute = !Musica.mute;
    }
}
