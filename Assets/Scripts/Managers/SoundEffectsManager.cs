using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ActionController actionController;

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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (!gameManager)
        {
            Debug.LogError($"{name}: {nameof(gameManager)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;

        }

        if (!actionController)
        {
            Debug.LogError($"{name}: {nameof(actionController)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;

        }

        if (!heal)
        {
            Debug.LogError($"{name}: {nameof(heal)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!rangeAttack)
        {
            Debug.LogError($"{name}: {nameof(rangeAttack)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!meleeAttack)
        {
            Debug.LogError($"{name}: {nameof(meleeAttack)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!death)
        {
            Debug.LogError($"{name}: {nameof(death)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!pressPlay)
        {
            Debug.LogError($"{name}: {nameof(pressPlay)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!pressButton)
        {
            Debug.LogError($"{name}: {nameof(pressButton)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!win)
        {
            Debug.LogError($"{name}: {nameof(win)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!lose)
        {
            Debug.LogError($"{name}: {nameof(lose)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
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
    private void HandleGameOverSound(GameOverReason reason)
    {
        switch (reason)
        {
            case GameOverReason.WIN:
                audioSource.PlayOneShot(win);
                break;

            case GameOverReason.LOSE:
                audioSource.PlayOneShot(lose);
                break;
        }
    }

    private void HandleDeathSound()
    {
        audioSource.PlayOneShot(death);
    }

    private void HandleActionSound(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.HEAL:
                audioSource.PlayOneShot(heal);
                break;

            case ActionType.RANGE:
                audioSource.PlayOneShot(rangeAttack);
                break;

            case ActionType.MELEE:
                audioSource.PlayOneShot(meleeAttack);
                break;
        }
    }

    public void OnPressPlay()
    {
        audioSource.PlayOneShot(pressPlay);
    }

    public void OnPressButton()
    {
        audioSource.PlayOneShot(pressButton);
    }
}