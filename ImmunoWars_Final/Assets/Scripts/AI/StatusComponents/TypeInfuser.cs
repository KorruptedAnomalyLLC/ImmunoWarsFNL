///
///This script stores a unit's type
///
using UnityEngine;

public enum Type
{
    Color1,
    Color2,
    Color3,
    None
}

public class TypeInfuser : MonoBehaviour
{
    public Type myType = Type.None;
}
