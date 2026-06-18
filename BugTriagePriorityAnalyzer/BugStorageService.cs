using System.Text.Json;
using BugTriageAnalyzer.Models;

namespace BugTriageAnalyzer.Services;

// Updated: 06/18/2026
// This service saves and loads bug reports from a JSON file.

public class BugStorageService
{
    private readonly string _filePath = "bugs.json";

    public void SaveBugs(List<BugReport> bugs)
    {
        string json = JsonSerializer.Serialize(bugs, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }

    public List<BugReport> LoadBugs()
    {
        if (!File.Exists(_filePath))
            return new List<BugReport>();

        string json = File.ReadAllText(_filePath);

        return JsonSerializer.Deserialize<List<BugReport>>(json)
               ?? new List<BugReport>();
    }
}