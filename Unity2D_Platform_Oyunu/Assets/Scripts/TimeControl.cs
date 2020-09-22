using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] float time;
    private bool gameActive;
    // Start is called before the first frame update
    void Start()
    {
        gameActive = true;
        timeText.text = time.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)time).ToString();
        }
        
        if (time <= 0 && gameActive == true)
        {
            GetComponent<PlayerControler>().Die();
            gameActive = false;
        }
    }
}
