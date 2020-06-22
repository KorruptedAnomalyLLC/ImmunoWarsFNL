using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Temp, shit code... will get updated later this week 6/22/20
    /// </summary>
    
    
    [SerializeField]
    private TouchPhase _touchPhase = TouchPhase.Began;
    [SerializeField]
    private LayerMask rayCastMask;
    [SerializeField]
    private bool unitSelected = false;
    [SerializeField]
    private UnitBase selectedUnit;

    void Update()
    {
        MobileControls();
        PCControls();
    }

    private void MobileControls()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == _touchPhase)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, rayCastMask))
            {
                GameObject hitObj = hit.transform.gameObject;

                if (!unitSelected)
                {
                    selectedUnit = hitObj.GetComponentInParent<UnitBase>();

                    if (selectedUnit != null)
                    {
                        selectedUnit.Selected();
                        //pause da game
                        unitSelected = true;
                    }
                }
                else
                {
                    selectedUnit.MoveTo(hit.point);
                    selectedUnit.Drop();
                    selectedUnit = null;
                    unitSelected = false;
                }
            }
        }
    }

    private void PCControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, rayCastMask))
            {
                GameObject hitObj = hit.transform.gameObject;

                if (!unitSelected)
                {
                    selectedUnit = hitObj.GetComponentInParent<UnitBase>();

                    if (selectedUnit != null)
                    {
                        selectedUnit.Selected();
                        //pause da game
                        unitSelected = true;
                    }
                }
                else
                {
                    selectedUnit.MoveTo(hit.point);
                    selectedUnit.Drop();
                    selectedUnit = null;
                    unitSelected = false;
                }
            }
        }
    }
}
