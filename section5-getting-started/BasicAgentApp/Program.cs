
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Shared;

EnvironmentHelper.LoadRootEnv();

var endpoint = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var deploymentName = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME");

IChatClient chatClient = new AzureOpenAIClient(
            new Uri(endpoint),
            new AzureCliCredential())
        .GetChatClient(deploymentName)
        .AsIChatClient();

// 3. Define the Agent's Anatomy
AIAgent supportAgent = chatClient.AsAIAgent(
    name: "NetworkSupport",
    instructions: "You are a Tier 1 IT Support Agent. Your answers should be concise, professional, and limited strictly to the topic at hand, do not provide answer outside your role.");

string userIssues = "I am getting a DNS resolution error when connection to the corporate VPN from a coffee shop.";

var response = await supportAgent.RunAsync(userIssues);

Console.WriteLine($"Agent '{supportAgent.Name}' is online\n");

Console.WriteLine($"{supportAgent.Name}: {response}");
