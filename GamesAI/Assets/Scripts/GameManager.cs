﻿using System;
using System.Collections.Generic;
using System.Linq;
using GamesAI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Follower followerPrefab;
    public Enemy enemyPrefab;

    public Text CurrentScoreText;
    public Text MaxScoreText;

    public Transform Ground;

    public float SpawnCooldown = 5f;
    public int MaxEnemySpawn = 5;

    public ObjectPool<Follower> Followers { get; private set; }
    public ObjectPool<Enemy> Enemies { get; private set; }
    public Character Player { get; private set; }

    public static GameManager Instance { get; private set; }

    public IEnumerable<GameObject> FollowerGameObjects => Followers.Select(follower => follower.gameObject);

    public IEnumerable<GameObject> EnemyGameObjects => Enemies.Select(enemy => enemy.gameObject);
    
    private int maxScore;
    private float lastSpawn;

    private Vector3 groundMin;
    private Vector3 groundMax;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Followers =
            new ObjectPool<Follower>(
                GameObject.FindGameObjectsWithTag("Follower")
                    .Select(follower => follower.GetComponent<Follower>())
                    .ToList(), followerPrefab);
        Enemies =
            new ObjectPool<Enemy>(
                GameObject.FindGameObjectsWithTag("Enemy").Select(enemy => enemy.GetComponent<Enemy>()).ToList(),
                enemyPrefab);
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
        
        maxScore = 0;

        groundMin = Ground.position - Ground.localScale.IgnoreY()/2*10;
        groundMax = Ground.position + Ground.localScale.IgnoreY()/2*10;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        UpdateScores();
        SpawnEnemies();
    }

    private void UpdateScores()
    {
        int score = Followers.Count();
        CurrentScoreText.text = score.ToString();
        maxScore = Math.Max(score, maxScore);
        MaxScoreText.text = maxScore.ToString();
    }

    private void SpawnEnemies()
    {
        float time = Time.time;
        if (time - lastSpawn < SpawnCooldown) return;

        lastSpawn = time;
        int spawn = Random.Range(0, MaxEnemySpawn);
        for (var i = 0; i < spawn; i++)
        {
            var point = new Vector3(Random.Range(groundMin.x, groundMax.x), groundMin.y + 0.8f, Random.Range(groundMin.z, groundMax.z));
            Enemy enemy = Enemies.Instantiate();
            enemy.transform.position = point;
        }
    }
}