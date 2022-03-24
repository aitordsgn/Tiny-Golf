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
    [SerializeField] GameObject Nmusica, Juego, JuegoUI, Victoria, Menu, BotonAtras,Ranking, NewHigScore , InputName;
    [SerializeField] Scr_Pelota pelota;
    [SerializeField] Image TimeTrialLeaderboard, SurvivalLeaderboard;
    [SerializeField] Color MarronClaro, MarronOscuro;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Sonido")==1)
        {
            Sonido.mute= true;
            img.sprite = Mute;

        }
        if (PlayerPrefs.GetInt("Musica") == 1)
        {
            Musica.mute = true;
            Nmusica.SetActive(true);
        }
    }
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
            PlayerPrefs.SetInt("Sonido", 1);

        }
        else
        {
            img.sprite = NMute;
            PlayerPrefs.SetInt("Sonido", 0);

        }
    }
    public void MusicaV()
    {
        Musica.mute = !Musica.mute;
        Nmusica.SetActive(!Nmusica.active);
        if (Musica.mute)
        {
            PlayerPrefs.SetInt("Musica", 1);
            Debug.Log("MusicaMute PlayerPref");
        }
        else
        {
            PlayerPrefs.SetInt("Musica", 0);

        }
    }
    public void Atras()
    {
        Juego.SetActive(false);
        JuegoUI.SetActive(false);
        Victoria.SetActive(false);
        Menu.SetActive(true);
        Ranking.SetActive(true);
        BotonAtras.SetActive(false);
        NewHigScore.SetActive(false);
        pelota.SetRecord();
        pelota.tryAgain();
    }

    public void CambioColorTimeTrial()
    {
        
        TimeTrialLeaderboard.color = MarronOscuro;
        SurvivalLeaderboard.color = MarronClaro;
        TimeTrialLeaderboard.GetComponent<Button>().interactable = false;
        SurvivalLeaderboard.GetComponent<Button>().interactable = true;
    }
    public void CambioColorSurv()
    {
        SurvivalLeaderboard.color = MarronOscuro;
        TimeTrialLeaderboard.color = MarronClaro;
        SurvivalLeaderboard.GetComponent<Button>().interactable = false;
        TimeTrialLeaderboard.GetComponent<Button>().interactable = true;


    }
    public void CerrarNombre()
    {
        InputName.SetActive(false);
    }
}
