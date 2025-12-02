using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmonizer : MonoBehaviour
{
    public static Harmonizer Instance;
    public List<(List<Note>, int)> population = new List<(List<Note>, int)>();
    public List<(List<Note>, int)> newPopulation = new List<(List<Note>, int)>();

    public int populationSize;
    public List<string> notes = new List<string>{ "C", "D","E", "F", "G", "A", "B" };
    (int, int) parents;
    public float mutationProbability;
    public int mutateIterations;
    public bool elitism;


    private void Awake()
    {
        Instance = this;
    }

    public void Generate()
    {
        population.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        newPopulation.Clear();
        int eliteSubtract = 0;
        if (elitism)
        {
            eliteSubtract = 1;
            newPopulation.Add(population[population.Count - 1]);
            newPopulation.Add(population[population.Count - 2]);
        }
        for (int i=0; i<(populationSize/2 - eliteSubtract); i++)
        {
            SelectParents();
            (List<Note>, List<Note>) children = SinglePointCrossOver();
            Mutate(children.Item1, mutateIterations, mutationProbability);
            Mutate(children.Item2, mutateIterations, mutationProbability);
            newPopulation.Add((children.Item1, 0));
            newPopulation.Add((children.Item2, 0));
        }
        population.Clear();
        population = new List<(List<Note>, int)>(newPopulation);  
    }

    public void InitialisePopulation()
    {
        population.Clear();
        for(int i = 0; i<populationSize; i++)
        {
            List<Note> harmony = GenerateRandomHarmony();
            population.Add((harmony, 0));
        }
    }

    List<Note> GenerateRandomHarmony()
    {
        List<Note> harmony = new List<Note>();
        foreach(GameObject m in GameManager.Instance.melody)
        {
            Note note = new Note();
            note.name = notes[Random.Range(0, 7)];
            while(note.name == m.GetComponent<Note>().key.name)
            {
                note.name = notes[Random.Range(0, 7)];
            }
            harmony.Add(note);
            
        }
        return harmony;
    }


    public void SelectParents()
    {
        int p1 = SelectFromPopulation();
        int p2 = SelectFromPopulation();
        int i = 0;
        while(p1 == p2)
        {
            print("is this the issue?");
            p2 = SelectFromPopulation();
            i++;
        }
        parents.Item1 = p1;
        parents.Item2 = p2;
    }

    public int SelectFromPopulation()
    {
        int weightedSum = 0;
        for(int i = 0; i < population.Count; i++)
        {
            weightedSum = weightedSum + population[i].Item2;
        }
        int randomNumber = Random.Range(1, weightedSum + 1);

        weightedSum = 0;
        int index = -1;
        for (int i = 0; i < population.Count; i++)
        {
            weightedSum = weightedSum + population[i].Item2;
            if (weightedSum >= randomNumber)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    (List<Note>, List<Note>) SinglePointCrossOver()
    {
        int point = Random.Range(0, population[0].Item1.Count);
        List<Note> child1 = new List<Note>();
        child1.AddRange(population[parents.Item1].Item1.GetRange(0, point));
        child1.AddRange(population[parents.Item2].Item1.GetRange(point, population[0].Item1.Count - point));

        List<Note> child2 = new List<Note>();
        child2.AddRange(population[parents.Item2].Item1.GetRange(0, point));
        child2.AddRange(population[parents.Item1].Item1.GetRange(point, population[0].Item1.Count - point));


        return (child1, child2);


    }

    void Mutate(List<Note> genome, int iterations, float probability)
    {
        for (int i = 0; i<iterations; i++)
        {
            int index = Random.Range(0, genome.Count);
            float randomFloat = Random.Range(0f, 1f);
            if(randomFloat <= probability)
            {
                int noteIndex = notes.IndexOf(genome[index].name);
                if (noteIndex > 0)
                {
                    noteIndex = noteIndex - 1;
                }
                else
                {
                    noteIndex = noteIndex + 1;
                }
                if (notes[noteIndex] != GameManager.Instance.melody[index].GetComponent<Note>().name)
                {
                    genome[index].name = notes[noteIndex];
                }
            }
        }
    }



    
}
