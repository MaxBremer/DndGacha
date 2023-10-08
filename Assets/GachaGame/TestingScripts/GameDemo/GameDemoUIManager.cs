using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameDemoUIManager : MonoBehaviour
{
    private const int UI_UPDATE_PRIORITY = 5;
    public Game MyGame; // Reference to the Game script
    public TMP_Text PlayerTurnText; // Use TMP_Text for TextMeshPro
    public TMP_Text InitiativeCountText; // Use TMP_Text for TextMeshPro
    public Text EndTurnButtonText;

    public void InitUIManagement(Game game)
    {
        MyGame = game;
        EventManager.StartListening(GachaEventType.StartOfTurn, UpdateUI, UI_UPDATE_PRIORITY);
        UpdateUI(this, new TurnStartArgs { PlayerWhoseTurnIsStarting = 0 });
    }

    private void UpdateUI(object sender, EventArgs e)
    {
        if (e is TurnStartArgs eStart)
        {
            PlayerTurnText.text = "Current Player: " + (MyGame.CurrentPlayer.MyPlayerIndex + 1);
            InitiativeCountText.text = "Current Initiative Count: " + MyGame.CurrentInitiative + "\n" + (MyGame.IsInitTurn ? "IS an Initiative Turn" : "NOT an Initiative Turn");
            EndTurnButtonText.text = "End Player " + (MyGame.CurrentPlayerIndex + 1) + " Turn";
        }
    }
}
