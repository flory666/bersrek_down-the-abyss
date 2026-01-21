using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{
    public Progres progres;
    private float time;
    private int kills;
    public Text timeText;
    public Text killsText;
    [SerializeField] private Button ReturnButton;

    void Start()
    {   Time.timeScale = 1f;
        progres = GameObject.FindGameObjectWithTag("progres").GetComponent<Progres>();

        int seconds = Mathf.FloorToInt(progres.time_survived);

        timeText.text = "" + seconds + " s";
        killsText.text = "" + progres.enemies_killed;
    }
    public void OnReturnButtonClicked()
    {
        SceneManager.LoadScene("main menu");
    }
}