using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerProfileSystem playerProfile;
    public static void GameLost()
    {
      //  Time.timeScale = 0;
        // show game lost screen
    }

    public void GameRestart()
    {
        // restart game after death
    }

    public void GameStart()
    {

    }
}
