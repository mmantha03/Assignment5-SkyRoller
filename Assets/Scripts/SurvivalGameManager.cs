using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SurvivalGameManager : MonoBehaviour
{
    public static SurvivalGameManager Instance;

    public Transform player;
    public PlayerMovement playerMovement;
    public TMP_Text scoreText;
    public TMP_Text gameOverText;
    public float fallHeight = -8f;

    bool gameOver;
    float startZ;
    int score;
    string loseMessage = "Game Over";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (player != null)
        {
            startZ = player.position.z;
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        UpdateScoreText();
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        if (player == null)
        {
            return;
        }

        score = Mathf.Max(0, Mathf.FloorToInt(player.position.z - startZ));
        UpdateScoreText();

        if (player.position.y < fallHeight)
        {
            LoseGame("You fell!");
        }
    }

    public void LoseGame(string message)
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
        loseMessage = message;

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        UpdateScoreText();
        UpdateGameOverText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void UpdateGameOverText()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(gameOver);
            gameOverText.text = loseMessage + "\nScore: " + score + "\nPress R to Restart";
        }
    }

    void OnGUI()
    {
        if (scoreText == null)
        {
            GUIStyle scoreStyle = new GUIStyle(GUI.skin.label);
            scoreStyle.fontSize = 28;
            scoreStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(20f, 18f, 300f, 60f), "Score: " + score, scoreStyle);
        }

        if (!gameOver || gameOverText != null)
        {
            return;
        }

        GUIStyle gameOverStyle = new GUIStyle(GUI.skin.label);
        gameOverStyle.fontSize = 42;
        gameOverStyle.alignment = TextAnchor.MiddleCenter;
        gameOverStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(0f, Screen.height * 0.35f, Screen.width, 220f), loseMessage + "\nScore: " + score + "\nPress R to Restart", gameOverStyle);
    }
}
