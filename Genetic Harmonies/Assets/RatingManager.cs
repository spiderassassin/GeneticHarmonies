using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatingManager : MonoBehaviour
{
    public TMP_Dropdown dropDown;
    public static RatingManager Instance;
    public TMP_Text populationIterationText;
    public GameObject RateButton;
    public GameObject RateText;
    int i = 0;//Rating Iterator
    

    private void Awake()
    {
        Instance = this;
    }

    public void startRatingPhase()
    {
        i = 0;
        populationIterationText.text = "GENOME: " + (i + 1).ToString() + "/" + Harmonizer.Instance.populationSize;
        FetchPopulationGenome();
    }
    void FetchPopulationGenome()
    {  
        GameManager.Instance.DisplayGenome(Harmonizer.Instance.population[i].Item1, Color.yellow);
    }

    public void SetRating()
    {
        RateButton.SetActive(false);
        RateText.SetActive(true);
        GameManager.Instance.ResetGeneratedMelody();
        Harmonizer.Instance.population[i] = (Harmonizer.Instance.population[i].Item1, dropDown.value + 1);
        
        i = i + 1;
        if (i >= Harmonizer.Instance.populationSize)
        {
            GameManager.Instance.ShowBestInPopulation();
            return;
        }
        populationIterationText.text = "GENOME: " + (i + 1).ToString() + "/" + Harmonizer.Instance.populationSize;
        FetchPopulationGenome();

    }
    public void EnableRateButton()
    {
        RateButton.SetActive(true);
        RateText.SetActive(false);
    }

}
