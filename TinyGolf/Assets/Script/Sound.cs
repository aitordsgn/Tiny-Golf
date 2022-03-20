using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] // Si no escribes esto no aparece en el editor
public class Sound
{
    public string name;
    public AudioClip Clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
    public bool Musica;
    [HideInInspector]//Asi no aparece en  el inspector
    public AudioSource Source;
    public bool loop;
}
