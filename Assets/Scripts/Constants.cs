using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int MOON_X_SIZE = 40;
    public const int MOON_Y_SIZE = 40;

    public const int FULL_DEPOT_CAPACITY = 70;
    public const int HALF_DEPOT_CAPACITY = 50;
    public const int WEAK_DEPOT_CAPACITY = 30;

    public const int FREE_SUPPLIES_FROM_6 = 15;

    public static List<RoverType> DEPOT_OPENABLE_ROVERS = new List<RoverType>{RoverType.Supply};
    public static List<RoverType> DEPOT_REPAIRABLE_ROVERS = new List<RoverType>{RoverType.Repair};
    public static List<RoverType> DEPOT_ACCESSABLE_ROVERS = new List<RoverType>{RoverType.Supply, RoverType.Repair};

    public static int SUPPLY_REQUIREMENT(int movement)
    {
        switch(movement)
        {
            case 1:
            {
                return 1;
            }
            case 2:
            {
                return 2;
            }
            case 3:
            {
                return 3;
            }
            case 4:
            {
                return 4;
            }
            case 5:
            {
                return 5;
            }
            case 6:
            {
                return 0;
            }
            default:
            {
                return 0;
            }
        }
    }
}




