using BugTriageAnalyzer.Models;
using BugTriageAnalyzer.Services;

namespace BugTriageAnalyzer;

// Updated: 06/18/2026
// AppManager now provides a more user-friendly experience with clearer screens,
// plain-English instructions, better spacing, and guided input.

public class AppManager
{
    private readonly BugTriageService _triageService;
    private readonly BugStorageService _storageService;

    private List<BugReport> _bugReports;
    private int _nextBugId;

    public AppManager()
    {
        _triageService = new BugTriageService();
        _storageService = new BugStorageService();

        _bugReports = _storageService.LoadBugs();

        _nextBugId = _bugReports.Count > 0
            ? _bugReports.Max(b => b.Id) + 1
            : 1001;
    }

    public void AddNewBug()
    {
        Console.Clear();

        ShowHeader("Add a New Bug");

        Console.WriteLine("This screen will ask a few simple questions.");
        Console.WriteLine("Your answers will help the app decide how urgent the bug is.");
        Console.WriteLine();
        Console.WriteLine("Tip: Enter 0 when asked for a number to return to the Main Menu.");
        Console.WriteLine();

        Console.Write("Short bug title: ");
        string title = Console.ReadLine() ?? "";

        Console.WriteLine();
        Console.WriteLine("Describe what happened in plain language.");
        Console.Write("Bug description: ");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine();
        Console.Write("Who reported this bug? ");
        string reportedBy = Console.ReadLine() ?? "";

        Console.WriteLine();
        Console.WriteLine("About how many users are affected?");
        Console.WriteLine("Example: 1, 10, 50, 100");
        Console.Write("Users affected: ");

        int usersAffected = ReadNumberOrReturn();

        if (usersAffected == 0)
            return;

        Console.WriteLine();
        Console.WriteLine("Does this bug stop the user from continuing?");
        Console.WriteLine("Example: The user cannot log in, submit a form, or complete checkout.");
        Console.Write("Enter yes or no: ");
        bool blocksUser = ReadYesNo();

        Console.WriteLine();
        Console.WriteLine("Could this bug create a security or privacy risk?");
        Console.WriteLine("Example: Exposed user data, login problems, or permission issues.");
        Console.Write("Enter yes or no: ");
        bool securityRisk = ReadYesNo();

        Console.WriteLine();
        Console.WriteLine("Is there a workaround?");
        Console.WriteLine("Example: The user can still complete the task another way.");
        Console.Write("Enter yes or no: ");
        bool hasWorkaround = ReadYesNo();

        string priority = _triageService.AnalyzePriority(
            usersAffected,
            blocksUser,
            securityRisk,
            hasWorkaround);

        string assessment = _triageService.GenerateAssessment(
            priority,
            usersAffected,
            blocksUser,
            securityRisk,
            hasWorkaround);

        List<string> triageSteps = _triageService.GenerateTriageSteps(
            priority,
            blocksUser,
            securityRisk,
            hasWorkaround);

        BugReport newBug = new BugReport
        {
            Id = _nextBugId++,
            Title = title,
            Description = description,
            ReportedBy = reportedBy,
            UsersAffected = usersAffected,
            BlocksUser = blocksUser,
            SecurityRisk = securityRisk,
            HasWorkaround = hasWorkaround,
            Priority = priority,
            Assessment = assessment,
            TriageSteps = triageSteps,
            DateReported = DateTime.Now
        };

        _bugReports.Add(newBug);
        _storageService.SaveBugs(_bugReports);

        Console.Clear();
        ShowHeader("Bug Added Successfully");

        Console.WriteLine($"Bug ID: {newBug.Id}");
        Console.WriteLine($"Priority: {newBug.Priority}");
        Console.WriteLine();
        Console.WriteLine(_triageService.GetPriorityExplanation(newBug.Priority));
        Console.WriteLine();
        Console.WriteLine("Assessment:");
        Console.WriteLine(newBug.Assessment);

        ReturnToMainMenu();
    }

    public void ViewAllBugs()
    {
        Console.Clear();
        ShowHeader("View All Bugs");

        if (_bugReports.Count == 0)
        {
            Console.WriteLine("No bugs have been entered yet.");
            Console.WriteLine();
            Console.WriteLine("Use option 1 from the Main Menu to add your first bug.");
            ReturnToMainMenu();
            return;
        }

        Console.WriteLine("This screen shows each saved bug in a simple summary format.");
        Console.WriteLine();

        foreach (BugReport bug in _bugReports)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Bug ID:       {bug.Id}");
            Console.WriteLine($"Title:        {bug.Title}");
            Console.WriteLine($"Reported By:  {bug.ReportedBy}");
            Console.WriteLine($"Date:         {bug.DateReported}");
            Console.WriteLine($"Priority:     {bug.Priority}");
            Console.WriteLine($"Status:       {bug.Status}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
        }

        ReturnToMainMenu();
    }

