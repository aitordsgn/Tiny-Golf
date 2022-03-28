using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_Pelota : MonoBehaviour
{
    public bool aiming, ready, EnAgujero, Enhollo,Pausa;
    public Vector3 PosicionFinal, PosicionInicial, PosicionFinalBola;
    public float DistanciaMaxima, Velocidad, VelocidadMaxima;
    public int Puntuacion, AgujeroNumero, HoleinOne;
    public GameObject Linea, AgujeroCerca ,Ancla;
    public ParticleSystem Polvo;
    public TextMeshProUGUI Agujero, Tiros;
    public AudioManager audioManager;
    public string Ag="0";
    [SerializeField] GameObject Glow;


    [Header("Survival")]
    private int NumeroAleatorioSuma; // Numero que se suma
    [SerializeField] int StartingPuntuacion; // Numero que se suma
    public TextMeshProUGUI Suma; // Texto que enseña la suma

    [Header("Contrareloj")]
    [SerializeField] float CurrentTime = 0f;
    [SerializeField] float StartingTime = 30f;
    [SerializeField] float AnuncioTime = 20f;
    [SerializeField] int posicion;

    [Header("UI")]
    [SerializeField] GameObject UIVictoria, NewHigScore;
    [SerializeField] TextMeshProUGUI UIAnuncio,UIPuntos;
    [SerializeField] GameObject SpawnPointsObj;
    [SerializeField] List<Vector2> SpanwPointList = new List<Vector2>();
    [SerializeField] int NumeroDeAgujeroEnLista,NumeroAnterior;
    [SerializeField] int RecordSurvival, RecordContra;
    [SerializeField] TextMeshProUGUI RecordContratext, RecordSurvivaltext, TextoAyuda;
    [SerializeField] PlayFabManager playFabManager;
    [SerializeField] string[] Frases;
    [SerializeField] bool enseñado, anunciado;
    [SerializeField] GameObject UIAnuncioObj, UITryAgain, UITryAgainGrande;
    [SerializeField] Settings Settings;

    [Header ("Camara")]
    [SerializeField] float ZoomMaximo;
    [SerializeField] float ZoomMinimo;
    [SerializeField] float TiempoEntreToques;
    [SerializeField] float TiempoMaximo;
    [SerializeField] float Incremento;
    [SerializeField] bool Tirado;
    
    private void Start()
    {
        audioManager.PlayMusica("Fondo2");

        CurrentTime = StartingTime;
        Puntuacion = StartingPuntuacion;
        for(int c=0;c<SpawnPointsObj.transform.childCount;c++)
        {
            SpanwPointList.Add(SpawnPointsObj.transform.GetChild(c).transform.position);
        }
        Destroy(SpawnPointsObj);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount ==2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevmagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevmagnitude;

            Zoom(difference * 0.01f);
            TiempoEntreToques = 0;
            Linea.GetComponent<LineRenderer>().enabled = false;

        }

        else
        {
            if(Camera.main.orthographicSize >=6)
            {
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Time.deltaTime * Incremento, ZoomMinimo, ZoomMaximo);
            }
            TiempoEntreToques += Time.deltaTime;
            /* ------------------------------------- SIN QUE SEA CENTRO-------------------------------------------------------------------------------
             if (Input.GetMouseButtonDown(0) == true && !aiming && ready && Input.mousePosition.y < Ancla.transform.position.y && !Pausa && !Enhollo && TiempoEntreToques > TiempoMaximo && Input.touchCount != 2 && Puntuacion>0 && CurrentTime>0)
             {
                 aiming = true;
                 PosicionInicial = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             }
             if (Input.GetMouseButtonUp(0) == true && aiming && ready && !Pausa && !Enhollo && TiempoEntreToques > TiempoMaximo && Input.touchCount != 2 && Puntuacion > 0 && CurrentTime > 0)
             {
                 PosicionFinal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                 TiempoEntreToques = 0;
                 ready = false;
                 Lanzar();
             }


             if (aiming && TiempoEntreToques > TiempoMaximo)
             {
                 //Encender la  Linea
                 Linea.GetComponent<LineRenderer>().enabled = true;
                 //Controlar posiciones
                 PosicionInicial = this.transform.position;
                 Linea.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);

                 Vector3 shootPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                 shootPos.z = 0;
                 shootPos = this.transform.position + (this.transform.position - shootPos);
                 PosicionFinal = shootPos;


                 //Enseñamos la linea
                 if (Vector3.Distance(PosicionInicial, PosicionFinal) > DistanciaMaxima)
                 {
                     Vector3 dir = PosicionFinal - PosicionInicial;
                     Debug.Log(dir.normalized);
                     PosicionFinal = this.transform.position + (dir.normalized * DistanciaMaxima);

                 }
                 Linea.GetComponent<LineRenderer>().SetPosition(1, PosicionFinal);

             }
             else
             {
                 Linea.GetComponent<LineRenderer>().enabled = false;
             }
            */
            //---------------------------------------------CUALQUIER PARTE------------------------------------------
            if (Input.GetMouseButtonDown(0) == true && !aiming && ready && Input.mousePosition.y < Ancla.transform.position.y && !Pausa && !Enhollo && TiempoEntreToques > TiempoMaximo && Input.touchCount != 2 && Puntuacion > 0 && CurrentTime > 0)
            {
                aiming = true;
                PosicionInicial = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0) == true && aiming && ready && !Pausa && !Enhollo && TiempoEntreToques > TiempoMaximo && Input.touchCount != 2 && Puntuacion > 0 && CurrentTime > 0)
            {
                PosicionFinal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TiempoEntreToques = 0;
                ready = false;
                Lanzar();
            }


            if (aiming && TiempoEntreToques > TiempoMaximo)
            {
                //Encender la  Linea
                Linea.GetComponent<LineRenderer>().enabled = true;
                //Controlar posiciones
                //PosicionInicial = this.transform.position;
                Linea.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
                
                Vector3 shootPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                shootPos.z = 0;
                shootPos = PosicionInicial + (PosicionInicial - shootPos);
                PosicionFinal = shootPos;
                

                //Enseñamos la linea
                if (Vector3.Distance(PosicionInicial, PosicionFinal) > DistanciaMaxima)
                {
                    Vector3 dir = PosicionFinal - PosicionInicial;
                    Debug.Log(dir.normalized);
                    PosicionFinal = this.transform.position + (dir.normalized* DistanciaMaxima);

                }
                Linea.GetComponent<LineRenderer>().SetPosition(1, PosicionFinal);

            }
            else
            {
                Linea.GetComponent<LineRenderer>().enabled = false;
            }

        }


        //Modo Survival
        if (posicion==1)
        {
            UIAnuncio.text = "+3 Shoots";
            Tiros.text = Puntuacion.ToString();
        }
        //Modo Contrareloj
        if(posicion==0)
        {
            UIAnuncio.text = "+20s";
            if (CurrentTime>0)
            {
                CurrentTime -= Time.deltaTime;
                Tiros.text = CurrentTime.ToString("0");
            }
            else
            {
                Tiros.text = "0";
            }
        }
        Agujero.text = "Hole " + AgujeroNumero.ToString();
        UIPuntos.text = AgujeroNumero.ToString() + " holes";
        Debug.Log(Ag);
        if(!anunciado)
        {
            UIAnuncioObj.SetActive(true);
            UITryAgain.SetActive(true);
            UITryAgainGrande.SetActive(false);
        }
        else
        {
            UIAnuncioObj.SetActive(false);
            UITryAgain.SetActive(false);
            UITryAgainGrande.SetActive(true);
        }
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.05f && EnAgujero == false)
        {
            ready = true;
            Glow.SetActive(true);
            if (posicion == 0 && CurrentTime < 0)
            {
                if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0f && EnAgujero == false)
                {
                    Pausa = true;
                    UIVictoria.SetActive(true);
                    if (PlayerPrefs.GetInt("RecordContra") < AgujeroNumero)
                    {
                        if (!enseñado)
                        {
                            NewHigScore.SetActive(true);
                            TextoAyuda.gameObject.SetActive(false);
                            TextoAyuda.text = Frases[Random.Range(0, Frases.Length)];
                            enseñado = true;
                        }

                        PlayerPrefs.SetInt("RecordContra", AgujeroNumero);
                        RecordContra = AgujeroNumero;
                        playFabManager.SendLeaderboard(AgujeroNumero, "Contrareloj");
                    }
                    else
                    {
                        if (!enseñado)
                        {
                            NewHigScore.SetActive(false);
                            TextoAyuda.gameObject.SetActive(true);
                            TextoAyuda.text = Frases[Random.Range(0, Frases.Length)];
                            enseñado = true;
                        }
                    } 
                }

            }
            if (posicion == 1 && Puntuacion == 0 && ready && !Tirado)
            {
                if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0f && EnAgujero == false)
                {
                    Pausa = true;
                    UIVictoria.SetActive(true);
                    if (PlayerPrefs.GetInt("RecordSurvival") < AgujeroNumero)
                    {
                        if (!enseñado)
                        {
                            NewHigScore.SetActive(true);
                            TextoAyuda.gameObject.SetActive(false);
                            TextoAyuda.text = Frases[Random.Range(0, Frases.Length)];
                            enseñado = true;
                        }
                        PlayerPrefs.SetInt("RecordSurvival", AgujeroNumero);
                        RecordSurvival = AgujeroNumero;
                        playFabManager.SendLeaderboard(AgujeroNumero, "Survival");
                    }
                    else
                    {
                        if (!enseñado)
                        {
                            NewHigScore.SetActive(false);
                            TextoAyuda.gameObject.SetActive(true);
                            TextoAyuda.text = Frases[Random.Range(0, Frases.Length)];
                            enseñado = true;
                        }
                    }
                }
            }
        }
        else
        {
            ready = false;
            Glow.SetActive(false);
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude.ToString());
        }

        
        if(AgujeroCerca != null)
        {
            //audioManager.Play("Hollo");
            Debug.Log("Lerping");
            this.transform.position = Vector3.Lerp(this.transform.position,AgujeroCerca.transform.position, 0.5f);
            this.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f, 1), new Vector3(0.15f, 0.15f, 1), 0.5f);
            
        }
    }
    void Lanzar()
    {
        Tirado = true;
        ready = false;
        aiming = false; //No dejamos apuntar al jugador
        audioManager.PlaySonido("Tiro"); 
        if(Puntuacion == 0) //Te quedas sin lanzamientos posibles
        {
            Debug.Log("Game Over");
        }
        //Debug.Log("Apuntando a" + PosicionFinal.ToString());
        if(Vector2.Distance(PosicionInicial, PosicionFinal) > DistanciaMaxima)
        {

        }
        Vector2 Direccion = PosicionInicial - PosicionFinal;
        CrearPolvo();
        this.GetComponent<Rigidbody2D>().AddForce(Direccion * Velocidad);
        Puntuacion--; //Restamos una unidad a la puntuacion
        Tirado = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Agujero" && this.GetComponent<Rigidbody2D>().velocity.magnitude <= VelocidadMaxima && !Enhollo)
        {
            /*
                Debug.Log("Win");
                HoleinOne = 0;
                AgujeroNumero++;
                this.transform.position = Agujeros[AgujeroNumero];
            */
            audioManager.PlaySonido("Hollo");
            StartCoroutine(NuevoHollo(collision.gameObject));
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Pared")
        {
            Debug.Log("Pared");

            audioManager.PlaySonidoN("Choque", this.GetComponent<Rigidbody2D>().velocity.magnitude/6);
            Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude/6);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Agujero" && this.GetComponent<Rigidbody2D>().velocity.magnitude <= VelocidadMaxima && !Enhollo)
        {
            audioManager.PlaySonido("Hollo");

            /*
                Debug.Log("Win");
                HoleinOne = 0;
                AgujeroNumero++;
                this.transform.position = Agujeros[AgujeroNumero];
            */
            StartCoroutine(NuevoHollo(collision.gameObject));
        }
    }
    void CrearPolvo()
    {
        Polvo.Play();

    }
    IEnumerator NuevoHollo(GameObject objeto)
    {
        Settings.BotonAtrasDes();
        if (posicion==1)
        {
            NumeroAleatorioSuma = Random.Range(1, 4);
            Puntuacion = Puntuacion + NumeroAleatorioSuma; 
        }
        else
        {
            NumeroAleatorioSuma = Random.Range(5, 15);
            CurrentTime += NumeroAleatorioSuma;
        }
        Suma.text = "+" + NumeroAleatorioSuma.ToString();
        Suma.gameObject.SetActive(true);
       

        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ready = false;
        Enhollo = true;
        AgujeroCerca = objeto;
        //Sonido de ganar
        yield return new WaitForSeconds(2.0f);
        if(AgujeroNumero != SpanwPointList.Count - 1)
        {
            AgujeroNumero++;
            AgujeroCerca = null;
            ready = true;
            Enhollo = false;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<TrailRenderer>().enabled = false;

            while (NumeroDeAgujeroEnLista == NumeroAnterior)
            {
                NumeroDeAgujeroEnLista = Random.Range(0, SpanwPointList.Count); 
            }
            NumeroAnterior = NumeroDeAgujeroEnLista;
            this.transform.position = SpanwPointList[NumeroDeAgujeroEnLista];
            this.GetComponent<TrailRenderer>().enabled = true;

            this.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        }
        Suma.gameObject.SetActive(false);
        this.GetComponent<TrailRenderer>().enabled = true;

        Settings.BotonAtrasDes();
    }
    public void CambiarPosicion(int p)
    {
        posicion = p;
    }
    public void tryAgain()
    {
        StopAllCoroutines();
        Enhollo = false;
        UIVictoria.SetActive(false);
        CurrentTime = StartingTime;
        Puntuacion = StartingPuntuacion;
        AgujeroNumero = 0;
        Pausa = false;
        NewHigScore.SetActive(false);
        enseñado = anunciado = false;
        this.GetComponent<TrailRenderer>().enabled = false;
        this.transform.position = SpanwPointList[0];
        this.GetComponent<TrailRenderer>().enabled = true;
    }
    public void SetRecord()
    {
        if (posicion == 0 && CurrentTime < 0)
        {
            Pausa = true;
            UIVictoria.SetActive(true);
            if (PlayerPrefs.GetInt("RecordContra") < AgujeroNumero)
            {
                PlayerPrefs.SetInt("RecordContra", AgujeroNumero);
                RecordContra = AgujeroNumero;
            }

        }
        if (posicion == 1 && Puntuacion == 0)
        {
            Pausa = true;
            UIVictoria.SetActive(true);
            if (PlayerPrefs.GetInt("RecordSurvival") < AgujeroNumero)
            {
                PlayerPrefs.SetInt("RecordSurvival", AgujeroNumero);
                RecordSurvival = AgujeroNumero;
            }
        }
    }
    public void Anuncio()
    {
        CurrentTime += AnuncioTime;
        Puntuacion += 3;
        UIVictoria.SetActive(false);
        NewHigScore.SetActive(false);
        enseñado = false;
        Pausa = false;
        anunciado = true;
    }
    void Zoom(float increment)
    {
       
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, ZoomMinimo, ZoomMaximo);
        
    }
  
}
