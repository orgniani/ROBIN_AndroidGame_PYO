using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Scene indexes")]
    [SerializeField] private int gameBuildIndex = 0;

    [Header("Loading")]
    [SerializeField] private int fakeLoadingTime = 2;

    public void RestartGame()
    {
        LoadAndOpen(gameBuildIndex);
    }

    private void LoadAndOpen(int sceneBuildIndex)
    {
        LoaderManager.Get().LoadScene(sceneBuildIndex, fakeLoadingTime);
    }
}
