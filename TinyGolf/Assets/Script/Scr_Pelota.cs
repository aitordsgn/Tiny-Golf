using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_Pelota : MonoBehaviour
{
    public bool aiming, ready, EnAgujero, Enhollo = false;
    public Vector3 PosicionFinal, PosicionInicial, PosicionInicialDedo;
    public float DistanciaMaxima, Velocidad, VelocidadMaxima;
    public int Puntuacion, AgujeroNumero, HoleinOne;
    public GameObject Linea, AgujeroCerca, HolloEnUno;
    public ParticleSystem Polvo;
    public TextMeshProUGUI Agujero, Tiros;
    public AudioManager audioManager;
    public string Ag="0";

    //Variables Survival
    private int NumeroAleatorioSuma; // Numero que se suma
    public TextMeshProUGUI Suma; // Texto que enseña la suma

    // Variables Contrareloj
    
    float CurrentTime = 0f;
    [SerializeField] float StartingTime = 10f;
    [SerializeField] int posicion;

    //UI Victoria
    [SerializeField] GameObject UIVictoria;
    [SerializeField] GameObject SpawnPointsObj;
    [SerializeField] List<Transform> SpanwPointList = new List<Transform>();
    [SerializeField] int NumeroDeAgujeroEnLista,NumeroAnterior;
    private void Start()
    {
        CurrentTime = StartingTime;
        for(int c=0;c<SpawnPointsObj.transform.childCount;c++)
        {
            SpanwPointList.Add(SpawnPointsObj.transform.GetChild(c).transform);
            Destroy(SpawnPointsObj.transform.GetChild(c));
        }
        Destroy(SpawnPointsObj);
    }
    // Update is called once per frame
    void Update()
    {
        //Modo Survival
        if(posicion==1)
        {
            Agujero.text = "Hole " + AgujeroNumero.ToString();
            Tiros.text = Puntuacion.ToString();
        }

        //Modo Contrareloj
        if(posicion==0)
        {
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

        
        Debug.Log(Ag);
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f && EnAgujero == false)
        {
            ready = true;
            if(posicion<0)
            {
                UIVictoria.SetActive(true);
            }
        }
        else
        {
            ready = false;
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude.ToString());
        }

        if(Input.GetMouseButtonDown(0) == true && !aiming && ready)
        {
            aiming = true;
            PosicionInicial = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PosicionInicialDedo =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) == true && aiming && ready)
        {
            PosicionFinal= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Lanzar();
        }
        if(aiming)
        {
            //Encender la  Linea
            Linea.GetComponent<LineRenderer>().enabled = true;
            //Controlar posiciones
            PosicionInicial = this.transform.position;
            Linea.GetComponent<LineRenderer>().SetPosition(0, PosicionInicial);
            
            Vector3 shootPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shootPos.z = 0;
            shootPos = this.transform.position + (this.transform.position - shootPos);
            PosicionFinal = shootPos;
            
           
            //Enseñamos la linea
            if(Vector3.Distance(PosicionInicial , PosicionFinal) > DistanciaMaxima)
            {
                Vector3 dir = PosicionFinal - PosicionInicial;
                PosicionFinal = this.transform.position + (dir.normalized * DistanciaMaxima);
            }
            Linea.GetComponent<LineRenderer>().SetPosition(1, PosicionFinal);

        }
        else
        {
            Linea.GetComponent<LineRenderer>().enabled = false;
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
        aiming = false; //No dejamos apuntar al jugador
        Puntuacion--; //Restamos una unidad a la puntuacion
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
        NumeroAleatorioSuma = Random.Range(1, 4);
        Puntuacion = Puntuacion + NumeroAleatorioSuma;
        Suma.text = "+" + NumeroAleatorioSuma.ToString();
        Suma.gameObject.SetActive(true);
        if (HoleinOne == 1)
        {
            HolloEnUno.SetActive(true);
                
        }

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
            this.GetComponent<TrailRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            while (NumeroDeAgujeroEnLista == NumeroAnterior)
            {
                NumeroDeAgujeroEnLista = Random.Range(0, SpanwPointList.Count); 
            }
            NumeroAnterior = NumeroDeAgujeroEnLista;
            this.transform.position = SpanwPointList[NumeroDeAgujeroEnLista].position;
            this.GetComponent<TrailRenderer>().enabled = true;
            this.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        }
        Suma.gameObject.SetActive(false);

    }
    public void CambiarPosicion(int p)
    {
        posicion = p;
    }
    public void tryAgain()
    {
        UIVictoria.SetActive(false);
        CurrentTime = StartingTime;
        Puntuacion = 10;

    }
}
