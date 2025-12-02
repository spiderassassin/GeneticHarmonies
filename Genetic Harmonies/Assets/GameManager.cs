using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform start;
    public Transform end;
    public Transform tracker;
    public bool isPlaying = false;
    public bool isRecording = false;
    public float speed;
    public float timer = 0f;
    public GameObject fill;
    public bool noteIsBeingPlayed = false;
    public List<GameObject> melody;
    public List<GameObject> generatedMelody;
    public GameObject RecordingButton;
    public GameObject GeneratePopulation;
    public bool ratingPhase = false;
    
    public GameObject RatingUI;
    public GameObject ReadyToEvolveUI;
    public GameObject ResetButton;
    public Slider mutationProbability;

    public GameObject parameters;
    public TMP_Dropdown mutationIterationDropDown;
    public TMP_Dropdown populationSize;
    public Toggle elitism;


    private void Awake()
    {
        Instance = this;
    }

        // Start is called before the first frame update
    void Start()
    {
        tracker.position = start.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isPlaying || isRecording)
        {
            timer = timer + Time.deltaTime;
            tracker.position = new Vector3(tracker.position.x + Time.deltaTime * speed, tracker.position.y);
        }
        if (isPlaying)
        {
            PlayingLogic(melody);
            PlayingLogic(generatedMelody);
        }

        if (tracker.position.x > end.position.x)
        {
            Rewind();
        }

        
    }

    public void Rewind() {
        if ((melody.Count > 0) && (ratingPhase == false))
        {
            parameters.SetActive(true);
            GeneratePopulation.SetActive(true);
        }
        
        foreach (Key k in KeyManager.Instance.keys)
        {
            k.StopNote();
            if (k.noteRecording)
            {
                k.noteRecording = false;
                k.currentNote.GetComponent<Note>().endTime = timer;
                k.notes.Add(k.currentNote);
                melody.Add(k.currentNote);
                k.currentNote = null;
            }
        }
        tracker.position = start.position;
        isPlaying = false;
        isRecording = false;
        timer = 0f;
    }

    public void Record()
    {
        parameters.SetActive(false);
        GeneratePopulation.SetActive(false);
        foreach (Key k in KeyManager.Instance.keys)
        {
            k.Reset();
            melody.Clear();
            k.StopNote();
        }
        tracker.position = start.position;
        isRecording = true;
        isPlaying = false;
        timer = 0f;
        
    }

    public void Play()
    {
        GeneratePopulation.SetActive(false);
        foreach (Key k in KeyManager.Instance.keys)
        {
            k.StopNote();
        }
        tracker.position = start.position;
        isPlaying = true;
        isRecording = false;
        timer = 0f;
    }

    public void GeneratePopulationLogic()
    {
        RecordingButton.SetActive(false);
        parameters.SetActive(false);
        Harmonizer.Instance.InitialisePopulation();
        GeneratePopulation.SetActive(false);
        ratingPhase = true;
        RatingUI.SetActive(true);
        RatingManager.Instance.startRatingPhase();
        ResetButton.SetActive(true);
    }

    public void DisplayGenome(List<Note> genome, Color color)
    {
        for (int i = 0; i<genome.Count; i++)
        {
            Key k = KeyManager.Instance.keys[Harmonizer.Instance.notes.IndexOf(genome[i].name)];
            GameObject newNote = Instantiate(melody[i], new Vector3(melody[i].transform.position.x, k.track.position.y), Quaternion.identity);
            newNote.GetComponent<Note>().key = k;
            newNote.GetComponent<Note>().name = k.name;
            newNote.GetComponentInChildren<SpriteRenderer>().color = color;
            generatedMelody.Add(newNote);

        }
    }

    public void ResetGeneratedMelody()
    {
        for (int i = 0; i < generatedMelody.Count; i++)
        {
            Destroy(generatedMelody[i]);
        }
        generatedMelody.Clear();


    }

    public void PlayingLogic(List<GameObject> sequence)
    {   
        for (int i = 0; i < sequence.Count; i++)
        {
            if (Mathf.Abs(timer - sequence[i].GetComponent<Note>().startTime) < 0.1f)
            {
                sequence[i].GetComponent<Note>().key.PlayNote();
            }

            if (Mathf.Abs(timer - sequence[i].GetComponent<Note>().endTime) < 0.1f)
            {
                sequence[i].GetComponent<Note>().key.StopNote();
            }
        }  
    }

    public void ShowBestInPopulation()
    {
        RatingUI.SetActive(false);
        ReadyToEvolveUI.SetActive(true);
        Harmonizer.Instance.population.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        DisplayGenome(Harmonizer.Instance.population[Harmonizer.Instance.populationSize - 1].Item1, Color.blue); ;
    }

    public void Evolve()
    {
        //print("hello?d");
        Harmonizer.Instance.Generate();
        ResetGeneratedMelody();
        ReadyToEvolveUI.SetActive(false);
        ratingPhase = true;
        RatingUI.SetActive(true);
        RatingManager.Instance.startRatingPhase();
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void OnMutationChange()
    {
        Harmonizer.Instance.mutationProbability = mutationProbability.value;
    }

    public void OnMutationIterationChange()
    {
        Harmonizer.Instance.mutateIterations = mutationIterationDropDown.value;
    }

    public void OnPopulationSizeChange()
    {
        Harmonizer.Instance.populationSize = (populationSize.value + 2) * 2;
    }

    public void OnElitismChange()
    {
        Harmonizer.Instance.elitism = elitism.isOn;
    }


}
