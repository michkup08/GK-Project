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

        // Stw�rz now� siatk�, kt�ra b�dzie przechowywa� aktualn� pozycj� sk�rki
        colliderMesh = new Mesh();

        // Dodaj MeshCollider do obiektu
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {
        // Wypiecz bie��c� siatk� do colliderMesh
        skinnedMeshRenderer.BakeMesh(colliderMesh);

        // Ustaw wypieczon� siatk� jako sharedMesh w MeshCollider
        meshCollider.sharedMesh = colliderMesh;

        // Upewnij si�, �e collider ma odpowiedni� pozycj� i rotacj�
        meshCollider.transform.position = skinnedMeshRenderer.transform.position;
        meshCollider.transform.rotation = skinnedMeshRenderer.transform.rotation;
    }
}