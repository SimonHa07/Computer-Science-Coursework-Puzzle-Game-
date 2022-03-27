using SimonHa.Core.Singletons;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : Singleton<PlayerNetworkManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersinGame
    {
        get
        {
            return playersInGame.Value;
        }
        set
        {
            playersInGame.Value = value;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"{id} has connected...");
                playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"{id} has disconnected...");
                playersInGame.Value--;
            }
        };

    }
}
