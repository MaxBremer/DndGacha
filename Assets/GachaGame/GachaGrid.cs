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

    public bool AnyUnblockedSpacesExist => GetAllGridSquares().Where(x => !x.isBlocked).Any();

    public bool NumUnblockedSpacesExist(int numNeeded) => GetAllGridSquares().Where(x => !x.isBlocked).Count() >= numNeeded;

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
        var startSpace = path[0];
        foreach (var space in path)
        {
            // ALL SPACES CREATURE MOVES THROUGH ITERATED HERE
            // Probably invoke some "Moved through" event.
            if(space != startSpace)
            {
                mover.SpeedLeft -= space.GetSpeedWeight(mover);
            }
        }
        //mover.SpeedLeft -= path.Count - 1;
        CreatureEntersSpace(mover, destination);
        EventManager.Invoke(GachaEventType.CreatureMoved, mover, new System.EventArgs());
    }

    public void CreatureLeavesSpace(Creature leaver)
    {
        if(leaver.MySpace != null)
        {
            var leaveArgs = new CreatureSpaceArgs() { MyCreature = leaver, SpaceInvolved = leaver.MySpace };
            leaver.MySpace.Occupant = null;
            leaver.MySpace = null;
            EventManager.Invoke(GachaEventType.CreatureLeavesSpace, this, leaveArgs);
        }
    }

    public void CreatureEntersSpace(Creature enterer, GridSpace targetSpace)
    {
        enterer.MySpace = targetSpace;
        targetSpace.Occupant = enterer;
        EventManager.Invoke(GachaEventType.CreatureEntersSpace, this, new CreatureSpaceArgs() { MyCreature = enterer, SpaceInvolved = targetSpace });
    }

    public List<GridSpace> GetSpacesInRange(GridSpace targetSpace, int range, bool useDiagonals)
    {
        var ret = new List<GridSpace>();

        ret.AddRange(GetAllGridSquares().Where(x => useDiagonals ? IsInRangeDiags(targetSpace, x, range) : IsInRange(targetSpace, x, range)));
        Debug.Log("num in range: " + ret.Count());
        return ret;
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

    public static bool IsInRangeDiags(GridSpace from, GridSpace to, int range)
    {
        return Mathf.Max(Mathf.Abs(to.XPos - from.XPos), Mathf.Abs(to.YPos - from.YPos)) <= range;
    }

    public static int DistanceBetween(Creature from, Creature to)
    {
        if((!from.IsOnBoard) || (!to.IsOnBoard))
        {
            return -1;
        }
        return DistanceBetween(from.MySpace, to.MySpace);
    }

    public static int DistanceBetween(GridSpace from, GridSpace to) => Mathf.Abs(to.XPos - from.XPos) + Mathf.Abs(to.YPos - from.YPos);

    public GridSpace GetRandomAdjacent(GridSpace point, bool onlyUnblocked = false, bool includeDiagonals = false)
    {
        List<GridSpace> options = onlyUnblocked ? GetUnblockedAdjacents(point, includeDiagonals) : GetAdjacents(point, includeDiagonals);
        return options.Count > 0 ? options[Random.Range(0, options.Count - 1)] : null;
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

    public bool UnblockedAdjacentsExist(GridSpace gs, bool includeDiagonals)
    {
        return GetUnblockedAdjacents(gs, includeDiagonals).Count > 0;
    }

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(Creature c) 
    {
        if (c.HasTag(CreatureTag.SNOOZING) || c.HasTag(CreatureTag.CANT_MOVE))
        {
            return new Dictionary<GridSpace, List<GridSpace>>();
        }
        var potentialMoves = GetValidMoves(c.MySpace, c.SpeedLeft, c.HasTag(CreatureTag.FREEMOVER), c);

        EventManager.Invoke(GachaEventType.CreatureMovesFound, this, new ValidMovesFoundForCreatArgs() { ValidMovesWithPaths = potentialMoves, CreatureMoving = c });

        return potentialMoves;
    }

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(int x, int y, int moveSpeed) => GetValidMoves(this[(x, y)], moveSpeed, false, null);

    public GridSpace this[(int, int) x] => GridSquares[x];

    public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(GridSpace startSpace, int speed, bool includeDiagonals, Creature creatMoving)
    {
        // A dictionary to store paths to reachable spaces.
        Dictionary<GridSpace, List<GridSpace>> reachablePaths = new Dictionary<GridSpace, List<GridSpace>>();

        // A queue for the BFS.
        Queue<GridSpace> queue = new Queue<GridSpace>();

        // A dictionary to store the movement cost used to reach each space.
        Dictionary<GridSpace, int> movementCost = new Dictionary<GridSpace, int>();

        // Start the BFS from the startSpace with 0 cost.
        queue.Enqueue(startSpace);
        movementCost[startSpace] = 0;

        // The path to the startSpace is just the startSpace itself.
        reachablePaths[startSpace] = new List<GridSpace> { startSpace };

        while (queue.Count > 0)
        {
            // Get the next space from the queue.
            GridSpace space = queue.Dequeue();

            // Get the cost to reach this space.
            int costToSpace = movementCost[space];

            // Get the unblocked adjacent spaces.
            List<GridSpace> adjacents = GetUnblockedAdjacents(space, includeDiagonals);

            foreach (GridSpace adjacent in adjacents)
            {
                // Calculate the cost to move to the adjacent space.
                int costToAdjacent = costToSpace + adjacent.GetSpeedWeight(creatMoving);

                // If the cost to move to the adjacent space is within our speed limit,
                // and we haven't visited this adjacent space yet, or we found a cheaper path to it,
                // then add it to the queue and update the movement cost and path.
                if (costToAdjacent <= speed && (!movementCost.ContainsKey(adjacent) || costToAdjacent < movementCost[adjacent]))
                {
                    queue.Enqueue(adjacent);
                    movementCost[adjacent] = costToAdjacent;

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

    public GridSpace GetNearestAvailableSpace(GridSpace startSpace)
    {
        if (!startSpace.isBlocked)
        {
            return startSpace;
        }

        var visited = new HashSet<GridSpace>();
        var queue = new Queue<GridSpace>();
        queue.Enqueue(startSpace);
        visited.Add(startSpace);

        List<GridSpace> potentialSpaces = new List<GridSpace>();

        while (queue.Count > 0)
        {
            int levelSize = queue.Count; // Number of spaces at the current distance
            for (int i = 0; i < levelSize; i++)
            {
                var currentSpace = queue.Dequeue();
                var adjacents = GetAdjacents(currentSpace, false); // Get adjacent spaces without diagonals

                foreach (var adjacent in adjacents)
                {
                    if (!visited.Contains(adjacent))
                    {
                        visited.Add(adjacent);
                        queue.Enqueue(adjacent);

                        if (!adjacent.isBlocked)
                        {
                            potentialSpaces.Add(adjacent);
                        }
                    }
                }
            }

            // If we found any unblocked spaces at this distance, return one at random
            if (potentialSpaces.Any())
            {
                return potentialSpaces[UnityEngine.Random.Range(0, potentialSpaces.Count)];
            }
        }

        // If we run out of candidates, return null
        return null;
    }



    /*public Dictionary<GridSpace, List<GridSpace>> GetValidMoves(GridSpace startSpace, int speed, bool includeDiagonals)
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
            List<GridSpace> adjacents = GetUnblockedAdjacents(space, includeDiagonals);

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
    }*/

}
