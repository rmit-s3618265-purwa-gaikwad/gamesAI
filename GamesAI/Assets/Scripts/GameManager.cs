using System.Collections.Generic;
using System.Linq;
using GamesAI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Follower followerPrefab;
    public Enemy enemyPrefab;

    public ObjectPool<Follower> Followers { get; private set; }
    public ObjectPool<Enemy> Enemies { get; private set; }
    public Character Player { get; private set; }

    public static GameManager Instance { get; private set; }

    public IEnumerable<GameObject> FollowerGameObjects => Followers.Select(follower => follower.gameObject);

    public IEnumerable<GameObject> EnemyGameObjects => Enemies.Select(enemy => enemy.gameObject);

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
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}