# .NET Gemini SDK

![NuGet Version](https://img.shields.io/nuget/v/TheDesignInterview.DotnetGeminiSDK) ![NuGet Downloads](https://img.shields.io/nuget/dt/TheDesignInterview.DotnetGeminiSDK)

A lightweight, dependency-free .NET SDK for the Google Gemini REST API. This client supports text prompts, image processing, token counting, embeddings, and structured JSON output via response schemas.

> **Note:** This project was initially forked from [gsilvamartin/dotnet-gemini-sdk](https://github.com/gsilvamartin/dotnet-gemini-sdk)

## Table of Contents
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
    - [Text Prompt](#text-prompt)
    - [Stream Text Prompt](#stream-text-prompt)
    - [Multiple Text Prompt](#multiple-text-prompt)
    - [Get Model](#get-model)
    - [Get All Models](#get-models)
    - [Image Prompt](#image-prompt)
    - [Embedded](#embedded)
    - [Batch Embedded](#batch-embedded)
    - [Structured Output](#structured-output)
    - [Exception Handling](#exception-handling)
- [Contributing](#contributing)
- [License](#license)

# What is Google Gemini?
Google Gemini is an advanced AI platform that offers various interfaces for commands tailored to different use cases. It allows users to interact with machine learning models for generating content and responses to instructions. The platform supports free-form commands, structured commands, and chat-based requests. Additionally, Gemini provides the ability to adjust models for specific tasks, enhancing their performance for particular use cases.

## Installation 📦
Get started by installing the .NET Gemini SDK NuGet package. Run the following command in the NuGet Package Manager Console:

```sh
Install-Package TheDesignInterview.DotnetGeminiSDK
```

Or, if you prefer using the .NET CLI:

```sh
dotnet add package TheDesignInterview.DotnetGeminiSDK
```

## Configuration ⚙️
To use the Gemini SDK, configure the `GoogleGeminiConfig` object. Add the Gemini client to your service collection using `GeminiServiceExtensions`:

> [!NOTE]
> Only used when using the dependency injection method.

```csharp
using DotnetGeminiSDK;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGeminiClient(config =>
        {
            config.ApiKey = "YOUR_GOOGLE_GEMINI_API_KEY";
            config.ImageBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
            config.TextBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
        });
    }
}
```

## How to use? 🔎
### Dependency Injection

When you incorporate the Gemini client, you can seamlessly inject it into your code for immediate use.

```csharp
using DotnetGeminiSDK.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class YourClass
{
    private readonly IGeminiClient _geminiClient;

    public YourClass(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
    }

    public async Task Example()
    {
        var response = await _geminiClient.TextPrompt("Text for processing");
        // Process the response as needed
    }
}
```

### Class Instantiation

If you don't want to use dependency injection, you can instantiate the GeminiClient class, as a constructor parameter, place your settings in the GoogleGeminiConfig instance.

```csharp
using DotnetGeminiSDK.Client.Interfaces;

public class YourClass
{
    private readonly GeminiClient _geminiClient;

    public YourClass()
    {
        _geminiClient = new GeminiClient(new GoogleGeminiConfig(){ //Place your settings here });
    }

    public async Task Example()
    {
        var response = await _geminiClient.TextPrompt("Text for processing");
        // Process the response as needed
    }
}
```

## Implemented features 👾

- [x] Text Prompt
- [x] Stream Text Prompt
- [x] Multiple Text Prompt
- [x] Image Prompt
- [x] Counting Tokens
- [x] Get Model
- [x] List Models
- [x] Embedding
- [x] Batch Embedding
- [x] Structured Output (JSON Schema)

## Usage 🚀
### Text Prompt 📝
Prompt the Gemini API with a text message using the `TextPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.TextPrompt("Write a story about a magic backpack");
```

### Stream Text Prompt 🔁
Prompt the Gemini API with a text message using the `StreamTextPrompt` method:

> [!NOTE]
> This differs from the text prompt, it receives the response as string and in chunks.

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.StreamTextPrompt("Write a story about a magic backpack", (chunk) => {
  Console.WriteLine("Process your chunk of response here");
});
```

### Multiple Text Prompt 📚
Prompt the Gemini API with multiple text messages using the `TextPrompt` method with a list of `Content` objects:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

var messages = new List<Content>
{
    new Content
    {
        Parts = new List<Part>
        {
            new Part
            {
                Text = "Write a story about a magic backpack"
            }
        }
    },
    // Add more Content objects as needed
};

var response = await geminiClient.TextPrompt(messages);
```

### Get Model 📒
Get the specific model details of Gemini using `GetModel` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.GetModel("gemini-model-v1");
```

### List all models 🔖
Get all Gemini models using `GetModels` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.GetModels();
```

### Count Tokens 1️⃣
Prompt the Gemini API with a text message using the `CountTokens` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.CountTokens("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### Image Prompt 🖼️
#### Using file
Prompt the Gemini API with an image and a text message using the `ImagePrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var image = File.ReadAllBytes("path/to/your/image.jpg");
var response = await geminiClient.ImagePrompt("Describe this image", image, ImageMimeType.ImageJpeg);
```

#### Using Base64 String
Prompt the Gemini API with an base64 string and a text message using the `ImagePrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var base64Image = "image-as-base64";
var response = await geminiClient.ImagePrompt("Describe this image in details", base64Image, ImageMimeType.ImageJpeg);
```

### Embedded 🪡
Prompt the Gemini API with a text message and using embedded technique using the `EmbeddedContentsPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.EmbeddedContentsPrompt("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### Batch Embedded 🪡
Prompt the Gemini API with a text message and using batch embedded technique using the `BatchEmbeddedContentsPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.BatchEmbeddedContentsPrompt("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### Structured Output 📋
Generate structured JSON responses by defining a response schema. This ensures the AI returns data in your desired format:

```csharp
using DotnetGeminiSDK.Model.Request;

var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

// Define the structure you want for the response
var responseSchema = new ResponseSchema
{
    Type = "object",
    Properties = new Dictionary<string, ResponseSchema>
    {
        ["name"] = new ResponseSchema 
        { 
            Type = "string", 
            Description = "The person's full name" 
        },
        ["age"] = new ResponseSchema 
        { 
            Type = "integer", 
            Description = "The person's age in years" 
        },
        ["skills"] = new ResponseSchema 
        { 
            Type = "array", 
            Description = "List of skills",
            Items = new ResponseSchema { Type = "string" }
        }
    }
};

var generationConfig = new GenerationConfig
{
    ResponseMimeType = "application/json",
    ResponseSchema = responseSchema,
    Temperature = 0.3
};

var response = await geminiClient.TextPrompt(
    "Generate a profile for a software developer",
    generationConfig
);

// The response will be structured JSON matching your schema
var jsonResponse = response?.GetResponseText()?.FirstOrDefault();
```

#### Complex Schema Example
For more complex data structures:

```csharp
var productSchema = new ResponseSchema
{
    Type = "object",
    Properties = new Dictionary<string, ResponseSchema>
    {
        ["products"] = new ResponseSchema
        {
            Type = "array",
            Description = "List of products",
            Items = new ResponseSchema
            {
                Type = "object",
                Properties = new Dictionary<string, ResponseSchema>
                {
                    ["id"] = new ResponseSchema { Type = "string" },
                    ["name"] = new ResponseSchema { Type = "string" },
                    ["price"] = new ResponseSchema { Type = "number" },
                    ["category"] = new ResponseSchema { Type = "string" },
                    ["inStock"] = new ResponseSchema { Type = "boolean" }
                }
            }
        }
    }
};

var generationConfig = new GenerationConfig
{
    ResponseMimeType = "application/json",
    ResponseSchema = productSchema
};

var response = await geminiClient.TextPrompt(
    "Generate 3 sample e-commerce products",
    generationConfig
);
```

## Contributing 🤝
Contributions are welcome! Feel free to open issues or pull requests to enhance the SDK.

## License 📜
This project is licensed under the MIT License.
