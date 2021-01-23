/// <summary>
/// This script uses Unity's canvas event system to know when the attatched ui element has been clicked
/// When it is clicked, tells the UIManager script to stop being a lazy ass and do some work
/// </summary>

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler
{
    public bool buttonActive = true;

    public Image ButtonImage { get { return buttonImage; } }
    private Image buttonImage;


    [SerializeField]
    private ButtonType _buttonType;


    private void Awake()
    {
        if(TryGetComponent(out Image temp))
        {
            buttonImage = temp;
        }
        else
        {
            Debug.LogError(gameObject.name + " is Missing the Image component Add one or Suffer the consequences.");
        }
    }

    #region IPointerDown Implementation
    public void OnPointerDown(PointerEventData eventData)
    {
        if(buttonActive)
            UIManager.Instance.ButtonClicked(_buttonType);
    }
    #endregion
}
