using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProperty : MonoBehaviour
{
    private const string DividerLine = "////////////////////////////////////////////";

    public ScriptableCharacterBase HuskScriptableBase;
    public ScriptableCharacterBase DotScriptableBase;
    public ScriptableCharacterBase KnightScriptableBase;

    // Start is called before the first frame update
    void Start()
    {
        //RunAllTests();
        //AbilitiesStopListeningCorrectly();
        //GameInitializes();
        //RangedAttackChoiceTest();
        BasicCallAndAbilityTest();
        //BasicGameTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RunAllTests()
    {
        BasicEventListeningTest();
        Divider();
        EventArgsCarryOverTest();
        Divider();
        AbilitiesStopListeningCorrectly();
        Divider();
        GameInitializes();
    }

    private void BasicGameTest()
    {
        var g = SetupBasicTestGame(9, 0);
        g.Init();
        var huskBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(HuskScriptableBase);
        var dotBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(DotScriptableBase);
        var knightBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(KnightScriptableBase);

        var huskCreature = new Creature();
        huskCreature.InitFromBase(huskBase);
        var dotCreature = new Creature();
        dotCreature.InitFromBase(dotBase);
        var knightCreature = new Creature();
        knightCreature.InitFromBase(knightBase);
    }

    private void BasicCallAndAbilityTest()
    {
        var g = SetupBasicTestGame(9, 0);
        g.Init();
        var huskBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(HuskScriptableBase);
        var dotBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(DotScriptableBase);

        var huskCreature = new Creature();
        huskCreature.InitFromBase(huskBase);
        var dotCreature = new Creature();
        dotCreature.InitFromBase(dotBase);

        Debug.Log("Husks name is " + huskCreature.DisplayName);
        Debug.Log("Dots name is " + dotCreature.DisplayName);
        Debug.Log("Husk first ability is " + huskCreature.Abilities[0].DisplayName);
        Debug.Log("Dot third ability is " + dotCreature.Abilities[2].DisplayName);

        g.Players[0].PutInReserve(dotCreature);
        g.Players[1].PutInReserve(huskCreature);

        g.CallCharacter(dotCreature, g.Players[0].ValidInitSpaces[0], g.Players[0]);
        g.CallCharacter(huskCreature, g.Players[1].ValidInitSpaces[0], g.Players[1]);
        Debug.Log("Dot third ability is " + dotCreature.Abilities[2].DisplayName);


        dotCreature.RemoveAbility(dotCreature.Abilities[2]);
        dotCreature.GainAbility(new HelltechArmorAbility(), true);
        dotCreature.GainAbility(new HelltechArmorAbility(), true);
        huskCreature.GainAbility(new ExpensiveArmorAbility(), true);

        dotCreature.CanAct = true;
        Debug.Log("Dot attack is " + dotCreature.Attack);
        Debug.Log("Husk health is " + huskCreature.Health);
        Debug.Log("Dot health is " + dotCreature.Health);
        dotCreature.BasicAttack(huskCreature);
        Debug.Log("Dot attack is " + dotCreature.Attack);
        Debug.Log("Husk health is " + huskCreature.Health);
        Debug.Log("Dot health is " + dotCreature.Health);
        Debug.Log("Player 2 grave has " + g.Players[1].Graveyard.Count);
    }

    private void RangedAttackChoiceTest()
    {
        var g = SetupBasicTestGame(9, 3);

        g.Init();
        var attacker = g.Players[0].Reserve[0];
        var defender = g.Players[1].Reserve[0];
        var defender2 = g.Players[1].Reserve[1];
        attacker.DisplayName = "Blingus";
        defender.DisplayName = "FURGUSON";
        defender2.DisplayName = "STINKY";
        var abil = new ArmCannonAbility();
        attacker.GainAbility(abil, true);
        attacker.GainAbility(new HelltechArmorAbility(), true);
        attacker.GainAbility(new HelltechArmorAbility(), true);
        var buffAbil = new GargauthBlessingAbility();
        attacker.GainAbility(buffAbil, true);
        buffAbil.Cooldown = 0;
        abil.Cooldown = 0;
        attacker.StatsChange(AtkChg: 1, HealthChg: 1);
        defender.StatsChange(AtkChg: 1, HealthChg: 8);
        defender2.StatsChange(AtkChg: 1, HealthChg: 2);
        g.CallCharacter(attacker, g.GameGrid[(0, 0)], g.Players[0]);
        g.CallCharacter(defender, g.GameGrid[(0, 1)], g.Players[1]);
        g.CallCharacter(defender2, g.GameGrid[(0, 2)], g.Players[1]);

        Debug.Log("Performed Setup!");

        var targetAbil = (ActiveAbility)g.Players[0].OnBoardCreatures[0].Abilities[0];
        Debug.Log("Current health of defender 1: " + defender.DisplayName + " " + defender.Health + "/" + defender.MaxHealth);
        Debug.Log("Current health of defender 2: " + defender2.DisplayName + " " + defender2.Health + "/" + defender2.MaxHealth);
        Debug.Log("Current attack of attacker: " + attacker.Attack);
        targetAbil.Activate();
        Debug.Log("Activated ability!");
        Debug.Log("New health of defender 1: " + defender.DisplayName + " " + defender.Health + "/" + defender.MaxHealth);
        Debug.Log("New health of defender 2: " + defender2.DisplayName + " " + defender2.Health + "/" + defender2.MaxHealth);
        Debug.Log("Current attack of attacker: " + attacker.Attack);

        ((ActiveAbility)g.Players[0].OnBoardCreatures[0].Abilities[3]).Activate();
        Debug.Log("Post-buff stats of attacker: Health " + attacker.Health + " Attack " + attacker.Attack);
    }

    private void GameInitializes()
    {
        var g = SetupBasicTestGame(9, 3);

        g.Init();
        Debug.Log("Game initialized!");

        g.CallCharacter(g.Players[0].Reserve[0], g.GameGrid[(0, 0)], g.Players[0]);
        Debug.Log("Called a character!");

        g.EndTurn();
        Debug.Log("Ended a turn!");
    }

    private void AbilitiesStopListeningCorrectly()
    {
        EventManager.Reset();

        var a1 = new TestTurnEndPassive();
        var a2 = new TestTurnEndPassive();

        a1.AddOnboardTriggers();
        a2.AddOnboardTriggers();

        Debug.Log("Should see two triggers.");
        EventManager.Invoke(GachaEventType.EndOfTurn, this, new EventArgs());

        a1.RemoveOnboardTriggers();
        Debug.Log("Should see one trigger.");
        EventManager.Invoke(GachaEventType.EndOfTurn, this, new EventArgs());
    }

    private void EventArgsCarryOverTest()
    {
        List<Ability> abils = new List<Ability>();

        abils.Add(new Ability());
        abils.Add(new TestPassive());
        abils.Add(new TestPassive());

        EventManager.Reset();

        foreach (var a in abils)
        {
            a.AddReserveTriggers();
        }

        var args = new TestEventArgs(3);

        Debug.Log("Val starts at 3, should add one each time to 5");

        EventManager.Invoke(GachaEventType.TestAddingTrigger, this, args);

        Debug.Log("Final val is: " + args.ArgVal);
    }

    private void BasicEventListeningTest()
    {
        List<Ability> abils = new List<Ability>();

        abils.Add(new Ability());
        abils.Add(new TestPassive());
        abils.Add(new TestPassive());

        Debug.Log("Starting test run");

        EventManager.Reset();

        foreach (var a in abils)
        {
            a.AddReserveTriggers();
        }

        Debug.Log("Should see two TRIGGERED instances:");

        EventManager.Invoke(GachaEventType.TestTrigger, this, new TestEventArgs(3));
    }

    public static Game SetupBasicTestGame(int gridSize, int creatureCount)
    {
        var p1creats = new List<Creature>();
        var p2creats = new List<Creature>();
        for (int i = 0; i < creatureCount; i++)
        {
            p1creats.Add(new Creature());
            p2creats.Add(new Creature());
        }

        var p1Args = new PlayerArgs() { type = PlayerType.HUMAN, startingCreatures = p1creats };
        var p2Args = new PlayerArgs() { type = PlayerType.HUMAN, startingCreatures = p2creats };

        var gameArgs = new GameArgs() { GridXSize = gridSize, GridYSize = gridSize, Players = new PlayerArgs[] { p1Args, p2Args } };

        return new Game(gameArgs);
    }

    private void Divider()
    {
        Debug.Log(DividerLine);
    }
}
