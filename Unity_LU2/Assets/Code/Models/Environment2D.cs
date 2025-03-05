using System;
using System.Collections.Generic;
using UnityEngine;


/**
 * Bijzonderheden wegens beperkingen van JsonUtility:
 * - Models hebben variabelen met kleine letters omdat JsonUtility anders de velden uit de JSON niet correct overzet naar het C# object.
 * - De id is een string in plaats van een Guid omdat JsonUtility Guid niet ondersteunt. Gelukkig geeft dit geen probleem indien we gewoon een string gebruiken in Unity en een Guid in onze backend API.
*/
[Serializable]
public class Environment2D
{
    public string id;

    public string name;

    public int maxLength;

    public int maxHeight;
}

public class CreateEnvironment
{
    public string name;
    public int environmentType;
}


[System.Serializable]
public class EnvironmentData
{
    public string id;
    public string name;
    public int maxLength;
    public int maxHeight;
    public string createdAt;
    public string updatedAt;
    public int environmentType;
}

[System.Serializable]
public class EnvironmentDataList
{
    public List<EnvironmentData> environments;
}