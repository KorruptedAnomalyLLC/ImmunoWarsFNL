using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// Uses Unity's canvas event system to know when the attatched ui element has been clicked
    /// When it is clicked, tells the UIManager script to stop being a lazy ass and do some work
    /// </summary>
    [SerializeField]
    private ButtonType _buttonType;


    #region IPointerDown Implementation
    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.ButtonClicked(_buttonType);
    }
    #endregion

}
