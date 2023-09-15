using UnityEngine;

public class IAPManager : MonoBehaviour
{
    public static bool AdsRemoved
    {
        get { return PlayerPrefs.GetInt("AdsRemoved", 0) == 1; }
        set { PlayerPrefs.SetInt("AdsRemoved", value ? 1 : 0); }
    }

    public void RemoveAds()
    {
        AdsRemoved = true;
    }
}


