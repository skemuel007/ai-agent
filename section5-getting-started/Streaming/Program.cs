using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
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


bool isStreaming = true;

AgentResponse? agentResponse = null;
string userIssues = "I am getting a DNS resolution error when connection to the corporate VPN from a coffee shop.";


if (isStreaming)
{
    // streaming response
    await foreach (AgentResponseUpdate update in supportAgent.RunStreamingAsync(userIssues))
    {
        // print each update
        Console.WriteLine(update.Text);
    }
}
else
{
    agentResponse = await supportAgent.RunAsync(userIssues);
    var response = agentResponse ?? throw new Exception("Agent response is null");
    Console.WriteLine(response.Text);
}



