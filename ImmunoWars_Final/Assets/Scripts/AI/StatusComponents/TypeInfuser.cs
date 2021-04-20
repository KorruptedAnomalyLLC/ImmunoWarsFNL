///
///This script stores a unit's type
///
using UnityEngine;

public enum Type
{
    Stapha,
    Shiggy,
    SuperFluper,
    None
}

public class TypeInfuser : MonoBehaviour
{
    public Type myType = Type.None;
    public Type attackType = Type.None;
}
