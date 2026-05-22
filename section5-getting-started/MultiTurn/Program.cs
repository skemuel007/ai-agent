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

AIAgent travelAgent = chatClient.AsAIAgent(
    name: "TravelPlanner",
    instructions: "You are a practical travel planning assistant. Keep answers concise, remember details from the conversation, and ask clarifying questions when needed.");

/*ChatMessage[] conversation =
[
    new(ChatRole.User, "I want to plan a 3-day trip to Lisbon in June."),
    new(ChatRole.Assistant, "Great. What kind of trip do you want: food, history, beaches, nightlife, or a mix?"),
    new(ChatRole.User, "A mix of food and history. Keep the budget moderate."),
    new(ChatRole.User, "Can you suggest a simple itinerary and remember that I prefer walking when possible?")
];*/

AgentSession session = await travelAgent.CreateSessionAsync();

while (true)
{
    Console.Write("User: ");
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input)) break;
    
    AgentResponse response = await travelAgent.RunAsync(input, session);
    
    Console.WriteLine($"Agent '{travelAgent.Name}' response:\n");
    Console.WriteLine(response.Text); 
}
