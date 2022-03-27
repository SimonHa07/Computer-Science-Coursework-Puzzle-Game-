using SimonHa.Core.Singletons;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
public class RelayManager : Singleton<RelayManager>
{
    [SerializeField] private string environment = "production"; //this is in unity project settings dashboard

    [SerializeField] private int maxConnections = 10;

    public bool isRelayEnabled => Transport != null && 
        Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
    public bool isRelayServerActive = false;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    public async Task<RelayHostData> SetupRelay()
    {
        Debug.Log($"Relay Server starting with max connections {maxConnections}");
        InitializationOptions options = new InitializationOptions()
            .SetEnvironmentName(environment);
        await UnityServices.InitializeAsync(options);
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);
        //Ask Unity Services to allocate a Relay server

        RelayHostData relayHostData = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData,
        };

        relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID); //waits for the relay to give a join code

        Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, 
            relayHostData.AllocationIDBytes, relayHostData.Key, relayHostData.ConnectionData); //sends the information we have fetched  to the transport

        Debug.Log($"Relay Server generated join code {relayHostData.JoinCode}");
        isRelayServerActive = true;

        return relayHostData;
    }
    public async Task<RelayJoinData> JoinRelay(string joinCode)
    {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        //Ask Unity Services to join a specific Relay server

        RelayJoinData relayjoinData = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            JoinCode = joinCode,
        };

        Transport.SetRelayServerData(relayjoinData.IPv4Address, relayjoinData.Port, relayjoinData.AllocationIDBytes, 
            relayjoinData.Key, relayjoinData.ConnectionData, relayjoinData.HostConnectionData); //sends the information we have fetched  to the transport

        Debug.Log($"Client Joined with join code{joinCode}");
        return relayjoinData;
    }
    public void EndRelay()
    {
        //Transport.DisconnectLocalClient();
    }
}
