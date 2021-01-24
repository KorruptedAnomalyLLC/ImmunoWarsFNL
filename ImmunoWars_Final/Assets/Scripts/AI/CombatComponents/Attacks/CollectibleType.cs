///
///This script goes on the dropped type changing objects
///It stores a type and passes that info on to whatever picks it up
///
using UnityEngine;

public class CollectibleType : MonoBehaviour
{
    [SerializeField]
    private Type objectType = default;

    public Type RetrieveType()
    {
        return objectType;
    }
}
