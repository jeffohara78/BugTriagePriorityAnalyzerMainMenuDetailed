namespace BugTriageAnalyzer.Models;

// Updated: 06/18/2026
// This class stores all information for one reported bug.
// The properties use plain names so the data is easy to understand.

public class BugReport
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ReportedBy { get; set; } = "";

    public int UsersAffected { get; set; }

    public bool BlocksUser { get; set; }
    public bool SecurityRisk { get; set; }
    public bool HasWorkaround { get; set; }

    public string Priority { get; set; } = "";
    public string Assessment { get; set; } = "";
    public string Status { get; set; } = "Open";

    public DateTime DateReported { get; set; } = DateTime.Now;

    public List<string> TriageSteps { get; set; } = new();
}