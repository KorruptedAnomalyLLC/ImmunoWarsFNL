/// <summary>
/// Handles all game Input, calls needed functions based on input
/// Supports Mobile via touch and PC controls via mouse
/// All is disabled during pause
/// Priority is given to menu clicks, input is ignored if clicking on menu ui
/// ***Default touch phase mode should be set to Canceled in the inspector. Check there if Input seems messed up.***
/// </summary>

using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : GenericSingletonClass<InputManager>
{
    [SerializeField]
    private TouchPhase _touchPhase = TouchPhase.Began;
    [SerializeField]
    private LayerMask rayCastMask = default;

    private LocalBlackboard touchedUnit;
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
                        touchedUnit = hitObj.GetComponentInParent<LocalBlackboard>(); //if you've touched a unit, assign it

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

                    touchedUnit = hitObj.GetComponentInParent<LocalBlackboard>();

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
            //if unit's ui is touched, look for UI input
            //else, move to touch spot
            GlobalBlackboard.Instance.selectedUnit._commandMessenger.MoveToTouchPos(touchedPos);

            return;
        }

        if (touchedUnit.heroUnit) //if you touched a hero unit
        {
            if(touchedUnit == GlobalBlackboard.Instance.selectedUnit)
            {
                //re select unit, pause da game
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.CallSelected();
                UIManager.Instance.TurnOnBattleUI();
                PauseManager.Instance.PauseGame();
            }
            else
            {
                //move to touched unit's position
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.AddTarget(touchedUnit, false);
            }
        }
        else //if you touched an enemy unit
        {
            //target enemy, change to combat mode, use attack on enemy
            GlobalBlackboard.Instance.selectedUnit._commandMessenger.AddTarget(touchedUnit, true);
        }
    }



    private void NothingSelectedBranch()
    {
        if(touchedUnit == null)
        {
            return;
        }

        if (touchedUnit.heroUnit)
        {
            //select unit, pause da game
            GlobalBlackboard.Instance.selectedUnit = touchedUnit;
            GlobalBlackboard.Instance.unitSelected = true;
            GlobalBlackboard.Instance.selectedUnit._commandMessenger.CallSelected();
            UIManager.Instance.TurnOnBattleUI();
            PauseManager.Instance.PauseGame();
        }
        else
        {
            //display enemy info ui
        }
    }
}
