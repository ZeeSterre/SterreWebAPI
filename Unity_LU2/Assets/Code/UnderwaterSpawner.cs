using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnderwaterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;

    private string environmentID;

    void Start()
    {
        environmentID = PlayerPrefs.GetString("environmentID");

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        SterreWebAPI.Instance.Get($"/Userinfo/{environmentID}/allObjects", EnvironmentReceived);
    }

    public void EnvironmentReceived(APIResponse response)
    {
        if (!response.Success)
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
            return;
        }

        string wrappedJson = $"{{\"objects\": {response.Data} }}";
        ObjectListWrapper objectList = JsonUtility.FromJson<ObjectListWrapper>(wrappedJson);

        int objectCount = objectList.objects.Length;

        StartCoroutine(SpawnObjectsCoroutine(objectList.objects, objectCount));
    }

    private IEnumerator SpawnObjectsCoroutine(ObjectData[] objects, int objectCount)
    {
        for (int i = 0; i < objectCount; i++)
        {
            SpawnObject(objects[i]);

            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void SpawnObject(ObjectData objData)
    {
        if (string.IsNullOrEmpty(objData.prefabId))
        {
            return;
        }

        string prefabPath = "Prefabs/Underwater/" + objData.prefabId.Trim().Replace(" ", "_");
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab != null)
        {
            GameObject spawnedObject = Instantiate(prefab, new Vector3(objData.positionX, objData.positionY, 0), Quaternion.Euler(0, 0, objData.rotationZ));
            spawnedObject.transform.localScale = new Vector3(
                objData.scaleX != 0 ? objData.scaleX : 1,
                objData.scaleY != 0 ? objData.scaleY : 1,
                1
            );
            spawnedObject.transform.localScale = new Vector3(7f, 7f, 1f);

            Renderer renderer = spawnedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = -1;
            }
        }
    }
}
