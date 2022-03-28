using AutamationSystem.ElevatorSystem;
using System;

public static class IntArrayExtentions
{
    public static void Sort(this int[] arr)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            var min = i;
            for (var j = i + 1; j < arr.Length; j++)
            {
                if (arr[min] > arr[j])
                {
                    min = j;
                }
            }
            if (min != i)
            {
                var lowerValue = arr[min];
                arr[min] = arr[i];
                arr[i] = lowerValue;
            }
        }
    }

    public static int GetMin(this int[] arr)
    {
        int[] newArr = arr;
        newArr.Sort();
        return newArr[0];
    }

    public static int GetClosest(this int[] arr, int value)
    {
        int[] newArr = arr;
        newArr.Sort();
        int current = newArr[0];
        int diff = Math.Abs(value - current);
        for (int i = 0; i < newArr.Length; i++)
        {
            var idxValue = newArr[i];
            var newDiff = Math.Abs(idxValue - value);
            if(newDiff < diff)
            {
                diff = newDiff;
                current = idxValue;
            }
        }

        return current;
    }
}

public static class ElevatorExtentions
{
    public static void Sort(this Elevator[] arr)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            var min = i;
            for (var j = i + 1; j < arr.Length; j++)
            {
                if (arr[min].CurrentFloor.FloorIndex > arr[j].CurrentFloor.FloorIndex)
                {
                    min = j;
                }
            }
            if (min != i)
            {
                var lowerValue = arr[min];
                arr[min] = arr[i];
                arr[i] = lowerValue;
            }
        }
    }

    public static Elevator GetClosest(this Elevator[] arr, int floorIndex)
    {
        Elevator[] newArr = new Elevator[arr.Length];
        for (int i = 0; i < newArr.Length; i++)
        {
            newArr[i] = new Elevator(arr[i]);
        }
        newArr.Sort();

        Elevator current = newArr[0];
        int diff = Math.Abs(floorIndex - current.CurrentFloor.FloorIndex);
        for (int i = 0; i < newArr.Length; i++)
        {
            var idxValue = newArr[i];
            var newDiff = Math.Abs(idxValue.CurrentFloor.FloorIndex - floorIndex);
            if (newDiff < diff)
            {
                diff = newDiff;
                current = idxValue;
            }
        }
        for (int i = 0; i < arr.Length; i++)
        {
            if (current.ElevatorIndex == arr[i].ElevatorIndex)
            {
                current = arr[i];
                break;
            }
        }

        return current;
    }
}