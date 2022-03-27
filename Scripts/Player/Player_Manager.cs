using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player_Manager : NetworkBehaviour
{
    public GameObject DeathEffect;
    public bool invincible = false;
    int flash_counter = 0;
    SpriteRenderer player;
    public static bool GameOver = false;
    public GameObject interactableObject;
    [SerializeField] private NetworkVariable<bool> interacted = new NetworkVariable<bool>();
    [SerializeField] private NetworkVariable<NetworkString> interactedObjectClient = new NetworkVariable<NetworkString>();
    [SerializeField] private NetworkVariable<bool> takeDamage = new NetworkVariable<bool>();
    [SerializeField] private NetworkVariable<float> takenDamageNum = new NetworkVariable<float>(); 
    private bool old_interactedBool;
    private bool old_takeDamageBool;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<SpriteRenderer>(); //gets the sprite component from the current object
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && IsClient)
        {
            if (Input.GetKeyDown("o") && interactableObject != null)
            {
                TriggerInteraction(interactableObject);
                UpdateInteractableObjectServerRpc(interactableObject.name);
            }
            if (Input.GetKeyUp("o"))
            {
                UpdateInteractedBoolServerRpc(false);
            }
        }
        if (interacted.Value == true && !IsOwner)
        {
            if (interacted.Value != old_interactedBool)
            {
                string name = interactedObjectClient.Value.ToString();
                GameObject obj = GameObject.Find(name);
                TriggerInteraction(obj);
                Debug.Log("oh no");
            }
        }
        if (interacted.Value != old_interactedBool)
        {
            old_interactedBool = interacted.Value;
        }
        if (takeDamage.Value == true && !IsOwner)
        {
            if (takeDamage.Value != old_takeDamageBool)
            {
                TakeDamage(takenDamageNum.Value);
                //UpdateTakeDamageBoolServerRpc(false);
            }
        }
        if (takeDamage.Value != old_takeDamageBool)
        {
            old_takeDamageBool = takeDamage.Value;
        }
        if (Level_Switch.transition_anim)
        {
            StopAllCoroutines();
        }
    }
    private void TriggerInteraction(GameObject interactableObject)
    {
        if (interactableObject.GetComponent<Switch>() != null)
        {
            interactableObject.GetComponent<Switch>().FlipSwitch();
        }
        else
        {
            interactableObject.GetComponent<Completion_Button>().PressButton();
        }
    }
    public void TakeDamage(float damage)
    {
        if (IsOwner && !Level_Switch.transition)
        {
            Player_Data.health -= damage;
            StartCoroutine(Flash(8)); // flashes 4 times total, 4 non colour, 4 colour
            if (Player_Data.health <= 0)
            {
                Instantiate(DeathEffect, transform.position, transform.rotation);
                Destroy(gameObject);
                GameOver = true;
            }
            FindObjectOfType<On_ScreenUI>().ChangeHearts(damage);
            UpdateTakeDamageBoolServerRpc(true);
            UpdateTakeDamageValueServerRpc(damage);
        }
        else 
        {
            if (takeDamage.Value == true) //we don't want the other player to be registering damage not in their session
            {
                Player_Data.health_2 -= damage;
                StartCoroutine(Flash(8)); // flashes 4 times total, 4 non colour, 4 colour
                if (Player_Data.health_2 <= 0)
                {
                    Instantiate(DeathEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                    GameOver = true;
                }
                FindObjectOfType<On_ScreenUI>().ChangeHeartsPlayer2(damage);
            }
        }
    }
    IEnumerator Flash(int count)
    {
        float value = 1f;
        invincible = true;
        flash_counter += 1;
        while (flash_counter < count) // this controls how many times it flashes
        {
            player.color = new Color(player.color.r, player.color.g, player.color.b, 1 - value);
            yield return new WaitForSeconds(0.15f);
            value = -value;
            flash_counter += 1;
        }
        player.color = new Color(player.color.r, player.color.g, player.color.b, 1f);
        flash_counter = 0;
        invincible = false;
    }

    [ServerRpc]
    public void UpdateInteractableObjectServerRpc(string objectName)
    {
        interactedObjectClient.Value = objectName;
        interacted.Value = true;
    }
    [ServerRpc]
    public void UpdateInteractedBoolServerRpc(bool value)
    {
        interacted.Value = value;
    }
    [ServerRpc]
    public void UpdateTakeDamageBoolServerRpc(bool value)
    {
        takeDamage.Value = value;
    }
    [ServerRpc]
    public void UpdateTakeDamageValueServerRpc(float value)
    {
        takenDamageNum.Value = value;
    }
}
