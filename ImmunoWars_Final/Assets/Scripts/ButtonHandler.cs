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

    //Marked the Image as a read only variable. This is becuase there is never a need for any outside script to change this value
    //but there is still a need to read it. This doesn't help with any function but it lets other coders know for sure that
    //this value is only ever changed inside of this script
    public Image ButtonImage { get { return buttonImage; } }
    private Image buttonImage;


    [SerializeField]
    private ButtonType _buttonType;

    //Grab the Image component
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
    public void OnPointerDown(PointerEventData eventData) //reads click/touch events from Unity
    {
        if(buttonActive)
            UIManager.Instance.ButtonClicked(_buttonType);
    }
    #endregion
}
