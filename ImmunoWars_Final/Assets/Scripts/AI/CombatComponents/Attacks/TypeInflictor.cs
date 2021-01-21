///
///This script allows attacks to have and inflict a type effect
///It provides a compare type function that returns false if the attack type is None
///
using UnityEngine;

public class TypeInflictor : MonoBehaviour
{
    public bool CompareTypes(Type myType, Type othersType)
    {
        if (myType != Type.None && myType == othersType)
            return true;
        else
            return false;
    }
}
