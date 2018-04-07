using System;
using System.Collections.Generic;
using System.Linq;
using GamesAI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PathFinding))]
public class GameManager : MonoBehaviour
{
    public Follower followerPrefab;
    public Enemy enemyPrefab;
    public GameObject[] obstaclePrefabs;

    public Text CurrentScoreText;
    public Text MaxScoreText;

    public Transform Ground;

    public float SpawnCooldown = 5f;
    public int MaxEnemySpawn = 5;
    public int MinObstacles = 5;
    public int MaxObstacles = 15;
    public float MinObstacleDistance = 4f;
    public int obstacleTries = 100;
    public GridPlane GridPlane;

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

    [HideInInspector]
    public PathFinding PathFinding;

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

        PathFinding = GetComponent<PathFinding>();

        maxScore = 0;

        groundMin = Ground.position - Ground.localScale.IgnoreY()/2*10;
        groundMax = Ground.position + Ground.localScale.IgnoreY()/2*10;

        int numObstacles = Random.Range(MinObstacles, MaxObstacles + 1);
        for (var i = 0; i < numObstacles; i++)
        {
            Vector3 pos = Vector3.zero;
            var hasPos = false;
            for (var j = 0; j < obstacleTries; j++)
            {
                pos = Helper.RandomRange(groundMin, groundMax);
                if (!Physics.CheckSphere(pos, MinObstacleDistance, GridPlane.unwalkableMask))
                {
                    hasPos = true;
                    break;
                }
            }
            if(!hasPos) continue;
            
            GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            obstacle = Instantiate(obstacle);
            obstacle.transform.position = pos;
        }

        GridPlane.GenerateGrid();
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
        int spawn = Random.Range(0, MaxEnemySpawn + 1);
        for (var i = 0; i < spawn; i++)
        {
            Enemy enemy = Enemies.Instantiate();
            enemy.transform.position = Helper.RandomRange(groundMin, groundMax) + Vector3.up*0.8f;
        }
    }
}