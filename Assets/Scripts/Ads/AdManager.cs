using System;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] protected bool enableLogs = false;

    private string gameId;

    protected string adUnitId;
    protected bool adLoaded = false;
    
    protected static event Action OnUnityAdsInitialized;

    protected virtual void Awake()
    {
        InitializeAds();
    }

    private void Start()
    {
        SetIDs();
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        gameId = "5629789";
#elif UNITY_ANDROID
        gameId = "5629788";
#elif UNITY_EDITOR
        gameId = "5629788";
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, true, this);
        }
    }

    public void OnInitializationComplete()
    {
        if (enableLogs) Debug.Log("---------------------- DEBUG LOG ---------------------- Unity Ads initialization complete.");
        OnUnityAdsInitialized?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        if (enableLogs) Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    protected abstract void SetIDs();
}
