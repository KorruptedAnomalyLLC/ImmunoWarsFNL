/// <summary>
/// This script manages UI input...I should type more here but meh
/// ToDo:
///interact with pause feature
/// </summary>

using UnityEngine;
using UnityEngine.UI;


public enum ButtonType
{
    Attack1,
    Attack2,
    Attack3,
    DropUnit,
    Info,
    ExitInfo,
    Play,
    Pause
}

public class UIManager : GenericSingletonClass<UIManager>
{
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private GameObject playButton;

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
        DisablePause();
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
        EnablePause();
    }

    private void TurnOffUnitUI()
    {
        TurnOffBattleUI();
        UnitSelectedUI.SetActive(false);
    }

    private void TogglePauseButton(bool pauseActive)
    {
        pauseButton.SetActive(pauseActive);
        playButton.SetActive(!pauseActive);
    }

    private void DisablePause()
    {
        PauseUI.SetActive(false);
    }

    private void EnablePause()
    {
        PauseUI.SetActive(true);
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
            case ButtonType.Pause:
                TogglePauseButton(false);
                PauseManager.Instance.PauseGame();
                break;
            case ButtonType.Play:
                TogglePauseButton(true);
                PauseManager.Instance.UnpauseGame();
                break;
            default:
                break;
        }
    }
    
}
