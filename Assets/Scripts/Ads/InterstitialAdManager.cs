using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class InterstitialAdManager : AdManager, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Unit IDs")]
    [SerializeField] private string interstitialAndroidAdUnitId = "Interstitial_Android";
    [SerializeField] private string interstitialIOSAdUnitId = "Interstitial_iOS";

    [Header("Managers")]
    [SerializeField] private GameManager gameManager;

    private static InterstitialAdManager instance;

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            base.Awake();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnUnityAdsInitialized += InitializeInterstitial;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnUnityAdsInitialized -= InitializeInterstitial;

        if (gameManager != null)
            gameManager.OnGameOver -= ShowInterstitial;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGameManager();
    }

    private void FindGameManager()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        else
        {
            gameManager.OnGameOver += ShowInterstitial;
        }
    }

    protected override void SetIDs()
    {
#if UNITY_IOS
        adUnitId = interstitialIOSAdUnitId;
#elif UNITY_ANDROID
        adUnitId = interstitialAndroidAdUnitId;
#endif
    }

    private void InitializeInterstitial()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitial(GameOverReason reason)
    {
        if (adLoaded)
            Advertisement.Show(adUnitId, this);
        else
            Debug.Log($"Interstitial: Error loading Ad");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        adLoaded = true;
        if (enableLogs) Debug.Log("---------------------- DEBUG LOG ---------------------- Interstitial Ad loaded successfully");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (enableLogs) Debug.Log($"Interstitial: Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (enableLogs) Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (enableLogs) Debug.Log("Interstitial showing successfully");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        if (enableLogs) Debug.Log("Interstitial clicked successfully");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (enableLogs) Debug.Log("Interstitial fully watched");
        Advertisement.Load(adUnitId, this);
    }
}
