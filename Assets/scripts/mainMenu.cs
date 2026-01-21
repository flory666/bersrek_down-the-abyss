using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class mainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button ReturnButton;
    [SerializeField] public Slider sfx;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Animator animator;
    public GameObject settingMenu;
    public GameObject mainMenu_panel;

    private void Awake()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        settingMenu.SetActive(false);
        mainMenu_panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }
    public void OnPlayButtonClicked()
    {
        animator.Play("main menu play");
        Invoke(nameof(StartGame), 2.0f);
    }
    private void OnSettingsButtonClicked()
    {
        settingMenu.SetActive(true);
        mainMenu_panel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(sfx.gameObject);
    }
    public void OnReturnButtonClicked()
    {
        Debug.Log("return button pressed");
        mainMenu_panel.SetActive(true);
        settingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);

    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    public void volumeSetting()
    {
        float volume = sfx.value;
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
    }
    private void StartGame()
    {
        SceneManager.LoadScene("game");
    }
}