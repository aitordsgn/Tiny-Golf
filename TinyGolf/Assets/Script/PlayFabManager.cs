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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("name")!=null)
        {
            Login(); 
        }
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
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucces, OnError);
    }
    void OnLoginSucces(LoginResult result)
    {
        Debug.Log("Succesful login/account create");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        name = result.InfoResultPayload.PlayerProfile.DisplayName;
        if(name== null)
        {
            nameWindow.SetActive(true);
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error in loggin in/creating account");
        Debug.Log(error.GenerateErrorReport());
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
        GetLeaderboard("Survival");
    }
    public void GetLeaderboard( string LeaderboardName)
    {
        SendLeaderboard(PlayerPrefs.GetInt("RecordContra"), "Contrareloj");
        SendLeaderboard(PlayerPrefs.GetInt("RecordSurvival"), "Survival");

        var request = new GetLeaderboardRequest
        {
            StatisticName = LeaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(Transform item in RowsParent )
        {
            Destroy(item.gameObject);
        }
        foreach ( var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(RowPrefab,RowsParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString() + ".";
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    public void SubmitNameButton()
    {
        if (nameInput.text !="" || nameInput.text.Length <= 10)
        {
            var request = new UpdateUserTitleDisplayNameRequest
        {
        
            DisplayName = nameInput.text,
            
	           
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        PlayerPrefs.SetString("name",nameInput.text.ToString());
            nameWindow.SetActive(false);
        }
        else
        {
            if(nameInput.text =="")
            {
                nameError.gameObject.SetActive(true);
                nameError.text = "Your nickname can't be empty";
            }
            if (nameInput.text.Length > 10)
            {
                nameError.gameObject.SetActive(true);
                nameError.text = "Your nickname is too long";
            }
        }
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        nameWindow.SetActive(false);
        leaderboardWindow.SetActive(true);
    }
}
