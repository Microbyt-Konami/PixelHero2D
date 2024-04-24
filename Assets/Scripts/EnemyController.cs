using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // Fields
    [SerializeField] private GameObject enemyExplode;

    // Components
    private Transform transformEnemy;

    public void Explode()
    {
        Instantiate(enemyExplode, transformEnemy.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Awake()
    {
        transformEnemy = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindAnyObjectByType<SaveDataGame>().ResetData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
