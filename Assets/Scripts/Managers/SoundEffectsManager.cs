using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Audio clips")]
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip rangeAttack;
    [SerializeField] private AudioClip meleeAttack;
    [SerializeField] private AudioClip death;

    [SerializeField] private AudioClip pressPlay;
    [SerializeField] private AudioClip pressButton;

    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;

    private AudioSource audioSource;
    private ActionController actionController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        actionController = gameManager.ActionController;

        ValidateReferences();
    }

    private void OnEnable()
    {
        gameManager.OnGameOver += HandleGameOverSound;
        gameManager.OnPlayerDeath += HandleDeathSound;

        actionController.OnActionChosen += HandleActionSound;
    }

    private void OnDisable()
    {
        gameManager.OnGameOver -= HandleGameOverSound;
        gameManager.OnPlayerDeath -= HandleDeathSound;

        actionController.OnActionChosen -= HandleActionSound;
    }

    private void HandleGameOverSound(GameOverReason reason, Player player)
    {
        switch (reason)
        {
            case GameOverReason.WIN:
                PlaySound(win);
                break;

            case GameOverReason.LOSE:
                PlaySound(lose);
                break;
        }
    }

    private void HandleDeathSound()
    {
        PlaySound(death);
    }

    private void HandleActionSound(Player attacker, ActionType actionType, Player target)
    {
        switch (actionType)
        {
            case ActionType.HEAL:
                PlaySound(heal);
                break;

            case ActionType.RANGE_ATTACK:
                PlaySound(rangeAttack);
                break;

            case ActionType.MELEE_ATTACK:
                PlaySound(meleeAttack);
                break;
        }
    }

    public void OnPressPlay()
    {
        PlaySound(pressPlay);
    }

    public void OnPressButton()
    {
        PlaySound(pressButton);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    private void ValidateReferences()
    {
        if (!audioSource)
        {
            Debug.LogError($"{name}: {nameof(audioSource)} is missing!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!gameManager)
        {
            Debug.LogError($"{name}: {nameof(gameManager)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        ValidateAudioClips();
    }

    private void ValidateAudioClips()
    {
        if (heal == null) Debug.LogError($"{name}: {nameof(heal)} audio clip is missing.");
        if (rangeAttack == null) Debug.LogError($"{name}: {nameof(rangeAttack)} audio clip is missing.");
        if (meleeAttack == null) Debug.LogError($"{name}: {nameof(meleeAttack)} audio clip is missing.");
        if (death == null) Debug.LogError($"{name}: {nameof(death)}  audio clip is missing.");
        if (pressPlay == null) Debug.LogError($"{name}: {nameof(pressPlay)}  audio clip is missing.");
        if (pressButton == null) Debug.LogError($"{name}: {nameof(pressButton)}  audio clip is missing.");
        if (win == null) Debug.LogError($"{name}: {nameof(win)}  audio clip is missing.");
        if (lose == null) Debug.LogError($"{name}: {nameof(lose)}  audio clip is missing.");
    }
}