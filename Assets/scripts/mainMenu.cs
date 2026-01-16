using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] public Button ReturnButton;
    [SerializeField] public Slider music;
    [SerializeField] public Slider sfx;
    [SerializeField] private Animator animator;
    public GameObject settingMenu;
    public GameObject mainMenu_panel;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        settingMenu.SetActive(false);
        mainMenu_panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }
    private void OnPlayButtonClicked()
    {
        animator.Play("main menu play");
        Invoke(nameof(StartGame), 2.0f);
    }
    private void OnSettingsButtonClicked()
    {
        settingMenu.SetActive(true);
        mainMenu_panel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(music.gameObject);
    }
    public void OnReturnButtonClicked()
    {
        mainMenu_panel.SetActive(true);
        settingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    private void StartGame()
    {
        SceneManager.LoadScene("game");
    }
}