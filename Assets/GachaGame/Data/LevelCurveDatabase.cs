using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LevelCurveDatabase
{
    public const int ATK_INDEX = 0;
    public const int HEALTH_INDEX = 1;
    public const int SPEED_INDEX = 2;
    public const int INIT_INDEX = 3;

    public static Dictionary<LevelingCurveType, int[][]> LevelCurves;

    private static int[][] GetAttackHeavyLevelCurve()
    {
        int[][] returner = Enumerable.Range(0, 19).Select(_ => new int[4]).ToArray();


        return returner;
    }

}
