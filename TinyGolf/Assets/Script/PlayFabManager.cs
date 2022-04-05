using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    [Header("Ventanas")]
    public GameObject nameWindow , leaderboardWindow;

    [Header("Display name Window")]
    public TextMeshProUGUI nameError;
    public TMP_InputField nameInput;

    [Header ("Leaderboard")]
    public GameObject RowPrefab;
    public Transform RowsParent;
    [SerializeField] GameObject ErrorWifi;
    [SerializeField] GameObject Play;
    public string name2;
    [SerializeField] Color Color;
    [SerializeField] bool Encontrado;
    [SerializeField] string Actual,DisplayName;


    // Start is called before the first frame update
    void Start()
    {
        Login();
        /*if (PlayerPrefs.GetString("name")==null)
        {
            Login(); 
        }*/
        
    }
    

    public void Login(){
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucces, OnErrorName);
    }
    void OnLoginSucces(LoginResult result)
    {
        Debug.Log("Succesful login/account create");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {

            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            name2 = name;
            PlayerPrefs.SetString("name", name2);
            Debug.Log("Nombre:" + name2);
            if (name2 == null)
            {
                nameWindow.SetActive(true);
            }
        }
        else
        {
            nameWindow.SetActive(true);
        }
    }
    void OnErrorName(PlayFabError error)
    {
        Debug.Log("Error in loggin in/creating account");
        Debug.Log(error.GenerateErrorReport());
        nameWindow.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error in loggin in/creating account");
        Debug.Log(error.GenerateErrorReport());
        ErrorWifi.SetActive(true);
        
    } 
    void OnErrorLogin(PlayFabError error)
    {
        nameError.gameObject.SetActive(true);
        nameError.GetComponent<Animator>().SetTrigger("Show");
        Debug.Log("Nombre en uso");
        nameError.text = error.GenerateErrorReport().ToString();

    }

    public void SendLeaderboard(int Score ,string LeaderboardName)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = LeaderboardName ,Value= Score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesfull leaderboard send");
    }
    public void OpenLeaderboard()
    {
        leaderboardWindow.SetActive(!leaderboardWindow.active);
        Play.SetActive(!leaderboardWindow.active);
        ErrorWifi.SetActive(false);
        GetLeaderboard("Survival");
    }
    public void GetLeaderboard( string LeaderboardName)
    {
        Encontrado = false;
        SendLeaderboard(PlayerPrefs.GetInt("RecordContra"), "Contrareloj");
        SendLeaderboard(PlayerPrefs.GetInt("RecordSurvival"), "Survival");

        var request = new GetLeaderboardRequest
        {
            StatisticName = LeaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        Actual = LeaderboardName;
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    
    void GetLeaderboardAroundPlayer(string LeaderboardName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = LeaderboardName,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderBoardAroundplayer , OnError);
    }
    void OnLeaderBoardAroundplayer(GetLeaderboardAroundPlayerResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            item.DisplayName = name2;
            GameObject newGo = Instantiate(RowPrefab, RowsParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            if (item.Position==9)
            {
                texts[0].text = "11";

            }
            else
            {
                texts[0].text = (item.Position + 1).ToString() + ".";

            }
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        Encontrado = false;
        Debug.Log(PlayerPrefs.GetString("name"));
        foreach(Transform item in RowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach ( var item in result.Leaderboard)
        {
            if (item.StatValue!=0)
            {
                GameObject newGo = Instantiate(RowPrefab, RowsParent);
                TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = (item.Position + 1).ToString() + ".";
                texts[1].text = item.DisplayName;
                if(item.DisplayName == null)
                {
                    texts[1].text = "Anonimous";
                }
                if(item.DisplayName == name2)
                {
                    Encontrado = true;
                }
                texts[2].text = item.StatValue.ToString();
                //Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue); 
            }
        }
        if(!Encontrado)
        {
            GetLeaderboardAroundPlayer(Actual);
        }

    }
    public void SubmitNameButton()
    {
        if (nameInput.text !="" || nameInput.text.Length > 1 &&nameInput.text.Length <= 10)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = nameInput.text,
            };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnErrorLogin);
        PlayerPrefs.SetString("name",nameInput.text.ToString());
        nameWindow.SetActive(false);
        }
        else
        {
            if(nameInput.text =="" || nameInput.text.Length >= 3)
            {
                nameError.gameObject.SetActive(true);
                nameError.GetComponent<Animator>().SetTrigger("Show");
                nameError.text = "Your nickname can't be empty";
            }
            if (nameInput.text.Length > 10)
            {
                nameError.gameObject.SetActive(true);
                nameError.GetComponent<Animator>().SetTrigger("Show");
                nameError.text = "Your nickname is too long";
            }
        }
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        nameWindow.SetActive(false);
        leaderboardWindow.SetActive(true);
        OpenLeaderboard();
    }
    
}
