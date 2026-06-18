using BugTriageAnalyzer;

// Jeff O'Hara

// Updated: 06/18/2026
// Program.cs now keeps the main menu visible and simple.
// AppManager handles the larger user experience screens.

AppManager app = new AppManager();

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("      BUG TRIAGE PRIORITY ANALYZER       ");
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("What would you like to do?");
    Console.WriteLine();
    Console.WriteLine("1. Add a New Bug to Analyze");
    Console.WriteLine("2. View All Bugs");
    Console.WriteLine("3. View Triage Results by Bug ID");
    Console.WriteLine("4. View Dashboard");
    Console.WriteLine("5. Exit Program");
    Console.WriteLine();

    Console.Write("Enter your choice: ");
    string choice = Console.ReadLine() ?? "";

    if (choice == "1")
    {
        app.AddNewBug();
    }
    else if (choice == "2")
    {
        app.ViewAllBugs();
    }
    else if (choice == "3")
    {
        app.ViewTriageResultsMenu();
    }
    else if (choice == "4")
    {
        app.ViewDashboard();
    }
    else if (choice == "5")
    {
        app.SaveBeforeExit();
        running = false;
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("That was not a valid option. Please choose 1 through 5.");
        Console.WriteLine("Press Enter to try again...");
        Console.ReadLine();
    }
}

Console.WriteLine();
Console.WriteLine("Thank you for using Bug Triage Priority Analyzer.");