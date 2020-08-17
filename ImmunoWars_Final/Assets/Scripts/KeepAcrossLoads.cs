using UnityEngine;

public class KeepAcrossLoads : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
