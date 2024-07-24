using TMPro;
using UnityEngine;

public class UITimeCounter : ITimeCounter
{
    private float startTime;
    private bool isCounting = false;
    private TMP_Text timeText;

    public UITimeCounter(TMP_Text timeText)
    {
        this.timeText = timeText;
    }

    public void StartCounting()
    {
        startTime = Time.time;
        isCounting = true;
    }

    public void StopCounting()
    {
        isCounting = false;
    }

    public void UpdateTimeText()
    {
        if (!isCounting) return;

        float elapsedTime = Time.time - startTime;
        int minutes = (int)((elapsedTime % 3600) / 60);
        int seconds = (int)(elapsedTime % 60);

        timeText.text = $"{minutes:D2}:{seconds:D2}";
    }
}
