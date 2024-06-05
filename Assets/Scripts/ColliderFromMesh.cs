using UnityEngine;

public class ColliderFromMesh : MonoBehaviour
{
    private MeshCollider meshCollider;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh colliderMesh;

    void Start()
    {
        // Pobierz SkinnedMeshRenderer z obiektu
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer not found on the object.");
            return;
        }

        // Stwórz now¹ siatkê, która bêdzie przechowywaæ aktualn¹ pozycjê skórki
        colliderMesh = new Mesh();

        // Dodaj MeshCollider do obiektu
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {
        // Wypiecz bie¿¹c¹ siatkê do colliderMesh
        skinnedMeshRenderer.BakeMesh(colliderMesh);

        // Ustaw wypieczon¹ siatkê jako sharedMesh w MeshCollider
        meshCollider.sharedMesh = colliderMesh;

        // Upewnij siê, ¿e collider ma odpowiedni¹ pozycjê i rotacjê
        meshCollider.transform.position = skinnedMeshRenderer.transform.position;
        meshCollider.transform.rotation = skinnedMeshRenderer.transform.rotation;
    }
}