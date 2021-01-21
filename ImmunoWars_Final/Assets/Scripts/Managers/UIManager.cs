using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum ButtonType
{
    Attack1,
    Attack2,
    Attack3,
    DropUnit,
    Info,
    ExitInfo,
    Quit
}

public class UIManager : GenericSingletonClass<UIManager>
{
    /// <summary>
    /// This script manages UI input,
    /// ToDo:
    ///Swap UI images based on selected unit
    ///call ui when new unit is selected
    ///drop ui when unit is dropped
    ///swap unit's attacks based on attack chosen
    ///interact with pause feature
    /// </summary>
    [SerializeField]
    private GameObject AttackUI;

    [SerializeField]
    private GameObject UnitSelectedUI;

    [SerializeField]
    private GameObject UnitInfoUI;

    [SerializeField]
    private Text nameOfChar;
    [SerializeField]
    private Text charInfo;

    [SerializeField]
    private Image attack1;
    [SerializeField]
    private Image attack2;
    [SerializeField]
    private Image attack3;
    [SerializeField]
    private Image dropUnit;

    public void TurnOnBattleUI()
    {
        AttackUI.SetActive(true);
        UnitSelectedUI.SetActive(true);

        nameOfChar.text = GlobalBlackboard.Instance.selectedUnit.nameOfChar;
        charInfo.text = GlobalBlackboard.Instance.selectedUnit.charInfo;

        attack1.sprite = GlobalBlackboard.Instance.selectedUnit.attack1UI;
        attack2.sprite = GlobalBlackboard.Instance.selectedUnit.attack2UI;
        attack3.sprite = GlobalBlackboard.Instance.selectedUnit.attack3UI;
        dropUnit.sprite = GlobalBlackboard.Instance.selectedUnit.dropUnitUI;
    }

    public void TurnOffBattleUI()
    {
        AttackUI.SetActive(false);
    }

    private void TurnOffUnitUI()
    {
        AttackUI.SetActive(false);
        UnitSelectedUI.SetActive(false);
    }

    public void ButtonClicked(ButtonType _buttonType)
    {
        switch (_buttonType)
        {
            case ButtonType.Attack1:
                //change selected unit's attack mode to 1
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.AttackButtonChosen(0);
                PauseManager.Instance.UnpauseGame();
                TurnOffBattleUI();
                break;
            case ButtonType.Attack2:
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.AttackButtonChosen(1);
                PauseManager.Instance.UnpauseGame();
                TurnOffBattleUI();
                break;
            case ButtonType.Attack3:
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.AttackButtonChosen(2);
                PauseManager.Instance.UnpauseGame();
                TurnOffBattleUI();
                break;
            case ButtonType.DropUnit:
                //drop unit
                GlobalBlackboard.Instance.selectedUnit._commandMessenger.CallDropped();
                GlobalBlackboard.Instance.unitSelected = false;
                PauseManager.Instance.UnpauseGame();
                TurnOffUnitUI();
                break;
            case ButtonType.Info:
                UnitInfoUI.SetActive(true);
                break;
            case ButtonType.ExitInfo:
                UnitInfoUI.SetActive(false);
                break;
            case ButtonType.Quit:
                //pause game, bring up return to menu ui
                break;
            default:
                break;
        }
    }
    
}
