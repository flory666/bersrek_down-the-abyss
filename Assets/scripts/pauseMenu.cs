using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public Controls Inputs;
    public GameObject pauseMenuUI;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Inputs = new Controls();
        Inputs.guts.pause.performed += ctx => pause();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        pauseMenuUI.SetActive(false);
    }
    private void pause()
    {
        if (Time.timeScale == 1)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
    }
    private void OnPlayButtonClicked()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnQuitButtonClicked()
    {
        SceneManager.LoadScene("main menu");
    }
    private void OnEnable()
    {
        Inputs.Enable();
    }
    private void OnDisable()
    {
        Inputs.Disable();
    }
}
