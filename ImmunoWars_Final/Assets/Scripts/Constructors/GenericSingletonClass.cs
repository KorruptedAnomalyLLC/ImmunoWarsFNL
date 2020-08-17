using UnityEngine;

public class GenericSingletonClass<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Generic class to setup singleton for any script that inherits from it,
    /// </summary>
    
    private static T _instance; //internal copy of the instance, that way no outside scripts ever get to mess with this script's instance reference

    //reference that other scripts can read, if there isn't an instance in the scene already it creates one when another script tries to use Instance,
    //this fixes potential null reference errors from trying to call an instance that hasn't been placed yet
    public static T Instance 
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    //when this script gets a chance to call Awake, 
    //check for any other instances out in the wild. If there is one then destroy this script... there can only be one
    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;

            if(this.transform.parent == null) //only calls DontDestroyOnLoad if there are no parents to this script, DontDestroyOnLoad doesn't work on child objects
                DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
