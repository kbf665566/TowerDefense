using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform pathParent;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveIndex = 0;

    [SerializeField] private TextMeshProUGUI waveCountdownText;

    private void Update()
    {
        if (EnemiesAlive > 0)
            return;

        if (waveIndex == waves.Length)
        {
            //winEvent
        }

        if (countdown <=0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown,0f,Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}",countdown);
    }

    private IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position,spawnPoint.rotation);
    }

    [ContextMenu("SetSpawnPoint")]
    public void SetSpawnPoint()
    {
        spawnPoint = pathParent.Find("Start(Clone)");
    }
}
