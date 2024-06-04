using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameUIManager gameUIManager;
    [HideInInspector]
    public Character playerCharacter;
    private bool gameIsOver;

    void Awake()
    {
        playerCharacter = GameObject.FindWithTag(Tags.PLAYER).GetComponent<Character>();
    }

    private void GameOver()
    {
        // Debug.Log("GAME OVER");

        Time.timeScale = 1f;
        gameUIManager.ShowGameOverUI();
    }

    public void GameIsFinshed()
    {
        // Debug.Log("GAME IS FINISH");

        Time.timeScale = 1f;
        gameUIManager.ShowGameIsFinishUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            gameUIManager.TogglePauseUI();

        if (playerCharacter.currentState == Character.CharacterState.DEAD)
        {
            gameIsOver = true;
            GameOver();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Scenes.MAIN_MENU);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
