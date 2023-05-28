using UnityEngine;
using UnityEngine.UI;

namespace Clay.ImageProcessor
{
    public class ImageProcessor : MonoBehaviour
    {
        [SerializeField] private ComputeShader computeShader;
        [SerializeField] private RawImage rawImage;

        private void Start()
        {
            // RawImageのテクスチャをComputeShaderで加工する
            var inputTexture = rawImage.texture as Texture2D;
            if (inputTexture == null) { return; }
            var pixels = inputTexture.GetPixels32();
            
            // 必要な出力RenderTextureを用意する
            var outputRT = new RenderTexture(inputTexture.width, inputTexture.height, 0);
            outputRT.enableRandomWrite = true;
            outputRT.Create();

            // ComputeBufferの代わりにGraphicsBufferを使ってテクスチャ情報をGPUに渡す
            var buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, pixels.Length, 4);
            buffer.SetData(pixels);
            
            // NegativeConverter.computeに渡す引数を設定する
            var kernel = computeShader.FindKernel("NegativeConverter");
            computeShader.SetBuffer(kernel, "InputTexture", buffer);
        }
    }
}