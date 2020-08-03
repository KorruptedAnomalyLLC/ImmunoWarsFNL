using UnityEngine;

public class UnitRoot : MonoBehaviour
{
    
    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private float tickTime = 0.5f;
    private float currentTick = 0;


    [HideInInspector]
    public MovementRoot moveRoot; //not req

    [HideInInspector]
    public LocalBlackboard _localBlackboard; //req


    private void Start()
    {
        _localBlackboard = GetComponent<LocalBlackboard>();


        if (TryGetComponent(out MovementRoot temp))
        {
            moveRoot = temp;
            moveRoot.Setup();
        }
    }


    public void Selected()
    {
        ui.SetActive(true);
    }

    public void Drop()
    {
        ui.SetActive(false);
    }

    private void Update()
    {
        currentTick += Time.deltaTime;

        if(currentTick > tickTime)
        {
            if(moveRoot != null)
                moveRoot._update();

            currentTick = 0;
        }
    }
}
