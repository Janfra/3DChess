using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperClasses
{

}

public class DebugHelper
{
    public static void NullWarn(string nullObj, string funcName)
    {
        Debug.LogWarning($"{nullObj} in {funcName} is null!");
    }

    public static void loopNullWarn(string nullObj, string funcName, int iteration, string objName)
    {
        Debug.LogWarning($"There is no {nullObj} in {funcName}. Iteration number: {iteration} Object name: {objName}");
    }
}
