using UnityEngine;


[ExecuteInEditMode]
public class CRTPostProcess : MonoBehaviour
{
    [SerializeField] private Material crtMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (crtMaterial != null)
            Graphics.Blit(src, dest, crtMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
