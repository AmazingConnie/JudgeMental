using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class VRCharacterDialogue : MonoBehaviour
{
    [Header("AI Settings")]
    [TextArea(3, 10)]
    public string characterPrompt = "You are a tiny ballif who works for a judge that hates her job but must work in order to make ends meet.  You will help the player with instructions and make quirky remarks.  You are also a raging alchoholic who will take a sip at your flask when you feel sick of it all.";

    [Header("Dialogue References")]
    public Transform dialogueAnchor;
    public GameObject speechBubblePrefab;
    public AudioClip animalCrossingSound;

    private OpenAIChatbot chatbot;
    private GameObject currentSpeechBubble;
    private AudioSource audioSource;
    private bool canInteract = false;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize chatbot
        chatbot = gameObject.AddComponent<OpenAIChatbot>();

        // Set up VR interaction
        var interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnInteract);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            Debug.Log("Player can now interact with the ballif");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            Debug.Log("Player can no longer interact with the ballif");
        }
    }

    private void OnInteract(SelectEnterEventArgs args)
    {
        if (canInteract)
        {
            StartCoroutine(StartDialogue());
        }
    }

    private IEnumerator StartDialogue()
    {
        // Show "listening" state
        if (currentSpeechBubble != null) Destroy(currentSpeechBubble);
        currentSpeechBubble = Instantiate(speechBubblePrefab, dialogueAnchor);
        currentSpeechBubble.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "...";

        // Simulate player input
        string playerInput = "What did you see that night?";
        
        // Get AI response
        yield return chatbot.GetResponse(characterPrompt, playerInput, HandleAIResponse);
    }

    private void HandleAIResponse(string response)
    {
        // Update speech bubble text
        if (currentSpeechBubble != null)
        {
            currentSpeechBubble.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = response;
        }

        // Play sound effect
        if (animalCrossingSound != null)
        {
            audioSource.PlayOneShot(animalCrossingSound);
        }
        else
        {
            Debug.LogWarning("Animal Crossing sound effect not assigned!");
        }
    }
}