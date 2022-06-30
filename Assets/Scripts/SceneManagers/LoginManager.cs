using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using TMPro;

public class LoginManager : MonoBehaviour
{
    //----------------------------------------------------------
    // UI elements
    //----------------------------------------------------------
    [Header("Setup variables")]

    [SerializeField] private TMPro.TMP_InputField nameInputField;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMPro.TMP_Text errorText;

    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------

    [Header("SmartFoxServer variables setup")]

    [Tooltip("IP address or domain name of the SmartFoxServer 2X instance")]
    public string Host = "127.0.0.1";

    [Tooltip("TCP port listened by the SmartFoxServer 2X instance; used for regular socket connection in all builds except WebGL")]
    public int TcpPort = 9933;

    [Tooltip("WebSocket port listened by the SmartFoxServer 2X instance; used for in WebGL build only")]
    public int WSPort = 8080;

    [Tooltip("Name of the SmartFoxServer 2X Zone to join")]
    public string Zone = "BasicExamples";

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;

    [Header("Configuration")]
    [SerializeField] private string nextSceneToLoad;

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    void Start()
    {
        // Initialize UI
        errorText.text = "";
    }

    void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();
    }

    //----------------------------------------------------------
    // Public interface methods for UI
    //----------------------------------------------------------

    public void OnLoginButtonClick()
    {
        enableLoginUI(false);

        // Set connection parameters
        ConfigData cfg = new ConfigData();
        cfg.Host = Host;

        cfg.Port = TcpPort;

        cfg.Zone = Zone;

        // Initialize SFS2X client and add listeners

        sfs = new SmartFox();

        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        // Connect to SFS2X
        sfs.Connect(cfg);
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    private void enableLoginUI(bool enable)
    {
        nameInputField.interactable = enable;
        loginButton.interactable = enable;
        errorText.text = "";
    }

    private void reset()
    {
        // Remove SFS2X listeners
        sfs.RemoveAllEventListeners();

        // Enable interface
        enableLoginUI(true);
    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    private void OnConnection(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            // Save reference to the SmartFox instance in a static field, to share it among different scenes
            SmartFoxConnection.Connection = sfs;

            Debug.Log("SFS2X API version: " + sfs.Version);
            Debug.Log("Connection mode is: " + sfs.ConnectionMode);

            // Login
            sfs.Send(new Sfs2X.Requests.LoginRequest(nameInputField.text));
        }
        else
        {
            // Remove SFS2X listeners and re-enable interface
            reset();

            // Show error message
            errorText.text = "Connection failed; is the server running at all?";
        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS2X listeners and re-enable interface
        reset();

        string reason = (string)evt.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {
            // Show error message
            errorText.text = "Conexão perdida; a razão foi: " + reason;
        }
    }

    private void OnLogin(BaseEvent evt)
    {
        // Remove SFS2X listeners and re-enable interface before moving to the main game scene
        reset();

        // Go to main game scene
        SceneManager.LoadScene(nextSceneToLoad);
    }

    private void OnLoginError(BaseEvent evt)
    {
        // Disconnect
        sfs.Disconnect();

        // Remove SFS2X listeners and re-enable interface
        reset();

        // Show error message
        errorText.text = "Falha no login: " + (string)evt.Params["errorMessage"];
    }

}