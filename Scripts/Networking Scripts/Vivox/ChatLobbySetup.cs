using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VivoxUnity;

public class ChatLobbySetup : MonoBehaviour
{
    private VivoxVoiceManager _VVManager;
    [SerializeField] private GameObject chatScreen;
    private string lobbyChannelName = NetworkUIManager.joinCode;
    //private string lobbyChannelName = "lobbyChannel";
    private void Awake()
    {
        Setup();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        _VVManager.OnUserLoggedInEvent -= OnUserLoggedIn;
        _VVManager.OnUserLoggedOutEvent -= OnUserLoggedOut;
        //_VVManager.OnParticipantAddedEvent -= VivoxVoiceManager_OnParticipantAddedEvent;
    }
    private void Setup()
    {
        _VVManager = VivoxVoiceManager.Instance;
        _VVManager.OnUserLoggedInEvent += OnUserLoggedIn;
        _VVManager.OnUserLoggedOutEvent += OnUserLoggedOut;
        if (_VVManager.LoginState == LoginState.LoggedIn)
        {
            OnUserLoggedIn();
        }
        else
        {
            OnUserLoggedOut();
        }
    }
    private void OnUserLoggedIn()
    {
        chatScreen.SetActive(true);
        var lobbyChannel = _VVManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == lobbyChannelName);
        //var lobbychannel = _VVManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == LobbyChannelName);
        if ((_VVManager && _VVManager.ActiveChannels.Count == 0)
            || lobbyChannel == null)
        {
            JoinLobbyChannel();
        }
        else
        {
            if (lobbyChannel.AudioState == ConnectionState.Disconnected)
            {
                // Ask for hosts since we're already in the channel and part added won't be triggered.

                lobbyChannel.BeginSetAudioConnected(true, true, ar =>
                {
                    Debug.Log("Now transmitting into lobby channel");
                });
            }

        }
    }
    private void OnUserLoggedOut()
    {
        chatScreen.SetActive(false);
        _VVManager.DisconnectAllChannels();
    }
    private void JoinLobbyChannel()
    {
        // Do nothing, participant added will take care of this
        //_VVManager.OnParticipantAddedEvent += VivoxVoiceManager_OnParticipantAddedEvent;
        Debug.Log(lobbyChannelName);
        _VVManager.JoinChannel(lobbyChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);
    }
}
