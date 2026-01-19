using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class pauseMenu : MonoBehaviour
{
    public Controls Inputs;
    public GameObject pauseMenuUI;
    public GameObject Gutsty;
    public GameObject Camera;
    private gutadinberbec guts;
    private camera cam;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Inputs = new Controls();
        Inputs.guts.pause.performed += ctx => pause();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        pauseMenuUI.SetActive(false);
        guts=Gutsty.GetComponent<gutadinberbec>();
        cam=Camera.GetComponent<camera>();
    }
    private void pause()
    {
        if (Time.timeScale == 1)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            guts.OnDisable();
            cam.OnDisable();
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            guts.OnEnable();
            cam.OnEnable();
        }
    }
    private void OnPlayButtonClicked()
    {
        pauseMenuUI.SetActive(false);
        guts.OnEnable();
        cam.OnEnable();
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
