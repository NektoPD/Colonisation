using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Terrain))]
public class RandomResourseGenerator : MonoBehaviour
{
    [SerializeField] private Resource _template;

    private UnityEngine.Terrain _terrain;
    private float _cooldownTime = 1f;

    private void Start()
    {
        _terrain = GetComponent<UnityEngine.Terrain>();
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        float defaultYpoition = 1f;
        bool isWorking = true;
        float minX = _terrain.transform.position.x;
        float maxX = minX + _terrain.terrainData.size.x;
        float minZ = _terrain.transform.position.z;
        float maxZ = minZ + _terrain.terrainData.size.z;
        WaitForSeconds cooldown = new WaitForSeconds(_cooldownTime);

        while (isWorking)
        {
            Instantiate(_template, new Vector3(Random.Range(minX, maxX), defaultYpoition, Random.Range(minZ, maxZ)), Quaternion.identity);

            yield return cooldown;
        }
    }
}
