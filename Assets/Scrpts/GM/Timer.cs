using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Timer : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    private GameManager gameManager;
    private float preTimerDuration = 3f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartTimer()
    {
        StartCoroutine(PreTimerCountdown());
    }

    private IEnumerator PreTimerCountdown()
    {
        float countdown = preTimerDuration;
        while (countdown > 0)
        {
            timerText.text = countdown.ToString("0");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        timerText.text = "Go!";
        yield return new WaitForSeconds(1f);

        remainingTime = 100f;
        timerText.color = Color.white;
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return null;
        }

        remainingTime = 0;
        timerText.color = Color.red;
        gameManager.EndRound();
    }
}
