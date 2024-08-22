using UnityEngine.UI;
using UnityEngine;

public class Timerr : MonoBehaviour
{
    public int seconds, minutes = 0;
    [SerializeField] private Text timerText;

    void Start()
    {
        AddToTimer();
    }

    private void AddToTimer()
    {
        if (!UIManager.isSelectMode) 
        {
            seconds = 0;
            minutes = 0;
        }
        seconds += 1;

        if (seconds > 59)
        {
            minutes += 1;
            seconds = 0;
        }
        timerText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        Invoke(nameof(AddToTimer), 1f);

    }
    public void EndTimer(Text text)
    {
        text.text = timerText.text;
        text.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    }

    public void StopTimer()
    {
        CancelInvoke(nameof(AddToTimer));
        timerText.gameObject.SetActive(false);
    }
}
