using GamePlay;
using UnityEngine;
using ZFramework.Core;

public class GameEntry : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        UIEntry.CommandViewEntry.OpenStartGameView();
    }
}