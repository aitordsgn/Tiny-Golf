using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour , IUnityAdsListener
{
    [SerializeField] Scr_Pelota pelota;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("4670491");
        Advertisement.AddListener(this);
    }

    public void RewardedAd()
    {
        if(Advertisement.IsReady("Rewarded_Android"))
        {
            Advertisement.Show("Rewarded_Android");
        }
        else
        {
            Debug.Log("Reward ad is not ready");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Unity Ads Errpr");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Unity Ads Started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
            if(placementId == "Rewarded_Android" && showResult ==ShowResult.Finished)
            {
                pelota.Anuncio();
            }
    }
}
