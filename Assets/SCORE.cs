using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SCORE : MonoBehaviour
{
    private static SCORE instance;
    public static SCORE Instance
    {
        get
        {
            // Check if the instance is null
            if (instance == null)
            {
                // Find the singleton instance in the scene
                instance = FindObjectOfType<SCORE>();

                // If no instance found, create a new GameObject and attach the singleton component to it
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SCORE).Name);
                    instance = singletonObject.AddComponent<SCORE>();
                }

                // Make sure the singleton instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    private void Awake()
    {
        // Enforce the singleton pattern by destroying duplicate instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        ScoreVisual = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        ScoreVisual.text = $"Current Score Is : {Score} Out of ~";
    }

    public TextMeshProUGUI ScoreVisual;

    public int Score;

    public void AddToScore(int add)
    {
        Score += add;
    }
}
