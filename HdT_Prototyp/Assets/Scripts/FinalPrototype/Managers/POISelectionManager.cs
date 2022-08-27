using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class POISelectionManager : MonoBehaviour
{

    private POIHandler[] _POIs;

    private SessionManager _sessionManager;
    private POIMenuManager _POIMenuManager;
    private ModeManager _modeManager;

    [SerializeField]
    private OffScreenPointer _offScreenPointer;

    [SerializeField]
    private Color _idleColor;

    [SerializeField]
    private Color _selectedColor;


    void Awake()
    {

        SetManagers();
    }

    private void SetManagers()
    {
        _sessionManager = SessionManager.Instance;
        _POIMenuManager = _sessionManager.POIMenuManager;
        _modeManager = _sessionManager.ModeManager;
    }

    public void SetupPOIs()
    {
        _POIs = FindObjectsOfType<POIHandler>(true);

        Debug.Log("POIs Length = " + _POIs.Length);
        if(_modeManager == null)
        {
            SetManagers();
        }
        foreach (POIHandler poi in _POIs)
        {
            Debug.Log("POI Id=" + poi.Content.Id);
            poi.Setup(this, _idleColor, _modeManager.ARCamera);
            //poi.Setup(this, _selectedColor, _modeManager.ARCamera);
            //poi.ShowBillboard(false);
        }
    }


    public void SelectPOI(int idOfSelectedPOI)
    {
        if (IsActivePOI(idOfSelectedPOI))
        {
            return;
        }

        foreach (POIHandler poi in _POIs)
        {
            if (poi != null && poi.Content.Id == idOfSelectedPOI)
            {
                poi.SetColor(_selectedColor);
                poi.ShowBillboard(false);

                _POIMenuManager.OpenMenu(poi.Content);
                _sessionManager.ActivePOI = poi.Content;
                _modeManager.PoiIsSelected = true;

                //if (poi.IsVisibleInAR)
                //{
                    _offScreenPointer.Target = poi.ARSphereGO;
               // }
            }
            else
            {
                poi.SetColor(_idleColor);
                poi.ShowBillboard(true);

            }
        }
    }

    public void DeselectCurrentPOI()
    {
        foreach (POIHandler poi in _POIs)
        {
            if (IsActivePOI(poi.Content.Id))
            {
                poi.SetColor(_idleColor);
                poi.ShowBillboard(true);
            }
        }
        _sessionManager.ActivePOI = null;
        _offScreenPointer.Target = null;
        _modeManager.PoiIsSelected = false;

    }

    private bool IsActivePOI(int POIId)
    {
        return (SessionManager.Instance.ActivePOI != null && POIId == SessionManager.Instance.ActivePOI.Id);
    }


}
