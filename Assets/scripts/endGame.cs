using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class endGame : MonoBehaviour
{
    public Progres progres;
    private float time;
    private int kills;
    public Text timeText;
    public Text killsText;

    void Start()
    {
        progres = FindAnyObjectByType<Progres>();

        if (progres != null)
        {
            timeText.text = $"Time survived: {FormatTime(progres.time_survived)}";
            killsText.text = $"Enemies killed: {progres.enemies_killed}";
        }

        string FormatTime(float seconds)
        {
            int min = Mathf.FloorToInt(seconds / 60f);
            int sec = Mathf.FloorToInt(seconds % 60f);
            return $"{min:00}:{sec:00}";
        }
    }
}