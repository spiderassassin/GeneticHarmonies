using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource sound;
    bool wiggle = false;
    public bool noteRecording = false;
    public Transform track;
    public List<GameObject> notes;
    public GameObject currentNote = null;
    public string name;
    public float speed;
    
    //TODO: Get ride of notes when all the functionality is completed

    private void Start()
    {
        
    }

    private void Update()
    {
        WiggleLogic();

        RecordingLogic();


    }

   

    public void RecordingLogic()
    {
        if (GameManager.Instance.isRecording == false)
        {
            currentNote = null;
        }

        if (currentNote != null)
        {
            currentNote.transform.localScale = new Vector3(currentNote.transform.localScale.x + Time.deltaTime * speed, currentNote.transform.localScale.y);
        }


    }
    public void WiggleLogic()
    {
        if (wiggle == true)
        {
            float angle = Mathf.Sin(Time.time * 25) * 10;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void PressedDown()
    {
        if (GameManager.Instance.noteIsBeingPlayed)
        {
            return;
            
        }
        PlayNote();
        GameManager.Instance.noteIsBeingPlayed = true;    
        if(GameManager.Instance.isRecording == true)
        {
            
            noteRecording = true;
            currentNote = Instantiate(GameManager.Instance.fill, new Vector3(GameManager.Instance.tracker.position.x, track.position.y), Quaternion.identity);
            print("New Note start" + GameManager.Instance.timer);
            currentNote.GetComponent<Note>().startTime = GameManager.Instance.timer;
            currentNote.GetComponent<Note>().key = this;
        }
        
    }

    public void PressedUp()
    {
        StopNote();
        GameManager.Instance.noteIsBeingPlayed = false;
        if (GameManager.Instance.isRecording == true)
        {
            if (noteRecording)
            {
                noteRecording = false;
                print("Old note end" + GameManager.Instance.timer);
                currentNote.GetComponent<Note>().endTime = GameManager.Instance.timer;
                notes.Add(currentNote);
                GameManager.Instance.melody.Add(currentNote);
                currentNote = null;
            }
           
        }
        
    }

    public void PlayNote()
    {
        if (!sound.isPlaying)
        {
            sound.Play();
        }
        
        
        
        wiggle = true;
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void StopNote()
    {
        sound.Stop();
        wiggle = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Reset()
    {
        foreach (GameObject n in notes)
        {
            Destroy(n);
        }
        notes.Clear();

    }
}
