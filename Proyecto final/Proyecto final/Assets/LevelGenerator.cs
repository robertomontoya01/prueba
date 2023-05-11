using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 20f;
    [SerializeField] private Transform LevelPartStart;
    public float ItemSpawnProbability;
    [SerializeField] private List<Transform> Items;
    public List<float> ItemProbabilities;
    [SerializeField] private List<Transform> RoofItems;
    public List<float> RoofItemProbabilities;
    [SerializeField] private List<Transform> AirItems;
    public List<float> AirItemProbabilities;
    [SerializeField] private List<Transform> LevelParts;
    [SerializeField] private Transform RoofPart;
    [SerializeField] private Player player;
    private Vector3 lastEndPosition;

    private void Awake() {
        lastEndPosition = LevelPartStart.Find("EndPosition").position;
    }

    void Update() {
        if (Vector3.Distance(player.GetPosition(), lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART) {
            SpawnLevelPart();
        }
    }

    private void SpawnLevelPart() {
        Transform chosenLevelPart = LevelParts[Random.Range(0, LevelParts.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition + new Vector3(Random.Range(0, 3), Random.Range(-1, +2)));
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;

        if (Random.Range(0, 100) / 100f > ItemSpawnProbability) {
            return;
        }

        int whereToSpawn = Random.Range(0, 2);
        float deltaX = (lastEndPosition.x - lastLevelPartTransform.position.x) / 2.0f;

        switch (whereToSpawn) {
            case 0: {
                // Ground
                SpawnItem(Items, ItemProbabilities, lastLevelPartTransform.position + new Vector3(deltaX, 1.0f)); 
                break;
            }
            case 1: {
                // Roof
                SpawnItem(RoofItems, RoofItemProbabilities, lastLevelPartTransform.position + new Vector3(deltaX, 7.0f));
                break;
            }
        }

        float airSpawnProbability = 0.8f;

        if (Random.Range(0, 100) / 100f <= airSpawnProbability) {
            // Air
            SpawnItem(AirItems, AirItemProbabilities, lastLevelPartTransform.position + new Vector3(deltaX, 3.5f));
        }
    }

    void SpawnItem(List<Transform> items, List<float> itemProbabilities, Vector3 position) {
        // Make sure that sum(itemProbabilites) == 1.0
        float randomValue = (float)Random.Range(0, 100f) / 100f;
        int index = 0;
        float accumulatedProbability = 0f;

        for (; index < itemProbabilities.Count; index++) {
            accumulatedProbability += itemProbabilities[index];

            if (accumulatedProbability >= randomValue) {
                break;
            }
        }

        if (index < itemProbabilities.Count) {
            Transform chosenItem = items[index];
            SpawnLevelPart(chosenItem, position);
        }
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) {
        Instantiate(RoofPart, spawnPosition + new Vector3(0f, 12.5f), Quaternion.identity);
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        float paddingX = levelPartTransform.position.x - lastEndPosition.x;
        Instantiate(RoofPart, spawnPosition + new Vector3(-paddingX, 12.5f), Quaternion.identity);
        return levelPartTransform;
    }
}
