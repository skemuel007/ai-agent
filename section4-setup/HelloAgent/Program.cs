using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI.Chat;
using Shared;

// 1. Define the variables we extracted from Microsoft Foundry

EnvironmentHelper.LoadRootEnv();

var endpoint = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var model = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME");

AIAgent? agent = null;

bool isLegacySystem = false;

// 2. Create the Agent using MAF
if (!isLegacySystem)
{
    agent = new AzureOpenAIClient(
            new Uri(endpoint),
            new AzureCliCredential())
        .GetChatClient(model)
        .AsAIAgent();
}
else
{
    var apiKey = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_KEY");
    
    agent = new AzureOpenAIClient(
            new Uri(endpoint),
            new ApiKeyCredential(apiKey))
        .GetChatClient(model)
        .AsAIAgent();
}

Console.WriteLine(await agent.RunAsync("What is the largest city in Nigeria?"));
