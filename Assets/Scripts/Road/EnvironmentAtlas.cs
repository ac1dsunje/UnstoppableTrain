using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment Atlas", menuName = "Environment/Environment atlas")]

public class EnvironmentAtlas : ScriptableObject
{
    public List<GameObject> EnvironmentObjects;
}