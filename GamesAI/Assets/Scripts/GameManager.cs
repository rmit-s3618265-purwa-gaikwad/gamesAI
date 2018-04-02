using System.Collections.Generic;
using System.Linq;
using GamesAI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Enemy> enemies;
    private List<Follower> followers;
    public CharacterControl Player { get; private set; }
    public GameObject PlayerGameObject => Player.gameObject;

    public static GameManager Instance { get; private set; }

    public IEnumerable<Follower> Followers => followers;

    public IEnumerable<GameObject> FollowerGameObjects => followers.Select(follower => follower.gameObject);

    public IEnumerable<Enemy> Enemies => enemies;

    public IEnumerable<GameObject> EnemyGameObjects => enemies.Select(enemy => enemy.gameObject);

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
        followers =
            GameObject.FindGameObjectsWithTag("Follower").Select(follower => follower.GetComponent<Follower>()).ToList();
        enemies =
            GameObject.FindGameObjectsWithTag("Enemy").Select(enemy => enemy.GetComponent<Enemy>()).ToList();
        Player = GameObject.FindWithTag("Player").GetComponent<CharacterControl>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}