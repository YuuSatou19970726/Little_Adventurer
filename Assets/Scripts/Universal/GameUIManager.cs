using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameManager gameManager;

    // UI Character
    public TextMeshProUGUI CoinText;
    public Slider HealthSlider;

    // Menu
    [SerializeField]
    private GameObject UI_Pause;
    [SerializeField]
    private GameObject UI_GameOver;
    [SerializeField]
    private GameObject UI_GameIsFinish;

    private enum GameUIState
    {
        GAMEPLAY,
        PAUSE,
        GAMEOVER,
        GAMEISFINISHED,
    }

    GameUIState currentState;

    void Start()
    {
        SwitchUIState(GameUIState.GAMEPLAY);
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = gameManager.playerCharacter.GetComponent<Health>().CurrentHealthPercentage;
        CoinText.text = gameManager.playerCharacter.Coin.ToString();
    }

    private void SwitchUIState(GameUIState state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_GameIsFinish.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case GameUIState.GAMEPLAY:
                break;
            case GameUIState.PAUSE:
                Time.timeScale = 0;
                UI_Pause.SetActive(true);
                break;
            case GameUIState.GAMEOVER:
                UI_GameOver.SetActive(true);
                break;
            case GameUIState.GAMEISFINISHED:
                UI_GameIsFinish.SetActive(true);
                break;
        }

        currentState = state;
    }

    public void TogglePauseUI()
    {
        if (currentState == GameUIState.GAMEPLAY)
            SwitchUIState(GameUIState.PAUSE);
        else if (currentState == GameUIState.PAUSE)
            SwitchUIState(GameUIState.GAMEPLAY);
    }

    public void ButtonMainMenu()
    {
        gameManager.ReturnToMainMenu();
    }

    public void ButtonRestart()
    {
        gameManager.Restart();
    }

    public void ShowGameOverUI()
    {
        SwitchUIState(GameUIState.GAMEOVER);
    }

    public void ShowGameIsFinishUI()
    {
        SwitchUIState(GameUIState.GAMEISFINISHED);
    }
}
