using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameDemo : MonoBehaviour
{

    // Constants
    private const int RESERVE_OFFSET_AMOUNT = 1;
    private const float verticalSpacing = 150f;
    private Vector2 startingPosition = new Vector2(0, 300);
    private const int UI_EVENT_PRIORITY = -5;

    //Reference Dictionaries
    public Dictionary<GridSpace, (GameObject obj, GameDemoSquare square)> GridObjs = new Dictionary<GridSpace, (GameObject obj, GameDemoSquare square)>();
    internal Dictionary<Creature, GameObject> CreatObjs = new Dictionary<Creature, GameObject>();

    internal Dictionary<GridSpace, List<GridSpace>> curValidMoveDict = null;

    public Game MyGame;
    public GameDemoSelectState MySelectState = GameDemoSelectState.UNSELECTED;

    public bool TestGame;

    internal HighlightManager MyHighlightManager;

    //Prefabs and set references
    public GameObject gridSquarePrefab;
    public GameObject ReserveCharPrefab;
    public GameObject BoardCharPrefab;
    public GameObject InfoPanelPrefab;
    public GameObject OptionButtonPrefab;
    public Transform OptionButtonsParent;
    public GameObject P1ReserveLoc;
    public GameObject P2ReserveLoc;
    public List<ScriptableCharacterBase> P1Characters;
    public List<ScriptableCharacterBase> P2Characters;
    public List<Color> NeededColors;

    //Valid Target Lists
    public List<Creature> ValidAttackTargets = new List<Creature>();
    public List<Creature> ValidCreatureAbilityTargets = new List<Creature>();
    public List<GridSpace> ValidPointAbilityTargets = new List<GridSpace>();

    //Current Selections
    public Creature CurSelectedCreat;
    private GameObject CurInfoPanel;
    internal Ability CurrentChoiceMakingAbility = null;

    //Reserve-related variables
    private int CurP1ReserveOffset = 0;
    private int CurP2ReserveOffset = 0;
    private List<GameObject> P1Reserves = new List<GameObject>();
    private List<GameObject> P2Reserves = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MyHighlightManager = new HighlightManager(this);
        InitializeGame();
        InitializeUI();
        SubscribeToEvents();
        InitializeGameCharacters();

        OnStartOfTurn(this, new TurnStartArgs { PlayerWhoseTurnIsStarting = 0 });
    }

    private void InitializeGame()
    {
        MyGame = TestProperty.SetupBasicTestGame(9, 0);
        InitDemoPlayers();
        MyGame.Init(false);
    }

    private void InitializeGameCharacters()
    {
        foreach (var charBase in P1Characters)
        {
            var creat = new Creature();
            creat.InitFromBase(ScriptableCreatureConverter.GameBaseFromScriptableCharacter(charBase));
            MyGame.Players[0].PutInReserve(creat);
        }
        foreach (var charBase in P2Characters)
        {
            var creat = new Creature();
            creat.InitFromBase(ScriptableCreatureConverter.GameBaseFromScriptableCharacter(charBase));
            MyGame.Players[1].PutInReserve(creat);
        }

        if (TestGame)
        {
            foreach (var creat in MyGame.AllCreatures)
            {
                creat.Initiative = 1;
                foreach (var abil in creat.Abilities)
                {
                    if(abil is ActiveAbility active)
                    {
                        active.Cooldown = 0;
                    }
                }
            }
        }
    }

    private void InitializeUI()
    {
        var uiManager = GetComponent<GameDemoUIManager>();
        uiManager.InitUIManagement(MyGame);

        foreach (var gSquare in MyGame.GameGrid.GetAllGridSquares())
        {
            var space = Instantiate(gridSquarePrefab, new Vector3(gSquare.XPos, gSquare.YPos, 0), Quaternion.identity);
            var gameDemoProp = space.GetComponent<GameDemoSquare>();
            GridObjs.Add(gSquare, (space, gameDemoProp));
            gameDemoProp.BaseColor = NeededColors[(int)ColorIndex.BASE_COLOR];
            gameDemoProp.IsPathColor = NeededColors[(int)ColorIndex.GRID_PATH_COLOR];
            gameDemoProp.HighlightColor = NeededColors[(int)ColorIndex.GRID_HIGHLIGHT_COLOR];
            gameDemoProp.MyGridSpace = gSquare;
            gameDemoProp.MyGameDemo = this;
        }

        ResetTilesToBase();
    }

    private void SubscribeToEvents()
    {
        //EventManager.StartListening("CreatureSummoned", OnCreatureSummon);
        EventManager.StartListening(GachaEventType.CreatureReserved, OnCreatureReserve, UI_EVENT_PRIORITY);
        EventManager.StartListening(GachaEventType.CreatureLeavesReserve, OnCreatureLeavesReserve, UI_EVENT_PRIORITY);
        EventManager.StartListening(GachaEventType.CreatureEntersSpace, OnCreatureEntersSpace, UI_EVENT_PRIORITY);
        EventManager.StartListening(GachaEventType.StartOfTurn, OnStartOfTurn, UI_EVENT_PRIORITY);
        EventManager.StartListening(GachaEventType.AfterCreatureDies, OnCreatureDies, UI_EVENT_PRIORITY);
        EventManager.StartListening(GachaEventType.CreatureRemoved, OnCreatureDies, UI_EVENT_PRIORITY);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DecreaseSelectState();
        }
    }

    public void DecreaseSelectState()
    {
        if (MySelectState == GameDemoSelectState.BASICSELECT)
        {
            PotentialDeselect();
        }
        else if (MySelectState == GameDemoSelectState.TARGETSELECT)
        {
            ClearAllTargets(true);
        }
    }

    public void SelReserveChar(GameObject target)
    {
        PotentialDeselect(false);
        var targetCreat = target.GetComponent<GameDemoReserveChar>().MyCreature;
        UpdateOrInstantiateInfoPanel(targetCreat, target.transform.position);
        CurSelectedCreat = targetCreat;
        MySelectState = GameDemoSelectState.BASICSELECT;
    }

    public void SelOnboardChar(GameObject target)
    {
        // Get the GameDemoBoardChar component from the target GameObject
        GameDemoBoardChar boardCharComp = target.GetComponent<GameDemoBoardChar>();

        PotentialDeselect(false);
        MySelectState = GameDemoSelectState.BASICSELECT;
        CurSelectedCreat = boardCharComp.MyCreature;

        // Check if the character belongs to the current player
        if (boardCharComp.MyCreature.Controller == MyGame.Players[MyGame.CurrentPlayerIndex])
        {
            UpdateOrInstantiateInfoPanel(boardCharComp.MyCreature, target.transform.position);

            boardCharComp.Select();
            HighlightBasicActions(boardCharComp);
        }
        else
        {
            // Otherwise, select without allowing interaction.
            UpdateOrInstantiateInfoPanel(boardCharComp.MyCreature, target.transform.position, false);
            boardCharComp.Select();
        }
    }

    internal void HighlightBasicActions(GameDemoBoardChar boardCharComp)
    {
        MyHighlightManager.HighlightValidMoves(CurSelectedCreat);
        if (boardCharComp.MyCreature.CanBasicAttack)
        {
            ValidAttackTargets = boardCharComp.MyCreature.GetValidBasicAttackTargets();
            MyHighlightManager.HighlightValidAttackTargets();
        }
    }

    public void PotentialDeselect(bool destroyInfoPanel = true)
    {
        if(CurSelectedCreat != null)
        {
            if (CreatObjs.TryGetValue(CurSelectedCreat, out GameObject gameObj))
            {
                gameObj.GetComponent<GameDemoBoardChar>()?.Deselect();
                gameObj.GetComponent<GameDemoReserveChar>()?.Deselect();

                if(gameObj.GetComponent<GameDemoBoardChar>() != null)
                {
                    ResetTilesToBase();
                }
            }

            if (destroyInfoPanel)
            {
                MySelectState = GameDemoSelectState.UNSELECTED;
            }

            ClearSelChar(destroyInfoPanel);

        }
        ClearAllTargets(true);
    }

    public void EndTurn()
    {
        MyGame.EndTurn();
        PotentialDeselect();
    }

    public void ClearAllTargets(bool cancelAbil)
    {
        ClearAttackTargets();
        ClearCreatureAbilityTargets(false);
        ClearPointAbilityTargets(false);
        ClearOptionAbilityTargets(false);
        if (CurSelectedCreat != null)
        {
            HighlightBasicActions(GetOnboardComponent(CurSelectedCreat));
            MySelectState = GameDemoSelectState.BASICSELECT;
        }

        PotentiallyCancelActiveAbility(cancelAbil);
    }

    public List<GridSpace> GetPathTo(GridSpace space)
    {
        return curValidMoveDict != null ? curValidMoveDict[space] : null;
    }

    public void AttackTarget(Creature target)
    {
        if (CurSelectedCreat != null && ValidAttackTargets.Contains(target))
        {
            CurSelectedCreat.BasicAttack(target);
            PotentialDeselect();
        }
    }

    public void ClearCreatureAbilityTargets(bool cancelAbility = true)
    {
        MyHighlightManager.ClearCreatureAbilityTargetHighlights();

        ValidCreatureAbilityTargets.Clear();

        PotentiallyCancelActiveAbility(cancelAbility);
    }

    public void ClearPointAbilityTargets(bool cancelAbility = true)
    {
        MyHighlightManager.ClearPointAbilityTargetHighlights();

        ValidPointAbilityTargets.Clear();

        PotentiallyCancelActiveAbility(cancelAbility);
    }

    public void ClearOptionAbilityTargets(bool cancelAbility = true)
    {
        foreach (Transform child in OptionButtonsParent)
        {
            Destroy(child.gameObject);
        }

        PotentiallyCancelActiveAbility(cancelAbility);
    }

    public void TriggerCreatureAbil(Creature target)
    {
        if (MyGame.CurrentPlayer is DemoPlayer dPlayer)
        {
            dPlayer.SelectCreatureTarget(target);
        }
    }

    public void TriggerPointAbil(GridSpace target)
    {
        if (MyGame.CurrentPlayer is DemoPlayer dPlayer)
        {
            dPlayer.SelectPointTarget(target);
        }
    }

    public void DisplayOptionButtons(List<ChoiceOption> options, Action<ChoiceOption> onOptionSelected)
    {
        for (int i = 0; i < options.Count; i++)
        {
            var option = options[i];
            GameObject button = CreateOptionButton(option, onOptionSelected);

            // Adjust the position of the button based on its index
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startingPosition.x, startingPosition.y - i * verticalSpacing);
        }
    }

    private GameObject CreateOptionButton(ChoiceOption option, Action<ChoiceOption> onOptionSelected)
    {
        GameObject button = Instantiate(OptionButtonPrefab);

        // Set the button's text to the option
        button.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;

        // Attach an event listener to the button
        button.GetComponent<Button>().onClick.AddListener(() => onOptionSelected(option));

        // Add the button to the canvas or any other parent object
        button.transform.SetParent(OptionButtonsParent, false);

        return button;
    }

    

    public void PotentiallyCancelActiveAbility(bool cancelAbility)
    {
        if (cancelAbility)
        {
            
            var prevCurAbil = CurrentChoiceMakingAbility;

            CurrentChoiceMakingAbility = null;

            ClearActivePlayerChoices();
            if (prevCurAbil != null && prevCurAbil is ActiveAbility activeAbil)
            {
                activeAbil.CancelActivation();
            }
        }
    }

    public void ClearActivePlayerChoices()
    {
        if (MyGame.CurrentPlayer is DemoPlayer dPlayer)
        {
            dPlayer.ClearChoices();
        }
    }

    private void ClearAttackTargets()
    {
        MyHighlightManager.ClearAttackTargetHighlights();
        ValidAttackTargets.Clear();
    }

    private void ClearSelChar(bool destroyInfoPanel)
    {
        if (destroyInfoPanel && CurInfoPanel != null)
        {
            Destroy(CurInfoPanel);
            CurInfoPanel = null;
        }
        CurSelectedCreat = null;
    }

    private void UpdateOrInstantiateInfoPanel(Creature targetCreat, Vector3 position, bool allowAbilities = true)
    {
        if (CurInfoPanel != null)
        {
            CurInfoPanel.GetComponent<BaseInfoPanel>().SetCreature(targetCreat, !allowAbilities);
        }
        else
        {
            var infoPanel = Instantiate(InfoPanelPrefab, position - new Vector3(0, 0, 4), Quaternion.identity);
            infoPanel.GetComponent<BaseInfoPanel>().MyGameDemo = this;
            infoPanel.GetComponent<BaseInfoPanel>().SetCreature(targetCreat, !allowAbilities);
            CurInfoPanel = infoPanel;
        }
    }

    private void InstantiateBoardCharacter(CreatureSpaceArgs cArgs)
    {
        var boardCharacter = CreateBoardCharacter(new Vector3(cArgs.SpaceInvolved.XPos, cArgs.SpaceInvolved.YPos, 0), Quaternion.identity);
        SetupBoardCharacterProperties(boardCharacter, cArgs);
        if (CreatObjs.ContainsKey(cArgs.MyCreature))
        {
            CreatObjs[cArgs.MyCreature] = boardCharacter;
        }
        else
        {
            CreatObjs.Add(cArgs.MyCreature, boardCharacter);
        }
    }

    private GameObject CreateBoardCharacter(Vector3 position, Quaternion rotation)
    {
        return Instantiate(BoardCharPrefab, position, rotation);
        // Version that parents this to the board char obj, not sure if should use or not.
        //return Instantiate(BoardCharPrefab, position, rotation, transform);
    }

    private void SetupBoardCharacterProperties(GameObject boardCharacter, CreatureSpaceArgs cArgs)
    {
        var gameComp = boardCharacter.GetComponent<GameDemoBoardChar>();
        gameComp.SetCreat(cArgs.MyCreature);
        gameComp.MyGameDemo = this;
        gameComp.attackTargetColor = NeededColors[(int)ColorIndex.VALID_ATTACK_TARGET_COLOR];
        gameComp.defaultColor = cArgs.MyCreature.Controller.MyPlayerIndex == 0 ? NeededColors[(int)ColorIndex.P1_COLOR] : NeededColors[(int)ColorIndex.P2_COLOR];
        
    }

    private void InitDemoPlayers()
    {
        var pList = new List<DemoPlayer>();
        // For tracking player index while they're initialized.
        var count = 0;
        foreach (var pArgs in MyGame.MyGameArgs.Players)
        {
            var Player = new DemoPlayer(new PlayerArgs() { type = pArgs.type }) { MyPlayerIndex = count, };
            //Player.Creatures.AddRange(pArgs.startingCreatures);
            Player.MyGame = MyGame;
            Player.MyGameDemo = this;
            foreach (var creature in pArgs.startingCreatures)
            {
                creature.Controller = Player;
                Player.PutInReserve(creature);
                MyGame.AllCreatures.Add(creature);
            }
            pList.Add(Player);
            count++;
        }
        MyGame.Players = pList.ToArray();
    }

    private void OnCreatureEntersSpace(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs cArgs)
        {
            if(CreatObjs.TryGetValue(cArgs.MyCreature, out GameObject gameObj) && gameObj.GetComponent<GameDemoBoardChar>() != null)
            {
                gameObj.transform.position = new Vector3(cArgs.SpaceInvolved.XPos, cArgs.SpaceInvolved.YPos, 0);
                if (CurSelectedCreat == cArgs.MyCreature && MySelectState == GameDemoSelectState.BASICSELECT)
                {
                    ResetTilesToBase();
                    ClearAttackTargets();
                    HighlightBasicActions(gameObj.GetComponent<GameDemoBoardChar>());
                    //SelOnboardChar(gameObj);
                }
            }
            else
            {
                InstantiateBoardCharacter(cArgs);
            }
        }
    }

    private void OnCreatureReserve(object sender, EventArgs e)
    {
        if (e is CreatureReservedArgs cArgs)
        {
            if (CreatObjs.ContainsKey(cArgs.BeingReserved))
            {
                RemoveFromField(cArgs.BeingReserved);
            }

            var offsetAmount = cArgs.ReserveOwner.MyPlayerIndex == 0 ? CurP1ReserveOffset = CurP1ReserveOffset + RESERVE_OFFSET_AMOUNT : CurP2ReserveOffset = CurP2ReserveOffset + RESERVE_OFFSET_AMOUNT;
            var locForReserve = (cArgs.ReserveOwner.MyPlayerIndex == 0 ? P1ReserveLoc.transform.position : P2ReserveLoc.transform.position) + new Vector3(0, offsetAmount, 0);
            var creat = Instantiate(ReserveCharPrefab, locForReserve, Quaternion.Euler(270, 0, 0));
            if (cArgs.ReserveOwner.MyPlayerIndex == 0)
            {
                P1Reserves.Add(creat);
            }
            else
            {
                P2Reserves.Add(creat);
            }

            if(MyGame.CurrentPlayerIndex != cArgs.ReserveOwner.MyPlayerIndex)
            {
                creat.SetActive(false);
            }

            var gameComp = creat.GetComponent<GameDemoReserveChar>();
            gameComp.SetCreat(cArgs.BeingReserved);
            CreatObjs.Add(cArgs.BeingReserved, creat);
            gameComp.MyGameDemo = this;
        }
    }

    private void OnCreatureLeavesReserve(object sender, EventArgs e)
    {
        if (e is CreatureReservedArgs cArgs)
        {
            var targetList = cArgs.ReserveOwner.MyPlayerIndex == 0 ? P1Reserves : P2Reserves;
            var targetObj = GetReserve(cArgs.BeingReserved);
            if (targetObj != null)
            {
                for (int i = targetList.IndexOf(targetObj); i < targetList.Count; i++)
                {
                    targetList[i].transform.position -= new Vector3(0, RESERVE_OFFSET_AMOUNT, 0);
                }

                if(cArgs.ReserveOwner.MyPlayerIndex == 0)
                {
                    CurP1ReserveOffset -= RESERVE_OFFSET_AMOUNT;
                }
                else
                {
                    CurP2ReserveOffset -= RESERVE_OFFSET_AMOUNT;
                }

                targetList.Remove(targetObj);
                if(CurSelectedCreat != null && CurSelectedCreat == cArgs.BeingReserved)
                {
                    PotentialDeselect();
                }

                Destroy(targetObj);
            }
        }
    }

    public void OnCreatureDies(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs)
        {
            RemoveFromField(dieArgs.CreatureDied);
        }
    }

    private void RemoveFromField(Creature c)
    {
        if (CurSelectedCreat == c)
        {
            PotentialDeselect();
        }

        if (CreatObjs.ContainsKey(c))
        {
            var cObj = CreatObjs[c];
            CreatObjs.Remove(c);
            Destroy(cObj);
        }
    }

    private void OnStartOfTurn(object sender, EventArgs e)
    {
        if (e is TurnStartArgs startArgs)
        {
            var curPlayerList = startArgs.PlayerWhoseTurnIsStarting == 0 ? P1Reserves : P2Reserves;
            var otherPlayerList = startArgs.PlayerWhoseTurnIsStarting == 0 ? P2Reserves : P1Reserves;
            foreach (var reserveItem in curPlayerList)
            {
                reserveItem.SetActive(true);
            }

            foreach (var reserveItem in otherPlayerList)
            {
                reserveItem.SetActive(false);
            }
        }
    }

    // UTILITY METHODS
    public void ResetTilesToBase()
    {
        foreach (var gSquare in MyGame.GameGrid.GetAllGridSquares())
        {
            ResetSquareColorToBase(gSquare);
        }
    }

    internal void ResetSquareColorToBase(GridSpace gSquare)
    {
        GridObjs[gSquare].square.UnHighlight();
        if (MyGame.Players[0].ValidInitSpaces.Contains(gSquare))
        {
            SetSquareCol(gSquare, NeededColors[(int)ColorIndex.P1_COLOR]);
        }
        else if (MyGame.Players[1].ValidInitSpaces.Contains(gSquare))
        {
            SetSquareCol(gSquare, NeededColors[(int)ColorIndex.P2_COLOR]);
        }
        else if (gSquare.Obstacle)
        {
            SetSquareCol(gSquare, NeededColors[(int)ColorIndex.OBSTACLE_COLOR]);
        }
        else
        {
            SetSquareCol(gSquare, NeededColors[(int)ColorIndex.BASE_COLOR]);
        }
    }

    private void SetSquareCol(GridSpace gSquare, Color color)
    {
        GridObjs[gSquare].obj.GetComponent<Renderer>().material.color = color;
    }

    public GameDemoBoardChar GetOnboardComponent(Creature creat)
    {
        return CreatObjs.ContainsKey(creat) ? CreatObjs[creat].GetComponent<GameDemoBoardChar>() : null;
    }

    public GameDemoReserveChar GetReserveComponent(Creature creat)
    {
        var targetObj = GetReserve(creat);
        return targetObj.GetComponent<GameDemoReserveChar>();
    }

    public GameObject GetReserve(Creature creat)
    {
        var targetList = creat.Controller.MyPlayerIndex == 0 ? P1Reserves : P2Reserves;
        return targetList.Where(x => x.GetComponent<GameDemoReserveChar>().MyCreature == creat).FirstOrDefault();
    }

    public GameDemoSquare GetBoardSpaceComponent(GridSpace gs)
    {
        return GridObjs.ContainsKey(gs) ? GridObjs[gs].square : null;
    }
}

internal enum ColorIndex
{
    BASE_COLOR,
    P1_COLOR,
    P2_COLOR,
    OBSTACLE_COLOR,
    GRID_HIGHLIGHT_COLOR,
    GRID_PATH_COLOR,
    VALID_ATTACK_TARGET_COLOR,
}

public enum GameDemoSelectState
{
    UNSELECTED,
    BASICSELECT,
    TARGETSELECT,
}
