using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits for the UIelement class and handles updating the score display
/// </summary>
public class WaveDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    /// <summary>
    /// Description:
    /// Updates the score display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void DisplayScore()
    {
        if (displayText != null)
        {
            var wave = GameManager.instance.GetWave();
            var text = "";

            for (int i = 0; i < ((int) wave / 5); i++)
            {
                text += "<i>" + StrikeThrough("IIII") + "</i> ";
            }

            for (int i = 0; i < (wave % 5); i++)
            {
                text += "I";
            }

            displayText.text = "Wave: " + text;
        }
    }

    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    /// <summary>
    /// Description:
    /// Overides the virtual UpdateUI function and uses the DisplayScore to update the score display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayScore();
    }
}
