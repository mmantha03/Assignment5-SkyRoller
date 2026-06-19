using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] platformPrefabs;
    public GameObject[] startingSectionObjects;
    public float sectionLength = 18f;
    public float spawnAheadDistance = 85f;
    public float removeBehindDistance = 35f;
    public int startingSections = 7;

    readonly List<GameObject> activeSections = new List<GameObject>();
    float nextSpawnZ;
    int lastPrefabIndex = -1;

    void Start()
    {
        if (startingSectionObjects != null && startingSectionObjects.Length > 0)
        {
            float farthestSection = 0f;

            for (int i = 0; i < startingSectionObjects.Length; i++)
            {
                GameObject section = startingSectionObjects[i];

                if (section != null)
                {
                    activeSections.Add(section);
                    farthestSection = Mathf.Max(farthestSection, section.transform.position.z);
                }
            }

            if (activeSections.Count > 0)
            {
                nextSpawnZ = farthestSection + GetSectionLength(activeSections[activeSections.Count - 1]);
                return;
            }
        }

        for (int i = 0; i < startingSections; i++)
        {
            SpawnNextSection(i == 0);
        }
    }

    void Update()
    {
        if (player == null || platformPrefabs == null || platformPrefabs.Length == 0)
        {
            return;
        }

        while (nextSpawnZ < player.position.z + spawnAheadDistance)
        {
            SpawnNextSection(false);
        }

        for (int i = activeSections.Count - 1; i >= 0; i--)
        {
            GameObject section = activeSections[i];

            if (section.transform.position.z < player.position.z - removeBehindDistance)
            {
                activeSections.RemoveAt(i);
                Destroy(section);
            }
        }
    }

    void SpawnNextSection(bool useFirstPrefab)
    {
        int index = useFirstPrefab ? 0 : PickRandomPrefabIndex();
        GameObject section = Instantiate(platformPrefabs[index], new Vector3(0f, 0f, nextSpawnZ), Quaternion.identity);
        activeSections.Add(section);
        lastPrefabIndex = index;

        nextSpawnZ += GetSectionLength(section);
    }

    int PickRandomPrefabIndex()
    {
        int index = Random.Range(0, platformPrefabs.Length);

        if (platformPrefabs.Length > 1)
        {
            int tries = 0;

            while (index == lastPrefabIndex && tries < 6)
            {
                index = Random.Range(0, platformPrefabs.Length);
                tries++;
            }
        }

        return index;
    }

    float GetSectionLength(GameObject section)
    {
        PlatformSection platformSection = section.GetComponent<PlatformSection>();
        return platformSection != null ? platformSection.length : sectionLength;
    }
}
