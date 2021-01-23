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
    private GameObject PauseUI = default;
    [SerializeField]
    private GameObject pauseButton = default;
    [SerializeField]
    private GameObject playButton = default;

    [SerializeField]
    private GameObject AttackUI = default;

    [SerializeField]
    private GameObject UnitSelectedUI = default;

    [SerializeField]
    private GameObject UnitInfoUI = default;

    [SerializeField]
    private Text nameOfChar = default;
    [SerializeField]
    private Text charInfo = default;

    [SerializeField]
    private ButtonHandler[] attackButtons = default;
    [SerializeField]
    private Image dropUnit = default;

    [SerializeField]
    private Sprite disabledButtonImage = default;

    public void TurnOnBattleUI()
    {
        DisablePause();
        AttackUI.SetActive(true);
        UnitSelectedUI.SetActive(true);

        nameOfChar.text = GlobalBlackboard.Instance.selectedUnit.nameOfChar;
        charInfo.text = GlobalBlackboard.Instance.selectedUnit.charInfo;

        dropUnit.sprite = GlobalBlackboard.Instance.selectedUnit.dropUnitUI;

        #region Attack Buttons
        //disable all buttons initially
        foreach(ButtonHandler button in attackButtons)
        {
            button.ButtonImage.sprite = disabledButtonImage;
            button.buttonActive = false;
        }

        //Go through each attackUI on the selected unit and enable/update the coresponding button
        for (int i = 0; i < GlobalBlackboard.Instance.selectedUnit.attackUI.Length; i++)
        {
            if (GlobalBlackboard.Instance.selectedUnit.attackUI[i] != null)
            {
                attackButtons[i].ButtonImage.sprite = GlobalBlackboard.Instance.selectedUnit.attackUI[i];
                attackButtons[i].buttonActive = true;
            }
            else
            {
                Debug.LogError(gameObject.name + " is missing the button sprite for it's attack number " + i + "Fix This SHIT!!!\n-the sprite slot can be found on the Unit's LocalBlackboard component");
            }
        }
        #endregion
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
