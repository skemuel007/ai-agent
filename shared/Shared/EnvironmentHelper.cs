using DotNetEnv;

namespace Shared;

public static class EnvironmentHelper
{
    public static void LoadRootEnv()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            var envPath = Path.Combine(current.FullName, ".env");

            if (File.Exists(envPath))
            {
                Env.Load(envPath);
                return;
            }

            current = current.Parent;
        }
    }

    public static string GetRequiredEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception($"{name} is not set");
        }

        return value;
    }
}
