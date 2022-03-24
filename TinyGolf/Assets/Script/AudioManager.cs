using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    [SerializeField] AudioSource sonido, musica;

    private void Awake()
    {
        foreach(Sound s in Sounds)
        {
            if (s.Musica)
            {
                s.Source = musica; 
            }
            else
            {
                s.Source = sonido;
            }
            s.Source.clip = s.Clip;
            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
            s.Source.loop = s.loop;
        }
    }
    public void Start()
    {
        PlayMusica("FondoMenu");
    }
    public void PlaySonido(string name)
    {
       Sound s = Array.Find(Sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        sonido.PlayOneShot(s.Clip);
    }
    public void PlayMusica(string name)
    {
        Debug.Log("Musica");
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        musica.clip = s.Clip;
        musica.Play();
    }
    public void PlaySonidoN(string name,float volumen)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        sonido.PlayOneShot(s.Clip,volumen);
    }
}
