using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int totalShots = 5;
    public int goals = 0;
    public int currentShot = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;
    public BallShooter shooter;
    public Goalkeeper goalkeeper;
    private bool resultDecided = false;
    void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
    }
    public void RegisterShot()
    {
        resultDecided = false;
    }
    public void GoalScored()
    {
        if (resultDecided) return;
        resultDecided = true;
        goals++;
        UpdateUI();
        Invoke(nameof(NextShot), 2f);
    }
    public void ShotSaved()
    {
        if (resultDecided) return;
        resultDecided = true;
        UpdateUI();
        Invoke(nameof(NextShot), 2f);
    }
    public void ShotMissed()
    {
        if (resultDecided) return;
        resultDecided = true;
        UpdateUI();
        Invoke(nameof(NextShot), 2f);
    }
    void NextShot()
    {
        currentShot++;
        if (currentShot >= totalShots)
        {
            EndGame();
            return;
        }
        shooter.ResetBall();
        if (goalkeeper != null)
            goalkeeper.ResetGoalkeeper();
    }
    void UpdateUI()
    {
        scoreText.text = "Goals: " + goals + " / " + totalShots;
    }
    void EndGame()
    {
        if (goals >= 3)
            resultText.text = "YOU WIN!";
        else
            resultText.text = "YOU LOSE!";
        resultText.gameObject.SetActive(true);
        Invoke(nameof(RestartGame), 3f);
    }
    void RestartGame()
    {
        goals = 0;
        currentShot = 0;
        resultDecided = false;
        UpdateUI();
        resultText.gameObject.SetActive(false);
        shooter.ResetBall();
        if (goalkeeper != null)
            goalkeeper.ResetGoalkeeper();
    }
}