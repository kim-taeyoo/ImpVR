using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
    public GameObject credit;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button creditButton;
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(EnableOption);
        creditButton.onClick.AddListener(Enablecredit);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        credit.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        credit.SetActive(false);
    }
    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        credit.SetActive(false);
    }
    public void Enablecredit()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        credit.SetActive(true);
    }
}
