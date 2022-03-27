using System.Text;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueGameButton;
    [SerializeField] private Button JoinButton;
    [SerializeField] private TMP_InputField joinCode_input;
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private GameObject playerTwoUI;
    public static string joinCode;

    private bool newGame = false;
    private bool continueGame = false;
    // Start is called before the first frame update
    private void Start()
    {
        SetupMainButtons();
    }
    public void StartGame()
    {
        if (newGame)
        {
            gameObject.GetComponent<UI_Manager>().NewGame();
        }
        if (continueGame)
        {
            gameObject.GetComponent<UI_Manager>().Continue();
        }
        StartGameButton.interactable = false;
    }
    public void ResetState()
    {
        StartGameButton.interactable = false;
        playerTwoUI.SetActive(false);
    }
    public void ShutDownSession() //Shut downs all the server side systems
    {
        NetworkManager.Singleton.Shutdown();
        PlayerNetworkManager.Instance.PlayersinGame--;
        
    }
    private void Update()
    {
        // playersInGameText.text = $"Players in game : {PlayerNetworkManager.Instance.PlayersinGame}";
        Debug.Log(PlayerNetworkManager.Instance.PlayersinGame);
        Debug.Log(NetworkManager.Singleton.IsHost);
        if (PlayerNetworkManager.Instance.PlayersinGame == 2)
        {
            Debug.Log("two players are here!!");
            if (NetworkManager.Singleton.IsHost)
            {
                StartGameButton.interactable = true;
            }
            playerTwoUI.SetActive(true);
            PlayerNetworkManager.Instance.PlayersinGame++;
        }
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) // once the game has quit, the network manager doesnt exist (but the server still does)
        {
            return;
        }
        //NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        //NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }
    private void SetupMainButtons()
    {
        NewGameButton?.onClick.AddListener(async () =>
        {
            ResetState();
            if (RelayManager.Instance.isRelayEnabled) //&& !RelayManager.Instance.isRelayServerActive)
            {
                joinCodeText.text = $"Join Code : ";
                RelayHostData data = await RelayManager.Instance.SetupRelay();
                joinCodeText.text = $"Join Code : {data.JoinCode}";
                joinCode = data.JoinCode.ToUpper();
            }
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Server has started");
                newGame = true;
            }
            else
            {
                Debug.Log("Server has failed to start");
            }
        });
        ContinueGameButton?.onClick.AddListener(async () =>
        {
            ResetState();
            if (RelayManager.Instance.isRelayEnabled)//&& !RelayManager.Instance.isRelayServerActive)
            {
                joinCodeText.text = $"Join Code : ";
                RelayHostData data = await RelayManager.Instance.SetupRelay();
                joinCodeText.text = $"Join Code : {data.JoinCode}";
                joinCode = data.JoinCode.ToUpper();
            }
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Server has started");
                continueGame = true;
            }
            else
            {
                Debug.Log("Server has failed to start");
            }
        });
        JoinButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled && !string.IsNullOrEmpty(joinCode_input.text))
            {
                RelayJoinData data = await RelayManager.Instance.JoinRelay(joinCode_input.text);
                joinCodeText.text = $"Join Code : {data.JoinCode}";
                joinCode = data.JoinCode.ToUpper();
            }
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client has joined");
                playerTwoUI.SetActive(true);
            }
            else
            {
                Debug.Log("Client has failed to join server");
            }
        });
    }
    public void HostGame()
    {
        //NetworkManager.Singleton.StartHost();
    }
    public void JoinGame()
    {
        //NetworkManager.Singleton.StartClient();
    }
    public void LeaveGame()
    {
        NetworkManager.Singleton.Shutdown();
    }

    /* private void HandleClientConnected(ulong clientid)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        if (clientid == NetworkManager.Singleton.LocalClientId)
        {
            c_j_sessionUI.SetActive(false);
            leaveButton.SetActive(true);
        }
    }
    private void HandleClientDisconnected(ulong clientid)
    {
        if (clientid == NetworkManager.Singleton.LocalClientId)
        {
            c_j_sessionUI.SetActive(true);
            leaveButton.SetActive(false);
        }
    }
    /*private void ApprovalCheck(byte[] connection_data, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string password = Encoding.ASCII.GetString(connection_data);

        bool approvedConnection = password == password_input.text; // is the password correct?

        callback(true, null, approvedConnection, null, null);
    }
    */
}
