using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;
using TMPro;

public class ParticipantItem : MonoBehaviour
{
    private VivoxVoiceManager _VVManager;
    public IParticipant Participant;
    public TMP_Text playerNameText;
    [SerializeField] private Animator animator;
    [SerializeField] private Image participantStateImg;
    /*
    public Sprite MutedImage;
    public Sprite SpeakingImage;
    public Sprite NotSpeakingImage;
    */
    private void OnEnable()
    {
        UpdateParticipantImageAnim(); //this is to update the animation of the sprite everytime the chat menu is closed then reopened
    }
    private bool isMuted;
    public bool IsMuted
    {
        get { return isMuted;}
        private set
        {
            /*
            if (Participant.IsSelf)
            {
                //if this script is running for the local user
                _VVManager.AudioInputDevices.Muted = value;
            }
            else
            {
                if (Participant.InAudio)
                {
                    //checks if the other participant is muted too, as we want to update their state
                    Participant.LocalMute = value;
                }
            }
            */
            isMuted = value;
            UpdateParticipantImageAnim();
        }
    }
    private bool isSpeaking;
    public bool IsSpeaking
    {
        get { return isSpeaking; }
        private set
        {
            if (!IsMuted && participantStateImg)
            {
                //if we aren't muted and the participant's image exists
                isSpeaking = value;
                UpdateParticipantImageAnim();
            }
        }
    }
    private void UpdateParticipantImageAnim()
    {
        animator.SetBool("isMuted", isMuted);
        animator.SetBool("isSpeaking", isSpeaking);
    }
    public void SetupParticipantItem(IParticipant participant)
    {
        _VVManager = VivoxVoiceManager.Instance;
        Participant = participant;
        playerNameText.text = participant.Account.DisplayName;
        IsMuted = participant.IsSelf ? _VVManager.AudioInputDevices.Muted : Participant.LocalMute;
        //if we are the participant, we use the local device's mute, otherwise we use the mute boolean in the participant class
        IsSpeaking = participant.SpeechDetected;
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            IsMuted = !IsMuted;
        });
        Participant.PropertyChanged += (obj, args) => //when the participant has one of their properties changed
        {
            switch (args.PropertyName)
            { //if what has been changed is the SpeechDetected boolean
                case "SpeechDetected":
                    IsSpeaking = Participant.SpeechDetected;
                    break;
            }
        };
    }
}
