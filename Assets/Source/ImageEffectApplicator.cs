using UnityEngine;

[ExecuteAlways]
[ImageEffectAllowedInSceneView]
public class ImageEffectApplicator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Material _material;

    #endregion

    #region Methods

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _material);
    }

    #endregion
}