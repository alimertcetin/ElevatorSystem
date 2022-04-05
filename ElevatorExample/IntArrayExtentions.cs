using System;
using System.Collections.Generic;

public static class IntArrayExtentions
{
    public static void Sort(ref int[] arr)
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
        Sort(ref arr);
        return arr[0];
    }

    public static int GetClosest(this int[] arr, int value)
    {
        Sort(ref arr);
        int current = arr[0];
        int diff = Math.Abs(value - current);
        for (int i = 0; i < arr.Length; i++)
        {
            var idxValue = arr[i];
            var newDiff = Math.Abs(idxValue - value);
            if(newDiff < diff)
            {
                diff = newDiff;
                current = idxValue;
            }
        }

        return current;
    }

    public static int[] RemoveAt(ref int[] arr, int index)
    {
        int[] newArr = new int[arr.Length - 1];
        bool indexPassed = false;
        for (int i = 0; i < arr.Length; i++)
        {
            if (i == index)
            {
                indexPassed = true;
                continue;
            }
            if (indexPassed)
            {
                newArr[i - 1] = arr[i];
            }
            else
            {
                newArr[i] = arr[i];
            }
        }
        return newArr;
    }
    
    public static int[] RemoveValueAll(ref int[] arr, int value)
    {
        int[] newArr = new int[0];
        int counter = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if(arr[i] == value)
            {
                counter++;
                continue;
            }

            if(i - counter == newArr.Length)
            {
                newArr = Resize(ref newArr, (i - counter) + 1);
            }

            newArr[i - counter] = arr[i];
        }
        arr = newArr;
        return arr;
    }

    public static int[] RemoveDuplicates(ref int[] arr)
    {
        List<int> withoutDuplicateList = new List<int>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            if (!withoutDuplicateList.Contains(arr[i]))
            {
                withoutDuplicateList.Add(arr[i]);
            }
        }
        arr = withoutDuplicateList.ToArray();
        return arr;
    }

    public static int[] Resize(ref int[] arr, int newSize)
    {
        var newArr = new int[newSize];

        for (int i = 0; i < arr.Length; i++)
        {
            newArr[i] = arr[i];
        }
        arr = newArr;
        return arr;
    }

    public static bool Contains(this int[] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if(arr[i] == value)
            {
                return true;
            }
        }
        return false;
    }

    public static List<int> ToList(this int[] arr)
    {
        var list = new List<int>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            list.Add(arr[i]);
        }
        return list;
    }
}
