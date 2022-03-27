using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;
using System.Linq;
using System;

public class ChatRoomManager : MonoBehaviour
{
    private string LobbyChannelName = NetworkUIManager.joinCode;
    private VivoxVoiceManager _VVManager;

    private Dictionary<ChannelId, List<ParticipantItem>> participantObjs = new Dictionary<ChannelId, List<ParticipantItem>>();

    [SerializeField] private GameObject participantPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        Setup();
        if (_VVManager && _VVManager.ActiveChannels.Count > 0)
        {
            var LobbyChannel = _VVManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == LobbyChannelName);
            //gets the first channel that matches the name, or if none, gets the default option
            foreach (var participant in _VVManager.LoginSession.GetChannelSession(LobbyChannel.Channel).Participants)
            {
                UpdateParticipants(participant, participant.ParentChannelSession.Channel, true);
                //when we start, if there are already people in the channel, update participants
            }
        }
    }

    private void OnDestroy()
    {
        _VVManager.OnParticipantAddedEvent -= OnClientAdded;
        _VVManager.OnParticipantRemovedEvent -= OnClientRemoved;
        _VVManager.OnUserLoggedOutEvent -= OnUserLoggedOut;    
    }
    private void UpdateParticipants(IParticipant participant, ChannelId channel, bool addParticipant)
    {
        if (addParticipant) //if the boolean is true
        {
            GameObject newParticipantObj = Instantiate(participantPrefab, gameObject.transform); //spawns prefab at this gameobject transform
            //this gameobject is the panel on the left
            ParticipantItem newParticipantInfoItem = newParticipantObj.GetComponent<ParticipantItem>();
            List<ParticipantItem> thisChannelList;

            if (participantObjs.ContainsKey(channel))
            {
                //add object to the existing of participants
                participantObjs.TryGetValue(channel, out thisChannelList);
                //tries to get the value associated with this channel key in the dictionary
                newParticipantInfoItem.SetupParticipantItem(participant);
                participantObjs[channel] = thisChannelList;
            }
            else
            {
                //if this channel does not exist in the dictionary (i.e, new channel)
                //create a new list of participants (called a roster)
                thisChannelList = new List<ParticipantItem>();
                thisChannelList.Add(newParticipantInfoItem);
                newParticipantInfoItem.SetupParticipantItem(participant);
                participantObjs.Add(channel, thisChannelList);
            }
            UpdateRosterUI(channel);
        }
        else
        {
            if (participantObjs.ContainsKey(channel))
            {
                //if the channel exists
                ParticipantItem removedItem = participantObjs[channel].FirstOrDefault(p => p.Participant.Account.Name == participant.Account.Name);
                //the item to remove is selected through the participants account name, first or default
                if (removedItem != null)
                {
                    participantObjs[channel].Remove(removedItem);
                    Destroy(removedItem.gameObject);
                    UpdateRosterUI(channel);
                }
                else
                {
                    Debug.Log("removing item that does not exist in channel");
                }
            }
        }

    }
    private void UpdateRosterUI(ChannelId channel)
    {
        //resizes the roster of participants to fit number of participants
        RectTransform rosterBox = gameObject.GetComponent<RectTransform>();
        rosterBox.sizeDelta = new Vector2(0, 50 * participantObjs[channel].Count());
        //multiplies the size of the roster box by how many participants there are in the channel

    }
    private void ClearAllParticipants()
    {
        foreach (List<ParticipantItem> participantList in participantObjs.Values)
        { //participant Objs contains a pairing of Channel Id + List<ParticipantItem>
            foreach (ParticipantItem item in participantList)
            {
                //for each participant in the channel, destroy the UI
                Destroy(item.gameObject);
            }
            participantList.Clear(); //clear the list
        }
        participantObjs.Clear(); //clear the dictionary
    }
    private void Setup()
    {
        _VVManager = VivoxVoiceManager.Instance;
        _VVManager.OnParticipantAddedEvent += OnClientAdded;
        _VVManager.OnParticipantRemovedEvent += OnClientRemoved;
        _VVManager.OnUserLoggedOutEvent += OnUserLoggedOut;
    }
    private void OnClientAdded(string username, ChannelId channel, IParticipant participant)
    {
        Debug.Log("Added" + username);
        UpdateParticipants(participant, channel, true); //we want to add the participant to the chat room
    }
    private void OnClientRemoved(string username, ChannelId channel, IParticipant participant)
    {
        Debug.Log("Removed" + participant.Account.Name);
        UpdateParticipants(participant, channel, false); // we want to remove the participant from the chat room
    }
    private void OnUserLoggedOut()
    {
        ClearAllParticipants();
    }
}