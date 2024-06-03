using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character playerCharacter;
    private bool gameIsOver;

    void Awake()
    {
        playerCharacter = GameObject.FindWithTag(Tags.PLAYER).GetComponent<Character>();
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
    }

    public void GameIsFinshed()
    {
        Debug.Log("GAME IS FINISH");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

        if (playerCharacter.currentState == Character.CharacterState.DEAD)
        {
            gameIsOver = true;
            GameOver();
        }
    }
}
