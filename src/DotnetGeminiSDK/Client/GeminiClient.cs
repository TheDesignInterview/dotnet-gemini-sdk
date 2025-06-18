using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester.Interfaces;
using Content = DotnetGeminiSDK.Model.Request.Content;
using Part = DotnetGeminiSDK.Model.Request.Part;

namespace DotnetGeminiSDK.Client
{
    public class GeminiClient : IGeminiClient
    {
        private readonly IApiRequester _apiRequester;
        private readonly GoogleGeminiConfig _config;
        private const string MessagesCannotBeEmpty = "Messages cannot be empty.";

        public GeminiClient(GoogleGeminiConfig config, IApiRequester apiRequester)
        {
            ArgumentNullException.ThrowIfNull(config);
            ArgumentNullException.ThrowIfNull(apiRequester);

            _config = config;
            _apiRequester = apiRequester;
        }

        public async Task<GeminiMessageResponse?> TextPrompt(
            string model,
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            var promptUrl = $"{_config.BaseUrl}/{model}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);
            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiMessageResponse?> TextPrompt(
            string model,
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(messages);

            if (messages.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(messages));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);
            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            var promptUrl = $"{_config.BaseUrl}/{model}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);
            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            List<string> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(messages);

            var promptUrl = $"{_config.BaseUrl}/{model}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);
            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiCountTokenMessageResponse?> CountTokens(
            string model,
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(messages);
            if (messages.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(messages));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);
            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        public Task StreamTextPrompt(
            string model,
            string message,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSse = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            ArgumentNullException.ThrowIfNull(callback);

            var promptUrl =
                $"{_config.BaseUrl}/{model}:streamGenerateContent?key={_config.ApiKey}{(useSse ? "&alt=sse" : string.Empty)}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);
            return _apiRequester.PostStream(promptUrl, request, callback);
        }

        public Task StreamTextPrompt(
            string model,
            List<Content> messages,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSse = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(messages);
            ArgumentNullException.ThrowIfNull(callback);

            if (messages.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(messages));
            }

            var promptUrl =
                $"{_config.BaseUrl}/{model}:streamGenerateContent?key={_config.ApiKey}{(useSse ? "&alt=sse" : string.Empty)}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);
            return _apiRequester.PostStream(promptUrl, request, callback);
        }

        public async Task<GeminiMessageResponse?> ImagePrompt(
            string model,
            string message,
            byte[] image,
            ImageMimeType imageMimeType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            ArgumentNullException.ThrowIfNull(image);

            if (image.Length == 0)
            {
                throw new ArgumentException("Image cannot be empty.", nameof(image));
            }

            var mimeTypeString = GetMimeTypeString(imageMimeType);
            var promptUrl = $"{_config.BaseUrl}/{model}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, Convert.ToBase64String(image), mimeTypeString);
            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiMessageResponse?> ImagePrompt(
            string model,
            string message,
            string base64Image,
            ImageMimeType imageMimeType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            ArgumentException.ThrowIfNullOrWhiteSpace(base64Image);
            
            var mimeTypeString = GetMimeTypeString(imageMimeType);
            var promptUrl = $"{_config.BaseUrl}/{model}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, base64Image, mimeTypeString);
            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        public async Task<GeminiModelResponse?> GetModel(string model)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);

            var modelUrl = $"{_config.BaseUrl}/{model}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<GeminiModelResponse>(modelUrl);
        }

        public async Task<RootGeminiModelResponse?> GetModels()
        {
            var modelUrl = $"{_config.BaseUrl}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<RootGeminiModelResponse>(modelUrl);
        }

        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            var promptUrl = $"{_config.BaseUrl}/{model}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }

        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            List<string> message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(message);

            if (message.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(message));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }

        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(
            string model,
            List<Content> message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(message);

            if (message.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(message));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:embedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }

        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            var promptUrl = $"{_config.BaseUrl}/{model}:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            List<string> message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(message);

            if (message.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(message));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(
            string model,
            List<Content> message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentNullException.ThrowIfNull(message);

            if (message.Count == 0)
            {
                throw new ArgumentException(MessagesCannotBeEmpty, nameof(message));
            }

            var promptUrl = $"{_config.BaseUrl}/{model}:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        // Private helper methods remain unchanged
        private static GeminiMessageRequest BuildImageGeminiRequest(string message, string base64Image, string mimeType,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null) => new GeminiMessageRequest
        {
            Contents = new List<Content>
            {
                new Content
                {
                    Parts = new List<Part>
                    {
                        new Part { Text = message },
                        new Part { InlineData = new InlineData { MimeType = mimeType, Data = base64Image } }
                    }
                }
            },
            GenerationConfig = generationConfig, SafetySetting = safetySetting
        };

        private static GeminiMessageRequest BuildGeminiRequest(List<Content> messages,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null) => new GeminiMessageRequest
            { Contents = messages, GenerationConfig = generationConfig, SafetySetting = safetySetting };

        private static GeminiMessageRequest BuildGeminiRequest(string message,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null) => new GeminiMessageRequest
        {
            Contents = new List<Content> { new Content { Parts = new List<Part> { new Part { Text = message } } } },
            GenerationConfig = generationConfig, SafetySetting = safetySetting
        };

        private static GeminiMessageRequest BuildGeminiRequest(IEnumerable<string> messages,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null)
        {
            var content = new Content { Parts = messages.Select(message => new Part { Text = message }).ToList() };
            return new GeminiMessageRequest
            {
                Contents = new List<Content> { content }, GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }

        private static GeminiEmbeddedMessageRequest BuildEmbeddedGeminiRequest(string message, string model) =>
            new GeminiEmbeddedMessageRequest
                { Model = model, Content = new Content { Parts = new List<Part> { new Part { Text = message } } } };

        private static GeminiEmbeddedMessageRequest
            BuildEmbeddedGeminiRequest(IEnumerable<string> messages, string model) => new GeminiEmbeddedMessageRequest
        {
            Model = model,
            Content = new Content { Parts = messages.Select(message => new Part { Text = message }).ToList() }
        };

        private static string GetMimeTypeString(ImageMimeType mimeType) => mimeType switch
        {
            ImageMimeType.Jpeg => "image/jpeg", ImageMimeType.Png => "image/png", ImageMimeType.Jpg => "image/jpg",
            ImageMimeType.Heic => "image/heic", ImageMimeType.Heif => "image/heif",
            ImageMimeType.Webp => "image/webp",
            _ => throw new ArgumentOutOfRangeException(nameof(mimeType), mimeType, "Invalid image mime type.")
        };
    }
}