    public void ViewTriageResultsMenu()
    {
        Console.Clear();
        ShowHeader("View Triage Results");

        if (_bugReports.Count == 0)
        {
            Console.WriteLine("No bugs are available.");
            Console.WriteLine();
            Console.WriteLine("Use option 1 from the Main Menu to add a bug first.");
            ReturnToMainMenu();
            return;
        }

        Console.WriteLine("Choose a Bug ID below to view the full triage explanation.");
        Console.WriteLine("Enter 0 to return to the Main Menu.");
        Console.WriteLine();

        foreach (BugReport bug in _bugReports)
        {
            Console.WriteLine($"ID: {bug.Id} | Priority: {bug.Priority} | Title: {bug.Title}");
        }

        Console.WriteLine();
        Console.Write("Enter Bug ID: ");

        int selectedId = ReadNumberOrReturn();

        if (selectedId == 0)
            return;

        BugReport? selectedBug = _bugReports.FirstOrDefault(b => b.Id == selectedId);

        if (selectedBug == null)
        {
            Console.WriteLine();
            Console.WriteLine("That Bug ID was not found.");
            Console.WriteLine("Please return to the triage menu and choose one of the listed IDs.");
            ReturnToMainMenu();
            return;
        }

        ShowTriageDetails(selectedBug);
    }

    public void ViewDashboard()
    {
        Console.Clear();
        ShowHeader("Bug Dashboard");

        if (_bugReports.Count == 0)
        {
            Console.WriteLine("No dashboard information is available yet.");
            Console.WriteLine();
            Console.WriteLine("Add a bug first, then return here to see a summary.");
            ReturnToMainMenu();
            return;
        }

        int criticalCount = _bugReports.Count(b => b.Priority == "Critical");
        int highCount = _bugReports.Count(b => b.Priority == "High");
        int mediumCount = _bugReports.Count(b => b.Priority == "Medium");
        int lowCount = _bugReports.Count(b => b.Priority == "Low");

        Console.WriteLine("Overall Summary");
        Console.WriteLine("---------------");
        Console.WriteLine($"Total Bugs:     {_bugReports.Count}");
        Console.WriteLine($"Critical Bugs:  {criticalCount}");
        Console.WriteLine($"High Bugs:      {highCount}");
        Console.WriteLine($"Medium Bugs:    {mediumCount}");
        Console.WriteLine($"Low Bugs:       {lowCount}");
        Console.WriteLine();

        Console.WriteLine("What This Means");
        Console.WriteLine("---------------");

        if (criticalCount > 0)
        {
            Console.WriteLine("There are Critical bugs that should be reviewed immediately.");
        }
        else if (highCount > 0)
        {
            Console.WriteLine("There are High priority bugs that should be scheduled soon.");
        }
        else
        {
            Console.WriteLine("There are no Critical or High priority bugs at this time.");
        }

        Console.WriteLine();
        Console.WriteLine("Bug Priority Summary");
        Console.WriteLine("--------------------");

        foreach (BugReport bug in _bugReports)
        {
            Console.WriteLine($"Bug #{bug.Id}: {bug.Title}");
            Console.WriteLine($"Priority: {bug.Priority}");
            Console.WriteLine($"Result: {bug.Assessment}");
            Console.WriteLine();
        }

        ReturnToMainMenu();
    }

    public void SaveBeforeExit()
    {
        _storageService.SaveBugs(_bugReports);
    }

    private void ShowTriageDetails(BugReport bug)
    {
        Console.Clear();
        ShowHeader($"Triage Details for Bug #{bug.Id}");

        Console.WriteLine("Bug Summary");
        Console.WriteLine("-----------");
        Console.WriteLine($"Title:        {bug.Title}");
        Console.WriteLine($"Description:  {bug.Description}");
        Console.WriteLine($"Reported By:  {bug.ReportedBy}");
        Console.WriteLine($"Date:         {bug.DateReported}");
        Console.WriteLine($"Status:       {bug.Status}");
        Console.WriteLine();

        Console.WriteLine("Priority Result");
        Console.WriteLine("---------------");
        Console.WriteLine($"Priority: {bug.Priority}");
        Console.WriteLine(_triageService.GetPriorityExplanation(bug.Priority));
        Console.WriteLine();

        Console.WriteLine("Assessment");
        Console.WriteLine("----------");
        Console.WriteLine(bug.Assessment);
        Console.WriteLine();

        Console.WriteLine("Recommended Next Steps");
        Console.WriteLine("----------------------");

        for (int i = 0; i < bug.TriageSteps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {bug.TriageSteps[i]}");
        }

        ReturnToMainMenu();
    }

    private void ShowHeader(string title)
    {
        // Updated: 06/18/2026
        // Reusable screen header for a more consistent user experience.

        Console.WriteLine("========================================");
        Console.WriteLine(title.ToUpper());
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    private int ReadNumberOrReturn()
    {
        // Updated: 06/18/2026
        // Allows the user to enter 0 to return to the Main Menu.

        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (input == "0")
                return 0;

            if (int.TryParse(input, out int number))
                return number;

            Console.Write("Please enter a valid number, or 0 to return: ");
        }
    }

    private bool ReadYesNo()
    {
        // Updated: 06/18/2026
        // Makes yes/no prompts more forgiving for non-technical users.

        while (true)
        {
            string input = (Console.ReadLine() ?? "").Trim().ToLower();

            if (input == "yes" || input == "y")
                return true;

            if (input == "no" || input == "n")
                return false;

            Console.Write("Please type yes or no: ");
        }
    }

    private void ReturnToMainMenu()
    {
        // Updated: 06/18/2026
        // Standard return behavior for screens that display information.

        Console.WriteLine();
        Console.WriteLine("Enter 0 to return to the Main Menu.");

        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (input == "0")
                return;

            Console.Write("Please enter 0 to return to the Main Menu: ");
        }
    }
}