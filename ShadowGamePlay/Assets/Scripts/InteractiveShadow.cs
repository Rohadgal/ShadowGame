using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractiveShadow : MonoBehaviour
{
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private Transform lightTransform;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Vector3 extrucionDirecction = Vector3.zero;
    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f;

    private Vector3[] objectVertices;
    private LightType lightType;
    private Mesh shadowColliderMesh;
    private MeshCollider shadowsColider;

    private Vector3 previosPosition;
    private Quaternion previosRotation; 
    private Vector3 previosScale;

    private bool canUpdateCollider = true;

    private void Awake() {
        InitialiShadowCollider();
        lightType = lightTransform.GetComponent<Light>().type;
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
        shadowColliderMesh = new Mesh();
    }

    
    void Update(){
        shadowTransform.position = transform.position;
    }

    private void FixedUpdate() {
        if (TransformHasChanged() && canUpdateCollider) {
            Invoke("UpdateShadowCollider",shadowColliderUpdateTime);
            canUpdateCollider = false;
        }

        previosPosition = transform.position;
        previosRotation = transform.rotation;
        previosScale = transform.localScale;
    }

    private void InitialiShadowCollider() {
        GameObject shadowGameObject = shadowTransform.gameObject;
        //shadowGameObject.hideFlags = HideFlags.HideInHierarchy;
        shadowsColider = shadowGameObject.AddComponent<MeshCollider>();
        shadowsColider.convex = true;
        shadowsColider.isTrigger = true;
    }

    private void UpdateShadowCollider() {
        shadowColliderMesh.vertices = ComputerShadowColliderMeshVertices();
        shadowsColider.sharedMesh = shadowColliderMesh;
        canUpdateCollider = true;
    }

    private Vector3[] ComputerShadowColliderMeshVertices() {
        Vector3[] points = new Vector3[2 * objectVertices.Length];
        Vector3 raycastDirection = lightTransform.forward;
        int n = objectVertices.Length;

        for(int i = 0; i < n; i++) {
            Vector3 point = transform.TransformPoint(objectVertices[i]);
                if(lightType != LightType.Directional) {
                    raycastDirection = point - lightTransform.position;
                }
            points[i] = ComputerIntersectionPoint(point, raycastDirection);
            points[n + 1] = ComputerExtrusionPoint(point, points[i]);
        }

        return points;
    }

    private Vector3 ComputerIntersectionPoint(Vector3 fromPosition,Vector3 direction) {
        RaycastHit hit;

        if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, targetLayerMask)) {
            return hit.point - transform.position;
        }
        return fromPosition + 100 * direction - transform.position;
    }

    private Vector3 ComputerExtrusionPoint(Vector3 objetVertexPosition, Vector3 shadowPointPosition) {
        if(extrucionDirecction.sqrMagnitude == 0) {
            return objetVertexPosition - transform.position;
        }

        return shadowPointPosition + extrucionDirecction;
    }

    private bool TransformHasChanged() {
        return previosPosition != transform.position || previosRotation != transform.rotation || previosScale != transform.localScale;
    }
}
