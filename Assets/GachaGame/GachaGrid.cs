using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaGrid
{
    private Dictionary<(int, int), GridSpace> GridSquares;

    public GachaGrid(int width, int height)
    {
        Width = width;
        Height = height;
        GridSquares = new Dictionary<(int, int), GridSpace>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var gs = new GridSpace() { XPos = i, YPos = j };
                Add((i, j), gs);
            }
        }
    }

    public Dictionary<(int, int), GridSpace> Grid => GridSquares;

    public int Width { get; private set; }

    public int Height { get; private set; }

    public List<GridSpace> GetAllGridSquares()
    {
        var returner = new List<GridSpace>();
        foreach (var space in GridSquares.Values)
        {
            returner.Add(space);
        }
        return returner;
    }

    public void TeleportTo(Creature teleporter, GridSpace destination)
    {
        CreatureLeavesSpace(teleporter);
        CreatureEntersSpace(teleporter, destination);
        
        //INVOKE SOME KIND OF TELEPORTEDEVENT MAYBE?
    }

    public void Move(Creature mover, List<GridSpace> path, GridSpace destination)
    {
        CreatureLeavesSpace(mover);
        foreach (var space in path)
        {
            // ALL SPACES CREATURE MOVES THROUGH ITERATED HERE
            // Probably invoke some "Moved through" event.
        }
        mover.SpeedLeft -= path.Count - 1;
        CreatureEntersSpace(mover, destination);
    }

    public void CreatureLeavesSpace(Creature leaver)
    {
        if(leaver.MySpace != null)
        {
            EventManager.Invoke("CreatureLeavesSpace", this, new CreatureSpaceArgs() { MyCreature = leaver, SpaceInvolved = leaver.MySpace });
            leaver.MySpace.Occupant = null;
            leaver.MySpace = null;
        }
    }

    public void CreatureEntersSpace(Creature enterer, GridSpace targetSpace)
    {
        enterer.MySpace = targetSpace;
        targetSpace.Occupant = enterer;
        EventManager.Invoke("CreatureEntersSpace", this, new CreatureSpaceArgs() { MyCreature = enterer, SpaceInvolved = targetSpace });
    }

    public static bool InitSquareAvailable(Player p)
    {
        foreach (var sq in p.ValidInitSpaces)
        {
            if (!sq.isBlocked)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsInRange(Creature creat, GridSpace target, int range) => creat.IsOnBoard && IsInRange(creat.MySpace, target, range);

    public static bool IsInRange(Creature from, Creature to, int range) 
    {
        return from.IsOnBoard && to.IsOnBoard && IsInRange(from.MySpace, to.MySpace, range);
    }

    public static bool IsInRange(GridSpace from, GridSpace to, int range)
    {
        return Mathf.Abs(to.XPos - from.XPos) + Mathf.Abs(to.YPos - from.YPos) <= range;
    }

    public void Add((int, int) pos, GridSpace gs)
    {
        GridSquares.Add(pos, gs);
    }

    public List<GridSpace> GetAdjacents(GridSpace gs, bool includeDiagonals)
    {
        var ret = new List<GridSpace>();
        bool leftAvailable = gs.XPos > 0;
        bool rightAvailable = gs.XPos < Width - 1;

        if (leftAvailable)
        {
            ret.Add(this[(gs.XPos - 1, gs.YPos)]);
        }

        if(rightAvailable)
        {
            ret.Add(this[(gs.XPos + 1, gs.YPos)]);
        }

        if (gs.YPos > 0)
        {
            ret.Add(this[(gs.XPos, gs.YPos - 1)]);
            if (includeDiagonals)
            {
                if (leftAvailable)
                {
                    ret.Add(this[(gs.XPos - 1, gs.YPos - 1)]);
                }

                if (rightAvailable)
                {
                    ret.Add(this[(gs.XPos + 1, gs.YPos - 1)]);
                }
            }
        }

        if (gs.YPos < Height - 1)
        {
            ret.Add(this[(gs.XPos, gs.YPos + 1)]);
            if (includeDiagonals)
            {
                if (leftAvailable)
                {
                    ret.Add(this[(gs.XPos - 1, gs.YPos + 1)]);
                }

                if (rightAvailable)
                {
                    ret.Add(this[(gs.XPos + 1, gs.YPos + 1)]);
                }
            }
        }

        return ret;
    }

    public List<GridSpace> GetUnblockedAdjacents(GridSpace gs, bool includeDiagonals)
    {
        var ret = GetAdjacents(gs, includeDiagonals);
        var toRemove = new List<GridSpace>();

        foreach (var space in ret)
        {
            if(space.isBlocked)
            {
                toRemove.Add(space);
            }
        }

        foreach (var space in toRemove)
        {
            ret.Remove(space);
        }

        return ret;
    }

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(Creature c) => GetValidMoves(c.MySpace, c.SpeedLeft, c.HasTag(CreatureTag.FREEMOVER));

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(int x, int y, int moveSpeed) => GetValidMoves(this[(x, y)], moveSpeed, false);

    public GridSpace this[(int, int) x] => GridSquares[x];

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(GridSpace startSpace, int speed, bool includeDiagonals)
    {
        // A dictionary to store paths to reachable spaces.
        Dictionary<GridSpace, List<GridSpace>> reachablePaths = new Dictionary<GridSpace, List<GridSpace>>();

        // A queue for the BFS.
        Queue<GridSpace> queue = new Queue<GridSpace>();

        // A dictionary to store the number of moves used to reach each space.
        Dictionary<GridSpace, int> distance = new Dictionary<GridSpace, int>();

        // Start the BFS from the startSpace with 0 distance.
        queue.Enqueue(startSpace);
        distance[startSpace] = 0;

        // The path to the startSpace is just the startSpace itself.
        reachablePaths[startSpace] = new List<GridSpace> { startSpace };

        while (queue.Count > 0)
        {
            // Get the next space from the queue.
            GridSpace space = queue.Dequeue();

            // Get the distance to this space.
            int dist = distance[space];

            // If we have used all our moves, don't look at adjacent spaces.
            if (dist >= speed)
            {
                continue;
            }

            // Get the unblocked adjacent spaces.
            List<GridSpace> adjacents = GetUnblockedAdjacents(space);

            foreach (GridSpace adjacent in adjacents)
            {
                // If we haven't visited this adjacent space yet, add it to the queue and
                // update the distance and path.
                if (!distance.ContainsKey(adjacent))
                {
                    queue.Enqueue(adjacent);
                    distance[adjacent] = dist + 1;

                    // The path to the adjacent space is the path to the current space,
                    // plus the adjacent space itself. We create a new list to avoid
                    // modifying the path to the current space.
                    List<GridSpace> pathToAdjacent = new List<GridSpace>(reachablePaths[space]);
                    pathToAdjacent.Add(adjacent);
                    reachablePaths[adjacent] = pathToAdjacent;
                }
            }
        }

        // Return the paths to the reachable spaces.
        return reachablePaths;
    }

}
