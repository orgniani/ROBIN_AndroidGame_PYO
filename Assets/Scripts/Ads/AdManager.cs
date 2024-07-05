using System;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected bool enableLogs = false;

    private string gameId;

    protected string adUnitId;
    protected bool adLoaded = false;

    protected static event Action OnUnityAdsInitialized;

    private void Awake()
    {
        if (!gameManager)
        {
            Debug.LogError($"{name}: {nameof(gameManager)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

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

    private void Start()
    {
        SetIDs();
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
