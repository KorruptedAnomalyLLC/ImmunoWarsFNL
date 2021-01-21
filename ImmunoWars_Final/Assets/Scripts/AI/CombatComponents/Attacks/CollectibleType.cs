///
///This script goes on the dropped type changing objects
///It stores a type and passes that info on to whatever picks it up
///
using UnityEngine;

public class CollectibleType : MonoBehaviour
{
    private Type objectType;

    public Type RetrieveType()
    {
        return objectType;
    }
}
