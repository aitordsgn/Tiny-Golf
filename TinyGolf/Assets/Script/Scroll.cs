using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scroll : MonoBehaviour
{
    public GameObject scrollbar,BotonDcha,BotonIzda, Juego, JuegoUI,Principal,BotonAtras;
    public float scroll_pos = 0;
    public float[] pos;
    public int Posicion;
    [SerializeField] Scr_Pelota pelota;
    [SerializeField] TextMeshProUGUI RecordContra, RecordSurv;
    [SerializeField] PlayFabManager playFabManager;
    [SerializeField] GameObject RankingObj;
    // Update is called once per frame
    private void Start()
    {
        RecordContra.text = PlayerPrefs.GetInt("RecordContra").ToString();
        RecordSurv.text = PlayerPrefs.GetInt("RecordSurvival").ToString();
    }
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
        if(Principal.active)
        {
            RecordContra.text = PlayerPrefs.GetInt("RecordContra").ToString();
            RecordSurv.text = PlayerPrefs.GetInt("RecordSurvival").ToString();
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
        BotonAtras.SetActive(true);
        Juego.SetActive(true);
        JuegoUI.SetActive(true);
        Principal.SetActive(false);

    }
    public void Ranking()
    {
        playFabManager.SendLeaderboard(PlayerPrefs.GetInt("RecordContra"));
        RankingObj.SetActive(true);
    }
}