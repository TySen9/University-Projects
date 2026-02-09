using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PartyAISystem;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI finiteStateTextTMP;
    public TextMeshProUGUI behaviourExploreActionTMP;
    public TextMeshProUGUI behaviourBattleActionTMP;
    public TextMeshProUGUI subtitleTextTMP;

    [Header("Health and Mana Bars")]
    public Slider companionHealthBarSlider;
    public Slider companionManaBarSlider;
    public Slider playerHealthBarSlider;
    public Slider playerManaBarSlider;

    [Header("Cooldown Displays")]
    public TextMeshProUGUI attackCooldownText;
    public TextMeshProUGUI magicCooldownText;
    public TextMeshProUGUI itemCooldownText;

    [Header("Subtitle Lines")]
    [SerializeField] private string[] battleSubtitleLines = {
        "Run them through!",
        "Take'em down!",
        "Let's finish this!",
        "We've been through worse!"
    }; // Array of subtitle lines for the companion

    [SerializeField] private string[] exploreSubtitleLines = {
        "Where we off to?",
        "I think a rest sounds good right now",
        "*Hums a theme*",
    }; // Array of subtitle lines for the companion

    [SerializeField] private string[] healSubtitleLines =
    {
        "Looking Injured there, let me help you",
        "You look like you need a hand",
        "I got you covered",
        "Healing up!"
    };

    [SerializeField] private string[] magicSubtileLines =
    {
        "Blow them away!",
        "They'll feel this one!",
        "Big magic boom."
    };

    public Statistics playerStatistics; // Reference to the player's statistics
    public Statistics companionStatistics; // Reference to the companion's statistics
    private Coroutine subtitleFadeCoroutine; // Coroutine for fading subtitles
    private float subtitleFadeDuration = 2f; // Duration for fading subtitles
    private float exploreSubtitleCooldown = 15f; // Cooldown for explore subtitles
    private float exploreSubtitleTimer = 0f; // Timer for explore subtitles


    public static UIManager Instance;

    private void Awake()
    {
        companionHealthBarSlider = GameObject.FindGameObjectWithTag("CompanionHealthBar").GetComponent<Slider>();
        companionManaBarSlider = GameObject.FindGameObjectWithTag("CompanionManaBar").GetComponent<Slider>();
        playerHealthBarSlider = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Slider>();
        playerManaBarSlider = GameObject.FindGameObjectWithTag("PlayerManaBar").GetComponent<Slider>();

        playerStatistics = GameObject.FindGameObjectWithTag("Player").GetComponent<Statistics>();
        companionStatistics = GameObject.FindGameObjectWithTag("Companion").GetComponent<Statistics>();

        attackCooldownText = GameObject.FindGameObjectWithTag("AttackCooldownText").GetComponent<TextMeshProUGUI>();
        magicCooldownText = GameObject.FindGameObjectWithTag("MagicCooldownText").GetComponent<TextMeshProUGUI>();
        itemCooldownText = GameObject.FindGameObjectWithTag("ItemCooldownText").GetComponent<TextMeshProUGUI>();

        finiteStateTextTMP = GameObject.FindGameObjectWithTag("FiniteStateText").GetComponent<TextMeshProUGUI>();
        behaviourExploreActionTMP = GameObject.FindGameObjectWithTag("BehaviourExploreActionText").GetComponent<TextMeshProUGUI>();
        behaviourBattleActionTMP = GameObject.FindGameObjectWithTag("BehaviourBattleActionText").GetComponent<TextMeshProUGUI>();
        subtitleTextTMP = GameObject.FindGameObjectWithTag("SubtitleText").GetComponent<TextMeshProUGUI>();

        bool nullFields = finiteStateTextTMP == null || behaviourExploreActionTMP == null || behaviourBattleActionTMP == null;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (nullFields)
        {
            Debug.LogError("UIManager: One or more TextMeshProUGUI fields are not assigned in the inspector. Please Assign.");
        }
    }

    public void Start()
    {
        companionHealthBarSlider.maxValue = companionStatistics.GetMaxHealth();
        companionHealthBarSlider.value = companionStatistics.GetCurrentHealth();

        companionManaBarSlider.maxValue = companionStatistics.GetMaxMana();
        companionManaBarSlider.value = companionStatistics.GetCurrentMana();

        playerHealthBarSlider.maxValue = playerStatistics.GetMaxHealth();
        playerHealthBarSlider.value = playerStatistics.GetCurrentHealth();

        playerManaBarSlider.maxValue = playerStatistics.GetMaxMana();
        playerManaBarSlider.value = playerStatistics.GetCurrentMana();
    }

    public void Update()
    {
        playerHealthBarSlider.value = playerStatistics.GetCurrentHealth();
        playerManaBarSlider.value = playerStatistics.GetCurrentMana();

        companionHealthBarSlider.value = companionStatistics.GetCurrentHealth();
        companionManaBarSlider.value = companionStatistics.GetCurrentMana();

        exploreSubtitleTimer += Time.deltaTime;

        UpdateCooldownDisplays();
    }

    public void ShowRandomBattleSubtitle()
    {
       if (subtitleTextTMP != null)
        {
            string randomLine = battleSubtitleLines[Random.Range(0, battleSubtitleLines.Length)];
            ShowSubtitleWithFade(randomLine);
        }
    }

    public void ShowRandomExploreSubtitle()
    {
        if (subtitleTextTMP == null) return;

        if (exploreSubtitleTimer >= exploreSubtitleCooldown)
        {
            string randomLine = exploreSubtitleLines[Random.Range(0, exploreSubtitleLines.Length)];
            ShowSubtitleWithFade(randomLine);
            exploreSubtitleTimer = 0f; // Reset the timer after showing a subtitle
        }
    }

    public void ShowRandomHealSubtitle()
    {
        if (subtitleTextTMP != null)
        {
            string randomLine = healSubtitleLines[Random.Range(0, healSubtitleLines.Length)];
            ShowSubtitleWithFade(randomLine);
        }
    }

    public void ShowRandomMagicSubtitle()
    {
        if (subtitleTextTMP != null)
        {
            string randomLine = magicSubtileLines[Random.Range(0, magicSubtileLines.Length)];
            ShowSubtitleWithFade(randomLine);
        }
    }

    public void ShowSubtitleWithFade(string subtitle, float visableDuration = 2f)
    {
        if (subtitleTextTMP == null)
        {
            Debug.LogError("UIManager: SubtitleTextTMP is not assigned in the inspector.");
            return;
        }

        if (subtitleFadeCoroutine !=null)
        {
            StopCoroutine(subtitleFadeCoroutine);
        }

        subtitleFadeCoroutine = StartCoroutine(FadeSubtitleRoutine(subtitle, visableDuration));
    }

    private IEnumerator FadeSubtitleRoutine(string subtitle, float visableDuration)
    {
        subtitleTextTMP.text = "Companion: " + subtitle;

        //start transparent first
        Color colour = subtitleTextTMP.color;
        colour.a = 0f;
        subtitleTextTMP.color = colour;

        //fade in over time
        float fadeInTime = 0.5f;
        float fadeInElapsed = 0f;
        while (fadeInElapsed < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, fadeInElapsed / fadeInTime);
            colour.a = alpha;
            subtitleTextTMP.color = colour;

            fadeInElapsed += Time.deltaTime;
            yield return null;
        }

        //Ensures it is fully visible at the end
        colour.a = 1f;
        subtitleTextTMP.color = colour;

        //Wait before fading
        yield return new WaitForSeconds(visableDuration);

        //fade out over time
        float fadeOutElapsed = 0f;
        while (fadeOutElapsed < subtitleFadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeOutElapsed / subtitleFadeDuration);
            colour.a = alpha;
            subtitleTextTMP.color = colour;

            fadeOutElapsed += Time.deltaTime;
            yield return null;
        }

        //Ensures it is fully invisible at the end
        colour.a = 0f;
        subtitleTextTMP.color = colour;
        subtitleTextTMP.text = ""; // Clear the text after fading out
    }

    private void UpdateCooldownDisplays()
    {
        float attackRemainingCooldown = CooldownManager.TimeRemaining("Physical");
        attackCooldownText.text = attackRemainingCooldown > 0 ? $"Attack Cooldown: {attackRemainingCooldown:F1}s" : "Attack Ready";

        float magicRemainingCooldown = CooldownManager.TimeRemaining("Magic");
        magicCooldownText.text = magicRemainingCooldown > 0 ? $"Magic Cooldown: {magicRemainingCooldown:F1}s" : "Magic Ready";

        float itemRemainingCooldown = CooldownManager.TimeRemaining("Defense");
        itemCooldownText.text = itemRemainingCooldown > 0 ? $"Item/Defense Cooldown: {itemRemainingCooldown:F1}s" : "Item/Defense Ready";
    }

    public void UpdateFiniteStateText(string state)
    {
        if (finiteStateTextTMP != null)
        {
            finiteStateTextTMP.text = "FSM State: " + state;
        }
        else
        {
            Debug.LogError("UIManager: FiniteStateTextTMP is not assigned in the inspector.");
        }
    }

    public void UpdateSubtitle(string subtitle)
    {
        if (subtitleTextTMP != null)
        {
            subtitleTextTMP.text = "Companion: " + subtitle;
        }
        else
        {
            Debug.LogError("UIManager: SubtitleTextTMP is not assigned in the inspector.");
        }
    }

    public void UpdateBehaviourExploreActionText(string exploreAction)
    {
        if (behaviourExploreActionTMP != null)
        {
            behaviourExploreActionTMP.text = "Explore Action: " + exploreAction;
        }
        else
        {
            Debug.LogError("UIManager: BehaviourExploreActionTMP is not assigned in the inspector.");
        }
    }

    public void UpdateBehaviourBattleActionText(string battleAction)
    {
        if (behaviourBattleActionTMP != null)
        {
            behaviourBattleActionTMP.text = "Battle Action: " + battleAction;
        }
        else
        {
            Debug.LogError("UIManager: BehaviourBattleActionTMP is not assigned in the inspector.");
        }
    }
}
