using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private Material colorMaterial;
    private bool isHovered = false;
    private bool isGrabbed = false;

    void Start() {
        //outlineMaterial = new Material(outlineShader);
        //outlineMaterial.SetColor("_OutlineColor", outlineColor);
        //outlineMaterial.SetFloat("_Outline", outlineWidth);
        ApplyOutline(false);
    }

    void OnMouseEnter() {
        if (!isGrabbed) {
            isHovered = true;
            ApplyOutline(true);
        }
    }

    void OnMouseExit() {
        if (!isGrabbed) {
            isHovered = false;
            ApplyOutline(false);
        }
    }

    public void setIsGrabbed(bool t_isGrabbed) {
        isGrabbed = t_isGrabbed;
    
        ApplyOutline(t_isGrabbed);
    }

    void ApplyOutline(bool shouldOutline) {
        if (shouldOutline) {
            GetComponent<Renderer>().material = outlineMaterial;
            return;
        }
            // Replace with your original material
            GetComponent<Renderer>().material = colorMaterial;
    }
}
