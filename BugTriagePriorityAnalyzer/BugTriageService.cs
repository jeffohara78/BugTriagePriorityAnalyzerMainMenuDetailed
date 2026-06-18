namespace BugTriageAnalyzer.Services;

// Updated: 06/18/2026
// This service handles the priority decision and plain-English triage guidance.

public class BugTriageService
{
    public string AnalyzePriority(
        int usersAffected,
        bool blocksUser,
        bool securityRisk,
        bool hasWorkaround)
    {
        if (securityRisk)
            return "Critical";

        if (blocksUser && usersAffected >= 50)
            return "Critical";

        if (blocksUser)
            return "High";

        if (usersAffected >= 100 && !hasWorkaround)
            return "High";

        if (usersAffected >= 25)
            return "Medium";

        return "Low";
    }

    public string GenerateAssessment(
        string priority,
        int usersAffected,
        bool blocksUser,
        bool securityRisk,
        bool hasWorkaround)
    {
        // Updated: 06/18/2026
        // Assessment wording was made more user-friendly and less technical.

        if (priority == "Critical")
        {
            return "This bug needs immediate attention. It may stop users from completing important tasks, affect many people, or create a security concern.";
        }

        if (priority == "High")
        {
            return "This bug should be worked on soon. It may not be an emergency, but it has a noticeable impact on the user experience.";
        }

        if (priority == "Medium")
        {
            return "This bug should be reviewed and planned for an upcoming work cycle. It affects users, but it does not appear to be urgent right now.";
        }

        return "This bug appears to be lower impact for now. It should still be documented and reviewed if more users report it.";
    }

    public string GetPriorityExplanation(string priority)
    {
        // Updated: 06/18/2026
        // Added plain-English descriptions so non-technical users understand the result.

        if (priority == "Critical")
            return "Critical means the bug may seriously affect users, security, or core application features.";

        if (priority == "High")
            return "High means the bug is important and should be handled soon.";

        if (priority == "Medium")
            return "Medium means the bug should be planned and monitored, but it is not currently an emergency.";

        return "Low means the bug is documented, but it can usually wait unless its impact grows.";
    }

    public List<string> GenerateTriageSteps(
        string priority,
        bool blocksUser,
        bool securityRisk,
        bool hasWorkaround)
    {
        List<string> steps = new();

        if (priority == "Critical")
        {
            steps.Add("Review this bug as soon as possible.");
            steps.Add("Assign it to an experienced developer or support team member.");
            steps.Add("Notify the project lead so they understand the risk.");
        }
        else if (priority == "High")
        {
            steps.Add("Schedule this bug for the current or next work cycle.");
            steps.Add("Confirm the issue can be repeated.");
            steps.Add("Decide whether users need a temporary workaround.");
        }
        else if (priority == "Medium")
        {
            steps.Add("Add this bug to the active backlog.");
            steps.Add("Review it during planning.");
            steps.Add("Watch for additional reports from users.");
        }
        else
        {
            steps.Add("Document the bug clearly.");
            steps.Add("Keep it available for future review.");
            steps.Add("Increase the priority later if more users are affected.");
        }

        if (securityRisk)
            steps.Add("Because security may be involved, escalate this to the security team.");

        if (blocksUser)
            steps.Add("Because the user is blocked, test the full user workflow before closing the bug.");

        if (!hasWorkaround)
            steps.Add("Because there is no workaround, consider giving users temporary guidance.");

        return steps;
    }
}