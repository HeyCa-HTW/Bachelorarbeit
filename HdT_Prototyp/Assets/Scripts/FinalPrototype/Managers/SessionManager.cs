using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class SessionManager : MonoBehaviour
{


//------------------ GLOBAL VARIABLES----------------------------
    public static SessionManager Instance { get; private set; }

    [SerializeField]
    private User _loggedInUser;
    public User LoggedInUser { get => _loggedInUser; }

    public PointOfInterest ActivePOI { get; set; }


//------------------ MANAGERS----------------------------
   
    public MainGUIManager GUIController { get; private set; }
    public POISelectionManager POISelectionManager { get; private set; }
    public ModeManager ModeManager { get; private set; }
    public POIMenuManager POIMenuManager { get; private set; }
    public CommentManager CommentManager { get; private set; }

    [SerializeField]
    private UserInstructionManager _instructionController;
    public UserInstructionManager InstructionController { get => _instructionController; }

    [SerializeField]
    private ARTrackedImageManager _trackedImageManager;


//------------------ DATABASES----------------------------
    [SerializeField]
    private PointOfInterestDB _poiDB;
    public PointOfInterestDB POI_DB { get => _poiDB;  }

    [SerializeField]
    private CommentsDB _commentsDB;
    public CommentsDB CommentsDB { get => _commentsDB; }

    [SerializeField]
    private ThreadsDB _threadsDB;
    public ThreadsDB ThreadsDB { get => _threadsDB; }
    //---------------------------------------------------------

    [SerializeField]
    private bool _activateSessionAtStart;
    private bool _sessionInProgress;


    void OnEnable() => _trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;

    void OnDisable() => _trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SetManagers();
        SetupDatabases();

        if (_activateSessionAtStart)
        {
            ActivateSession();
        } else
        {
            InstructionController.ShowInstruction(Instructions.scanQRCode, 0, false);
        }
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            //activate Session
            if (!_sessionInProgress)
            {
                ActivateSession();
            }
        }
    }

    public void SetManagers()
    {
        GUIController = GetComponentInChildren<MainGUIManager>();
        POISelectionManager = GetComponentInChildren<POISelectionManager>();
        ModeManager = GetComponentInChildren<ModeManager>();
        POIMenuManager = GetComponentInChildren<POIMenuManager>();
        CommentManager = GetComponentInChildren<CommentManager>();
        //InstructionController = GetComponentInChildren<UserInstructionManager>();
        

    }

    public void SetupDatabases()
    {
        POI_DB.Setup();
        CommentsDB.Setup();
        ThreadsDB.Setup();
    }

    private void ActivateSession()
    {
        _sessionInProgress = true;
        InstructionController.HideInstructionScreen();
        //add wait time
        InstructionController.ShowInstruction(Instructions.switchMode, 7f, true);
        GUIController.ShowMainGUI(); //for ScreenShots
        POISelectionManager.SetupPOIs();
        ModeManager.SetupSession();
        Debug.Log("Inside Activate Session");
    }

 


}
