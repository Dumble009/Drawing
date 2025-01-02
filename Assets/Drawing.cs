using UnityEngine;

public class Drawing : MonoBehaviour
{
	[SerializeField] Material mat;
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
	}
}
