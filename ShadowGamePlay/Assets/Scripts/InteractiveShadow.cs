using UnityEngine;
using System.Linq;

/// <summary>
/// A script that creates interactive shadows for a GameObject based on the position of a light source.
/// </summary>
public class InteractiveShadow : MonoBehaviour {
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private Transform lightTransform;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Vector3 extrusionDirection = Vector3.zero;
    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f;

    private Vector3[] objectVertices;
    private LightType lightType;
    private Mesh shadowColliderMesh;
    private MeshCollider shadowCollider;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 previousScale;
    private bool canUpdateCollider = true;

    /// <summary>
    /// Initializes the shadow collider and retrieves necessary initial information.
    /// </summary>
    private void Awake() {
        InitializeShadowCollider();
        lightType = lightTransform.GetComponent<Light>().type;
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
        shadowColliderMesh = new Mesh();
    }

    /// <summary>
    /// Updates the position of the shadow to match the position of the GameObject.
    /// </summary>
    private void Update() {
        shadowTransform.position = transform.position;
    }

    /// <summary>
    /// Checks if the GameObject's transform has changed, and schedules an update to the shadow collider if needed.
    /// </summary>
    private void FixedUpdate() {
        if (TransformHasChanged() && canUpdateCollider) {
            Invoke("UpdateShadowCollider", shadowColliderUpdateTime);
            canUpdateCollider = false;
        }
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        previousScale = transform.localScale;
    }

    /// <summary>
    /// Initializes the MeshCollider for the shadow.
    /// </summary>
    private void InitializeShadowCollider() {
        GameObject shadowGameObject = shadowTransform.gameObject;
        shadowCollider = shadowGameObject.AddComponent<MeshCollider>();
        shadowCollider.convex = true;
        shadowCollider.isTrigger = true;
    }

    /// <summary>
    /// Updates the shadow collider with new vertices based on the current light and object positions.
    /// </summary>
    private void UpdateShadowCollider() {
        shadowColliderMesh.vertices = ComputeShadowColliderMeshVertices();
        shadowCollider.sharedMesh = shadowColliderMesh;
        canUpdateCollider = true;
    }

    /// <summary>
    /// Computes the vertices for the shadow collider mesh based on the object's vertices, light direction, and extrusion direction.
    /// </summary>
    /// <returns>An array of vertices for the shadow collider mesh.</returns>
    private Vector3[] ComputeShadowColliderMeshVertices() {
        Vector3[] points = new Vector3[2 * objectVertices.Length];
        Vector3 raycastDirection = lightTransform.forward;
        int n = objectVertices.Length;
        for (int i = 0; i < n; i++) {
            Vector3 point = transform.TransformPoint(objectVertices[i]);
            if (lightType != LightType.Directional) {
                raycastDirection = point - lightTransform.position;
            }
            points[i] = ComputeIntersectionPoint(point, raycastDirection);
            points[n + i] = ComputeExtrusionPoint(point, points[i]);
        }
        return points;
    }

    /// <summary>
    /// Computes the intersection point of a ray from the object's vertex towards the light source.
    /// </summary>
    /// <param name="fromPosition">The starting position of the ray.</param>
    /// <param name="direction">The direction of the ray.</param>
    /// <returns>The intersection point.</returns>
    private Vector3 ComputeIntersectionPoint(Vector3 fromPosition, Vector3 direction) {
        RaycastHit hit;
        if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, targetLayerMask)) {
            return hit.point - transform.position;
        }
        return fromPosition + 100 * direction - transform.position;
    }

    /// <summary>
    /// Computes the extrusion point based on the object's vertex position and a given shadow point position.
    /// </summary>
    /// <param name="objectVertexPosition">The position of the object's vertex.</param>
    /// <param name="shadowPointPosition">The position of the corresponding shadow point.</param>
    /// <returns>The extrusion point.</returns>
    private Vector3 ComputeExtrusionPoint(Vector3 objectVertexPosition, Vector3 shadowPointPosition) {
        if (extrusionDirection.sqrMagnitude == 0) {
            return objectVertexPosition - transform.position;
        }
        return shadowPointPosition + extrusionDirection;
    }

    /// <summary>
    /// Checks if the transform of the GameObject has changed.
    /// </summary>
    /// <returns>True if the transform has changed, false otherwise.</returns>
    private bool TransformHasChanged() {
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale;
    }
}