using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static bool CanCallCreatureToGridSpace(
        Creature selectedCreature,
        Player currentPlayer,
        Game game,
        GridSpace targetGridSpace)
    {
        return selectedCreature != null &&
               currentPlayer.Reserve.Contains(selectedCreature) &&
               currentPlayer == game.Players[game.CurrentPlayerIndex] &&
               game.IsInitTurn &&
               currentPlayer.CanCallThisTurn &&
               game.CurrentInitiative >= selectedCreature.Initiative &&
               currentPlayer.ValidInitSpaces.Contains(targetGridSpace);
    }
}
