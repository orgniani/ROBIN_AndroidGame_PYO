using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject creditsCanvas;

    private void Start()
    {
        creditsCanvas.SetActive(false);
    }

    //Credits Button
    public void OnOpenAndCloseCredits()
    {
        OnOpenAndCloseCanvas(creditsCanvas);
    }

    private void OnOpenAndCloseCanvas(GameObject canvas)
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
        }

        else
        {
            canvas.SetActive(true);
        }
    }
}
