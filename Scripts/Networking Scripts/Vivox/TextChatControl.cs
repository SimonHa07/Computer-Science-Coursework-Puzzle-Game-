using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VivoxUnity;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class TextChatControl : MonoBehaviour
{
    private VivoxVoiceManager _VVManager;
    [SerializeField] private GameObject chatBox; //the content of the chat box/scroll rect
    [SerializeField] private GameObject messagePrefab; // a prefab of a default message
    [SerializeField] private Button enterButton;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private EventSystem eventSystem;
    private ChannelId lobbyChannelId;
    private ScrollRect textScrollRect; //the scroll bar for the chat
    private List<GameObject> messageList = new List<GameObject>(); // holds all the messages that have been sent by the players
    private string lobbyChannelName = NetworkUIManager.joinCode;
    //private string lobbyChannelName = "lobbyChannel";
    // Start is called before the first frame update
    void Awake()
    {
        Setup();
        if (_VVManager.ActiveChannels.Count > 0)
        {
            lobbyChannelId = _VVManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == lobbyChannelName).Key;
        }
    }
    private void Setup()
    {
        _VVManager = VivoxVoiceManager.Instance;
        textScrollRect = GetComponent<ScrollRect>();
        _VVManager.OnParticipantAddedEvent += OnParticipantAdded;
        _VVManager.OnTextMessageLogReceivedEvent += OnTextMessageLogReceivedEvent;
        enterButton.onClick.AddListener(()=> { SubmitTextToChat(); });
        MessageFieldSetup();

    }
    private void MessageFieldSetup()
    {
        messageInputField.onEndEdit.AddListener((string text) => { EnterTextField();  Pause_Menu.isPaused = false; eventSystem.SetSelectedGameObject(null); }); 
        messageInputField.onSelect.AddListener((string text) => { Pause_Menu.isPaused = true; }); //if you are editing a message, game is paused
        messageInputField.onDeselect.AddListener((string text) => { Pause_Menu.isPaused = false; });
    }
    private void OnDestroy()
    {
        enterButton.onClick.RemoveAllListeners();
        messageInputField.onEndEdit.RemoveAllListeners();
        _VVManager.OnParticipantAddedEvent -= OnParticipantAdded;
        _VVManager.OnTextMessageLogReceivedEvent -= OnTextMessageLogReceivedEvent;

    }
    private void EnterTextField()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }
        SubmitTextToChat();
    }
    private void SubmitTextToChat()
    {
        if (string.IsNullOrEmpty(messageInputField.text))
        {
            return;
        }
        Debug.Log(messageInputField.text);

        _VVManager.SendTextMessage(messageInputField.text, lobbyChannelId);
        ClearInputField();
    }
    private void ClearInputField()
    {
        messageInputField.Select(); //selects this UI element 
        messageInputField.ActivateInputField();
        messageInputField.text = string.Empty; // clears the input field
    }

    /*public void DisplayHostingMessage(IChannelTextMessage channelTextMessage)
    {
        var newMessageObj = Instantiate(messagePrefab, chatBox.transform);
        messageList.Add(newMessageObj);
        TextMeshPro newMessageText = newMessageObj.GetComponent<TextMeshPro>();
    }
    */

    //#region Vivox Callbacks

    private IEnumerator ScrollDownToBottom()
    {
        yield return new WaitForEndOfFrame();

        // We need to wait for the end of the frame for this to be updated, otherwise it happens too quickly.
        textScrollRect.normalizedPosition = new Vector2(0, 0);

        yield return null;
    }
    void OnParticipantAdded(string username, ChannelId channel, IParticipant participant)
    {
        if (_VVManager.ActiveChannels.Count > 0)
        {
            lobbyChannelId = _VVManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == lobbyChannelName).Key;
        }
    }

    private void OnTextMessageLogReceivedEvent(string sender, IChannelTextMessage channelTextMessage)
    {
        if (!String.IsNullOrEmpty(channelTextMessage.ApplicationStanzaNamespace))
        {
            // If we find a message with an ApplicationStanzaNamespace we don't push that to the chat box.
            // Such messages mean opening/closing or requesting the open status of multiplayer matches.
            return;
        }
        var newMessageObj = Instantiate(messagePrefab, chatBox.transform);
        messageList.Add(newMessageObj);
        TMP_Text newMessageText = newMessageObj.GetComponent<TMP_Text>();

        if (channelTextMessage.FromSelf) //if it is my message
        {
            newMessageText.alignment = TextAlignmentOptions.TopRight;
            newMessageText.text = string.Format($"{channelTextMessage.Message} :<color=blue>{sender} </color>\n<color=#5A5A5A><size=7>{channelTextMessage.ReceivedTime}</size></color>");
            //formatting the string to have blue text for the sender, etc.
            StartCoroutine(ScrollDownToBottom());
        }
        else
        {
            newMessageText.alignment = TextAlignmentOptions.TopLeft;
            newMessageText.text = string.Format($"<color=green>{sender} </color>: {channelTextMessage.Message}\n<color=#5A5A5A><size=7>{channelTextMessage.ReceivedTime}</size></color>");
            Debug.Log(newMessageText.text);
        }
    }
}
