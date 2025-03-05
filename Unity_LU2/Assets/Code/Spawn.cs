using UnityEngine;

public class DraggablePrefabSaver : MonoBehaviour
{
    public GameObject[] prefabOptions;
    public void SpawnPrefab(int index)
    {
        if (index < 0 || index >= prefabOptions.Length) return;

        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        spawnPosition.z = 0f;

        GameObject newObject = Instantiate(prefabOptions[index], spawnPosition, Quaternion.identity);
        newObject.SetActive(true);

        if (!newObject.GetComponent<DragObjects>()) newObject.AddComponent<DragObjects>();
        if (!newObject.GetComponent<Collider2D>()) newObject.AddComponent<BoxCollider2D>();

        newObject.transform.localScale = new Vector3(7f, 7f, 1f);
        SetRenderingOrder(newObject);
    }

    private void SetRenderingOrder(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.sortingOrder = 1;

        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer) meshRenderer.sortingOrder = 1;
    }
}
