using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform pathParent;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveIndex = 0;

    [SerializeField] private TextMeshProUGUI waveCountdownText;

    private void Update()
    {
        if(countdown <=0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown,0f,Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}",countdown);
    }

    private IEnumerator SpawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);
    }

    [ContextMenu("SetSpawnPoint")]
    public void SetSpawnPoint()
    {
        spawnPoint = pathParent.Find("Start(Clone)");
    }
}
