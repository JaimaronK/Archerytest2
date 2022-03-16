using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Text Score; // Score text display
    public Text Close; // Text for if yor shoot too close
    float timehit; // Fade time for text if you shoot too close
    public static int score = 0; // Score to be displayed
    public void Start()
    {
        Close.enabled = false; // Prevents too close text from showing up at start
    }

    public void Update()
    {
        Score.text = "Score: " + score; //Shows Score constantly in the corner
        Disable(); // Disables too close text after a set time has passed.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            if (transform.position.z - Arrow.launchzone >= 15 && Arrow.launched == true)
            {
                score++; //Increases the score by 1 if the arrow was launched and the launch position was atleast 15 measurements away from the target.
            }
            else if (transform.position.z - Arrow.launchzone < 15 && Arrow.launched == true)
            {
                Close.enabled = true; // Shows the too close to target hit
                timehit = Time.time; // Checks time for fade
            }
            Arrow.launched = false;
        }
    }

    private void Disable()
    {
        if (timehit + 10 < Time.time)
        Close.enabled = false; // disables the too close text;
    }
}
