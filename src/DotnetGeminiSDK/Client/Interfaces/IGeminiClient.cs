using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using Content = DotnetGeminiSDK.Model.Request.Content;

namespace DotnetGeminiSDK.Client.Interfaces
{
    public interface IGeminiClient
    {
        Task<GeminiMessageResponse?> TextPrompt(
            string model,
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiMessageResponse?> TextPrompt(
            string model,
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            List<string> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task StreamTextPrompt(
            string model,
            string message,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSse = false
        );

        Task StreamTextPrompt(
            string model,
            List<Content> messages,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSse = false
        );

        Task<GeminiMessageResponse?> ImagePrompt(
            string model,
            string message,
            byte[] image,
            ImageMimeType imageMimeType
        );

        Task<GeminiMessageResponse?> ImagePrompt(
            string model,
            string message,
            string base64Image,
            ImageMimeType imageMimeType
        );

        Task<GeminiModelResponse?> GetModel(string model);

        Task<RootGeminiModelResponse?> GetModels();

        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            string message
        );

        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            List<string> message
        );

        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            List<Content> message
        );

        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            string message
        );

        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            List<string> message
        );

        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            List<Content> message
        );
    }
}