using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDemo : MonoBehaviour
{
    private Dictionary<GridSpace, (GameObject obj, GameDemoSquare square)> GridObjs = new Dictionary<GridSpace, (GameObject obj, GameDemoSquare square)>();
    private Dictionary<Creature, GameObject> CreatObjs = new Dictionary<Creature, GameObject>();
    private const int BASE_COLOR = 0;
    private const int P1_COLOR = 1;
    private const int P2_COLOR = 2;
    private const int OBSTACLE_COLOR = 3;
    private const int GRID_HIGHLIGHT_COLOR = 4;
    private const int GRID_DEFAULT_COLOR = 5;

    public Game MyGame;

    public GameObject gridSquareObj;
    public GameObject ReserveCharObj;
    public GameObject BoardCharObj;
    public GameObject InfoPanelObj;

    public Creature CurSelectedCreat;
    private GameObject CurInfoPanel;

    public GameObject P1ReserveLoc;
    public GameObject P2ReserveLoc;
    private int CurP1Offset = 0;
    private int CurP2Offset = 0;
    private List<GameObject> P1Reserves = new List<GameObject>();
    private List<GameObject> P2Reserves = new List<GameObject>();

    private Dictionary<GridSpace, List<GridSpace>> curValidMoveDict = null;

    private const int RESERVE_OFFSET_AMOUNT = 1;

    public List<ScriptableCharacterBase> P1Characters;
    public List<ScriptableCharacterBase> P2Characters;
    public List<Color> NeededColors;

    // Start is called before the first frame update
    void Start()
    {
        MyGame = TestProperty.SetupBasicTestGame(9, 0);
        MyGame.Init();

        //EventManager.StartListening("CreatureSummoned", OnCreatureSummon);
        EventManager.StartListening("CreatureReserved", OnCreatureReserve);
        EventManager.StartListening("CreatureLeavesReserve", OnCreatureLeavesReserve);
        EventManager.StartListening("CreatureEntersSpace", OnCreatureEntersSpace);
        EventManager.StartListening("StartOfTurn", OnStartOfTurn);

        var uiManager = GetComponent<GameDemoUIManager>();
        uiManager.InitUIManagement(MyGame);

        foreach (var gSquare in MyGame.GameGrid.GetAllGridSquares())
        {
            var space = Instantiate(gridSquareObj, new Vector3(gSquare.XPos, gSquare.YPos, 0), Quaternion.identity);
            var gameDemoProp = space.GetComponent<GameDemoSquare>();
            GridObjs.Add(gSquare, (space, gameDemoProp));
            gameDemoProp.MyGridSpace = gSquare;
            gameDemoProp.MyGameDemo = this;
        }

        ResetTilesToBase();

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

        OnStartOfTurn(this, new TurnStartArgs { PlayerWhoseTurnIsStarting = 0 });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PotentialDeselect();
        }
    }

    public void TrySelChar(GameObject obj)
    {

    }

    public void SelReserveChar(GameObject target)
    {
        PotentialDeselect(false);
        var targetCreat = target.GetComponent<GameDemoReserveChar>().MyCreature;
        UpdateOrInstantiateInfoPanel(targetCreat, target.transform.position);
        CurSelectedCreat = targetCreat;
    }

    public void SelOnboardChar(GameObject target)
    {
        // Deselect any currently selected character
        PotentialDeselect(false);

        // Get the GameDemoBoardChar component from the target GameObject
        GameDemoBoardChar boardCharComp = target.GetComponent<GameDemoBoardChar>();

        // Check if the character belongs to the current player
        if (boardCharComp.MyCreature.Controller == MyGame.Players[MyGame.CurrentPlayerIndex])
        {
            // Instantiate the InfoPanelObj to display the character's information
            UpdateOrInstantiateInfoPanel(boardCharComp.MyCreature, target.transform.position);

            // Set the CurSelectedCreat to the instantiated InfoPanelObj
            CurSelectedCreat = boardCharComp.MyCreature;
            Debug.Log("Pre-call");
            HighlightValidMoves(CurSelectedCreat);
        }
    }

    private void HighlightValidMoves(Creature targetCreat)
    {
        if(targetCreat.SpeedLeft > 0)
        {
            curValidMoveDict = MyGame.GameGrid.GetValidMoves(targetCreat);
            foreach (var gSpace in curValidMoveDict.Keys)
            {
                GridObjs[gSpace].square.Highlight(NeededColors[GRID_HIGHLIGHT_COLOR]);
            }
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

            ClearSelChar(destroyInfoPanel);
        }
    }

    public void EndTurn()
    {
        MyGame.EndTurn();
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

    private void UpdateOrInstantiateInfoPanel(Creature targetCreat, Vector3 position)
    {
        if (CurInfoPanel != null)
        {
            CurInfoPanel.GetComponent<BaseInfoPanel>().SetCreature(targetCreat);
        }
        else
        {
            var infoPanel = Instantiate(InfoPanelObj, position - new Vector3(0, 0, 4), Quaternion.identity);
            infoPanel.GetComponent<BaseInfoPanel>().SetCreature(targetCreat);
            CurInfoPanel = infoPanel;
        }
    }

    private void ResetTilesToBase()
    {
        foreach (var gSquare in MyGame.GameGrid.GetAllGridSquares())
        {
            if (MyGame.Players[0].ValidInitSpaces.Contains(gSquare))
            {
                SetSquareCol(gSquare, NeededColors[P1_COLOR]);
            }
            else if (MyGame.Players[1].ValidInitSpaces.Contains(gSquare))
            {
                SetSquareCol(gSquare, NeededColors[P2_COLOR]);
            }
            else if (gSquare.Obstacle)
            {
                SetSquareCol(gSquare, NeededColors[OBSTACLE_COLOR]);
            }
            else
            {
                SetSquareCol(gSquare, NeededColors[BASE_COLOR]);
            }
        }
    }

    private void SetSquareCol(GridSpace gSquare, Color color)
    {
        GridObjs[gSquare].obj.GetComponent<Renderer>().material.color = color;
    }

    /*private void OnCreatureSummon(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs cArgs) {
            var creat = Instantiate(BoardCharObj, new Vector3(cArgs.LocationOfSummon.XPos, cArgs.LocationOfSummon.YPos, 0), Quaternion.identity);
            var gameComp = creat.GetComponent<GameDemoBoardChar>();
            gameComp.MyCreature = cArgs.BeingSummoned;
        }
    }*/
    private void InstantiateBoardCharacter(CreatureSpaceArgs cArgs)
    {
        var creat = Instantiate(BoardCharObj, new Vector3(cArgs.SpaceInvolved.XPos, cArgs.SpaceInvolved.YPos, 0), Quaternion.identity);
        var gameComp = creat.GetComponent<GameDemoBoardChar>();
        gameComp.SetCreat(cArgs.MyCreature);
        gameComp.MyGameDemo = this;
        if (CreatObjs.ContainsKey(cArgs.MyCreature))
        {
            CreatObjs[cArgs.MyCreature] = creat;
        }
        else
        {
            CreatObjs.Add(cArgs.MyCreature, creat);
        }
    }

    private void OnCreatureEntersSpace(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs cArgs)
        {
            if(CreatObjs.TryGetValue(cArgs.MyCreature, out GameObject gameObj) && gameObj.GetComponent<GameDemoBoardChar>() != null)
            {
                gameObj.transform.position = new Vector3(cArgs.SpaceInvolved.XPos, cArgs.SpaceInvolved.YPos, 0);
            }
            else
            {
                InstantiateBoardCharacter(cArgs);
            }
        }
    }

    private void OnCreatureLeavesSpace(object sender, EventArgs e)
    {

    }

    private void OnCreatureReserve(object sender, EventArgs e)
    {
        if (e is CreatureReservedArgs cArgs)
        {
            var offsetAmount = cArgs.ReserveOwner.MyPlayerIndex == 0 ? CurP1Offset = CurP1Offset + RESERVE_OFFSET_AMOUNT : CurP2Offset = CurP2Offset + RESERVE_OFFSET_AMOUNT;
            var locForReserve = (cArgs.ReserveOwner.MyPlayerIndex == 0 ? P1ReserveLoc.transform.position : P2ReserveLoc.transform.position) + new Vector3(0, offsetAmount, 0);
            var creat = Instantiate(ReserveCharObj, locForReserve, Quaternion.Euler(270, 0, 0));
            if (cArgs.ReserveOwner.MyPlayerIndex == 0)
            {
                P1Reserves.Add(creat);
            }
            else
            {
                P2Reserves.Add(creat);
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
            var targetObj = targetList.Where(x => x.GetComponent<GameDemoReserveChar>().MyCreature == cArgs.BeingReserved).FirstOrDefault();
            if (targetObj != null)
            {
                for (int i = targetList.IndexOf(targetObj); i < targetList.Count; i++)
                {
                    targetList[i].transform.position -= new Vector3(0, RESERVE_OFFSET_AMOUNT, 0);
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
}
