using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : GenericSingletonClass<InputManager>
{
    /// <summary>
    /// Handles all game Input, calls needed functions based on input
    /// Supports Mobile via touch and PC controls via mouse
    /// All is disabled during pause
    /// Priority is given to menu clicks, input is ignored if clicking on menu ui
    /// </summary>
    
    
    [SerializeField]
    private TouchPhase _touchPhase = TouchPhase.Began;
    [SerializeField]
    private LayerMask rayCastMask;


    //private bool unitSelected = false;
    //private UnitRoot selectedUnit;
    private UnitRoot touchedUnit;
    private Vector3 touchedPos;

    void Update()
    {
        if (PauseManager.isPaused) //don't take in input if game is paused
            return;

        MobileControls();
        PCControls();
    }


    #region Input
    private void MobileControls()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == _touchPhase) //touch something
            {
                Touch currentTouch = Input.GetTouch(0);

                if (currentTouch.phase == _touchPhase)
                {
                    Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000, rayCastMask)) //touch actually reads, only do this if touch isn't a drag
                    {
                        GameObject hitObj = hit.transform.gameObject;
                        touchedPos = hit.point;
                        touchedUnit = hitObj.GetComponentInParent<UnitRoot>(); //if you've touched a unit, assign it

                        //choose your path
                        if (GlobalBlackboard.Instance.unitSelected)
                            UnitSelectedBranch();
                        else
                            NothingSelectedBranch();
                    }
                }
            }
        }
    }


    private void PCControls()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, rayCastMask))
                {
                    GameObject hitObj = hit.transform.gameObject;
                    touchedPos = hit.point;
                    Debug.LogError(touchedPos);
                    touchedUnit = hitObj.GetComponentInParent<UnitRoot>();

                    if (GlobalBlackboard.Instance.unitSelected)
                        UnitSelectedBranch();
                    else
                        NothingSelectedBranch();

                }
            }
        }
    }
    #endregion


    private void UnitSelectedBranch()
    {
        if(touchedUnit == null)
        {
            Debug.Log("move unit to touch position");
            //if unit's ui is touched, look for UI input
            //else, move to touch spot
            GlobalBlackboard.Instance.selectedUnit.MoveToTouchPos(touchedPos);
            return;
        }

        if (touchedUnit._localBlackboard.heroUnit) //if you touched a hero unit
        {
            if(touchedUnit == GlobalBlackboard.Instance.selectedUnit)
            {
                //re select unit, pause da game
                GlobalBlackboard.Instance.selectedUnit.Selected();
                UIManager.Instance.TurnOnBattleUI();
                PauseManager.Instance.PauseGame();
            }
            else
            {
                Debug.Log("Move to Hero Unit");
                //move to touched unit's position
                GlobalBlackboard.Instance.selectedUnit.UpdateTarget(touchedUnit.transform);
            }
        }
        else //if you touched an enemy unit
        {
            Debug.Log("Moveto enemy");
            //target enemy, change to combat mode, use attack on enemy
            GlobalBlackboard.Instance.selectedUnit.UpdateTarget(touchedUnit.transform);
            GlobalBlackboard.Instance.selectedUnit.EnterCombat();
        }
    }



    private void NothingSelectedBranch()
    {
        if(touchedUnit == null)
        {
            return;
        }

        if (touchedUnit._localBlackboard.heroUnit)
        {
            //select unit, pause da game
            GlobalBlackboard.Instance.selectedUnit = touchedUnit;
            GlobalBlackboard.Instance.unitSelected = true;
            GlobalBlackboard.Instance.selectedUnit.Selected();
            UIManager.Instance.TurnOnBattleUI();
            PauseManager.Instance.PauseGame();
        }
        else
        {
            //display enemy info ui
        }
    }
}
