using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI timerText;
    public Transform playerTransform;
    public GameObject bestScoreTextObject;
    public GameObject newBestTextObject;
    public Animator CoinMaskAnimator;
    public GameObject CoinCurrentMask;
    public ParticleSystem confettiParticleSystem;
    public AudioSource confettiAudioSource;
    public AudioSource TimerSound;
    public float activationDelay = 0.6f;
    public float activationScoreLimit = 0.6f;
    public float timerValue = 15.0f;
    public PlayerController playerController;
    public Camera mainCamera;
    public Transform cityDayParent;
    public Transform cityNightParent;
    public SpriteRenderer starsRenderer;
    public float cityTransitionSpeed = 0.5f;
    public int midScoreThreshold = 160;
    public int highScoreThreshold = 250;
    public GameObject clouds;  
    public GameObject birds;  

    private Animator cameraAnimator;
    private SpriteRenderer[] dayCityRenderers;
    private SpriteRenderer[] nightCityRenderers;
    private bool gameIsRunning = false;
    private int score = 0;
    private int bestScore = 0;
    private float highestYPosition;
    private bool hasMidScoreTransitioned = false; // Ensure the mid score transition only happens once
    private bool hasHighScoreTransitioned = false; // Ensure the high score transition only happens once
    private static bool hasShownGuide = false;

    private void Awake()
    {
        cameraAnimator = mainCamera.GetComponent<Animator>();
        dayCityRenderers = cityDayParent.GetComponentsInChildren<SpriteRenderer>();
        nightCityRenderers = cityNightParent.GetComponentsInChildren<SpriteRenderer>();

        SetChildrenAlpha(nightCityRenderers, 0f);
        starsRenderer.color = new Color(1f, 1f, 1f, 0f);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        highestYPosition = float.NegativeInfinity;
        gameIsRunning = false;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = bestScore.ToString() + "m";
        highestYPosition = playerTransform.position.y;
        bestScoreTextObject.SetActive(true);
        newBestTextObject.SetActive(false);
        CoinCurrentMask.SetActive(false);


    }

    private void Update()
    {
        if (!gameIsRunning) return;

        UpdateScore();
        UpdateScoreTexts();
        UpdateTimer();

        if (timerValue <= 0)
        {
            if (playerController.HasShield())
            {
                playerController.BreakShield();
                ResetTimer();
            }
            else
            {
                playerController.Die();
                timerValue = 0;
                gameIsRunning = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) // Press the 'R' key to reset best score
        {
            ResetBestScore();
        }


    }

    public int GetCurrentScore()
    {
        return score;
    }

    private void UpdateScore()
    {
        if (!gameIsRunning) return;

        // Only update the score if the player is getting deeper
        if (playerTransform.position.y < highestYPosition)
        {
            highestYPosition = playerTransform.position.y;
            score = Mathf.Abs(Mathf.FloorToInt(highestYPosition));
        }

        // Mid score transition to Sunset animation
        if (score >= midScoreThreshold && !hasMidScoreTransitioned)
        {
            cameraAnimator.SetTrigger("ToSunset");  // This triggers the ToSunset animation
            hasMidScoreTransitioned = true;
        }

        // High score transition to Night animation
        if (score >= highScoreThreshold && !hasHighScoreTransitioned)
        {
            cameraAnimator.SetTrigger("ToNight");  // This triggers the ToNight animation
            StartCoroutine(TransitionToNight());   // This will transition the city as well
            hasHighScoreTransitioned = true;
            clouds.SetActive(false);
            birds.SetActive(false);
        }
    }

    private IEnumerator TransitionToNight()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / cityTransitionSpeed;

            float sqrtT = Mathf.Sqrt(t);

            SetChildrenAlpha(nightCityRenderers, sqrtT);
            SetChildrenAlpha(dayCityRenderers, 1f - sqrtT);

            Color starsColor = starsRenderer.color;
            starsColor.a = sqrtT;
            starsRenderer.color = starsColor;

            yield return null;
        }

        SetChildrenAlpha(nightCityRenderers, 1f);
        SetChildrenAlpha(dayCityRenderers, 0f);

        Color finalStarsColor = starsRenderer.color;
        finalStarsColor.a = 1;
        starsRenderer.color = finalStarsColor;
    }

    private void UpdateScoreTexts()
    {
        scoreText.text = score.ToString() + "m";
        currentScoreText.text = score.ToString() + "m";
        timerText.text = Mathf.Round(timerValue).ToString() + "s";
    }

    private void UpdateTimer()
    {
        if (!gameIsRunning) return;

        timerValue -= Time.deltaTime;
        if (timerValue < 0)
        {
            timerValue = 0;
        }
    }

    public void ResetTimer()
    {
        timerValue = 16.0f;
        TimerSound.Play();
    }

    public void StartGame()
    {
        // Check if the guide should be shown
        if (bestScore == 0 && !hasShownGuide)
        {
            hasShownGuide = true;
            SceneManager.LoadScene(1);
        }

        gameIsRunning = true;

        CoinMaskAnimator.SetTrigger("Go");
        CoinCurrentMask.SetActive(true);


    }

    public void EndGame()
    {
        gameIsRunning = false;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = bestScore.ToString() + "m";
            newBestTextObject.SetActive(true);
            bestScoreTextObject.SetActive(false);
            confettiParticleSystem.Play();
            confettiAudioSource.Play();
        }
        else
        {
            newBestTextObject.SetActive(false);
            bestScoreTextObject.SetActive(true);
        }

        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void SetChildrenAlpha(SpriteRenderer[] renderers, float alpha)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    public void ResetBestScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        bestScore = 0;
        bestScoreText.text = "0m"; // Update the displayed score
        PlayerPrefs.Save(); // Save changes
    }

}































