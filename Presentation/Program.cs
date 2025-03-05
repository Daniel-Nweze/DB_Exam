using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Factories;
using Data.Interfaces;
using Data.Repositories;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

// Dependency injection och databaskontext
var serviceCollection = new ServiceCollection();

// Connection string som pekar mot min databas
serviceCollection.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Data Source=localhost;Initial Catalog=dbExam;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True"));

// Registrering av repositories och service med Dependecy Injection
serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
serviceCollection.AddScoped<IProjectService, ProjectService>();
// serviceCollection.AddScoped<IEmployeeService, EmployeeService>(); // används inte
serviceCollection.AddScoped<ICustomerService, CustomerService>();
serviceCollection.AddScoped<IProjectManagerService, ProjectManagerService>();
serviceCollection.AddScoped<IServiceService, ServiceService>();

var serviceProvider = serviceCollection.BuildServiceProvider();
var projectService = serviceProvider.GetRequiredService<IProjectService>();
var customerService = serviceProvider.GetRequiredService<ICustomerService>();
var projectManagerService = serviceProvider.GetRequiredService<IProjectManagerService>();
var serviceService = serviceProvider.GetRequiredService<IServiceService>();


try
{
    await RunApplicationAsync(projectService, customerService, projectManagerService, serviceService);
}
catch (Exception ex)
{
    ShowError($"Ett fel inträffade: {ex.Message}");
    Console.ReadKey();
}

// Kör appen
async Task RunApplicationAsync(IProjectService projectService,
    ICustomerService customerService,
    IProjectManagerService projectManagerService,
    IServiceService serviceService)
{
    bool exit = false;
    while (!exit)
    {
        int option = ShowMenuAndGetOption();
        try
        {
            switch (option)
            {
                case 1:
                    await ListProjects(projectService);
                    break;
                case 2:
                    await CreateProject(projectService, customerService, projectManagerService, serviceService);
                    break;
                case 3:
                    await EditProject(projectService, customerService, projectManagerService, serviceService);
                    break;
                case 4:
                    await DeleteProject(projectService);
                    break;
                case 5:
                    exit = true;
                    break;
                default:
                    ShowError("Ogiltigt val.");
                    break;
            }
        }
        catch
        {
            ShowError("Något gick fel. Försök igen.");
        }
    }
}

// Skapa projekt 
async Task CreateProject(IProjectService projectService,
    ICustomerService customerService,
    IProjectManagerService projectManagerService,
    IServiceService serviceService)
{
    try
    {
        Console.Clear();
        Console.WriteLine("Skapa nytt projekt");

        string projectName = GetValidInput("Ange projektets namn: ");
        DateTime startDate = GetValidDate("Ange projektets startdatum (yyyy-MM-dd): ");
        DateTime endDate;
        do
        {
            endDate = GetValidDate("Ange projektets slutdatum (yyyy-MM-dd): ");
            if (endDate < startDate)
            {
                ShowError("Slutdatum kan inte vara tidigare än startdatum.");
            }
        } while (endDate < startDate);

        decimal totalPrice = GetValidPrice("Ange totalpris: ");

        var customer = await ChooseOrCreateCustomer(customerService);
        var projectManager = await ChooseOrCreateProjectManager(projectManagerService);
        var service = await ChooseOrCreateService(serviceService);

   

        ProjectStatus status = GetValidProjectStatus("Ange projektstatus (EjPåbörjad, Pågående, Avslutad): ");

        var project = ProjectFactory.CreateProject(projectName, startDate, endDate, totalPrice, customer, projectManager, service, status);

        await projectService.CreateProjectAsync(project);

        Console.WriteLine("\nProjektet har skapats. Tryck på valfri tangent för att återgå till menyn... ");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        ShowError($"Fel vid skapande av projekt: {ex.Message}");
        Console.ReadKey();
    }

}



// Detaljvy för projekt och redigering
async Task ShowAndEditProjectDetails(IProjectService projectService, ICustomerService customerService, IProjectManagerService projectManagerService, IServiceService serviceService, Project project)
{
    bool editing = true;
    while (editing)
    {
        Console.Clear();
        Console.WriteLine($"Redigera projekt {project.ProjectId} (Projektnumret går ej att ändra)");
        Console.WriteLine($"1. Namn: {project.ProjectName}");
        Console.WriteLine($"2. Startdatum: {project.StartDate:yyyy-MM-dd}");
        Console.WriteLine($"3. Slutdatum: {project.EndDate:yyyy-MM-dd}");
        Console.WriteLine($"4. Totalpris: {project.TotalPrice}");
        Console.WriteLine($"5. Status: {project.Status}");
        Console.WriteLine($"6. Projektansvarig: {(project.ProjectManager != null ? project.ProjectManager.FullName : "Ej angiven")}");
        Console.WriteLine($"7. Kund: {(project.Customer != null ? project.Customer.Name : "Ej angiven")}");
        Console.WriteLine($"8. Tjänst: {(project.Service != null ? project.Service.Description : "Ej angiven")}");
        Console.WriteLine("\nAnge siffran för att ändra fältet, [S] för att spara, eller [A] för att avbryta:");
        string choice = Console.ReadLine()?.Trim().ToLower() ?? "";
        switch (choice)
        {
            case "1":
                project.ProjectName = GetValidInput("Ange nytt namn: ");
                break;
            case "2":
                project.StartDate = GetValidDate("Ange nytt startdatum (yyyy-MM-dd): ");
                break;
            case "3":
                project.EndDate = GetValidDate("Ange nytt slutdatum (yyyy-MM-dd): ");
                break;
            case "4":
                project.TotalPrice = GetValidPrice("Ange nytt totalpris: ");
                break;
            case "5":
                project.Status = GetValidProjectStatus("Ange ny status (EjPåbörjad, Pågående, Avslutad): ");
                break;
            case "6":
                project.ProjectManager = await ChooseOrCreateProjectManager(projectManagerService);
                break;
            case "7":
                project.Customer = await ChooseOrCreateCustomer(customerService);
                break;
            case "8":
                project.Service = await ChooseOrCreateService(serviceService);
                break;
            case "s":
                await projectService.UpdateProjectAsync(project);
                Console.WriteLine("Ändringarna har sparats. Tryck på en tangent för att återgå.");
                Console.ReadKey();
                editing = false;
                break;
            case "a":
                Console.WriteLine("Ändringar avbrutna. Tryck på en tangent för att återgå.");
                Console.ReadKey();
                editing = false;
                break;
            default:
                ShowError("Ogiltigt val, försök igen.");
                break;
        }
    }
}

// Projektöversikt - visar alla projekt
async Task ListProjects(IProjectService projectService)
{
    try
    {
        Console.Clear();
        Console.WriteLine("\nProjektöversikt\n");

        var projects = await projectService.GetAllProjectsAsync();
        if (!projects.Any())
        {
            ShowError("Inga projekt hittades i databasen.");
            Console.Write("Tryck på valfri tangent för att återgå till menyn... ");
            return;
        }

        await EditProject(projectService, customerService, projectManagerService, serviceService);
    }
    catch (Exception ex)
    {
        ShowError($"Fel vid listning av projekt: {ex.Message}");
    }
}

// Redigera projekt
async Task EditProject(IProjectService projectService, ICustomerService customerService, IProjectManagerService projectManagerService, IServiceService serviceService)
{
    try
    {
        Console.Clear();

        Console.WriteLine(" Redigera projekt\n");


        var projects = await projectService.GetAllProjectsAsync();
        if (!projects.Any())
        {
            ShowError("Inga projekt hittades i databasen.");
            return;
        }

        foreach (var project in projects)
        {
            Console.WriteLine($"Id: {project.ProjectId} " +
                $"| Namn: {project.ProjectName} " +
                $"| Tidsperiod: {project.StartDate:yyyy-MM-dd} - {project.EndDate:yyyy-MM-dd} " +
                $"| Status: {project.Status}");
        }

        int projectId;
        while (true)
        {
            Console.Write("\nAnge projektnummer att redigera (eller tryck [Enter] för att återgå): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (int.TryParse(input, out projectId))
            {
                break;
            }

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            ShowError("Ogiltigt ID. Ange en siffra.");
        }

        var selectedProject = await projectService.GetProjectByIdAsync(projectId);
        if (selectedProject == null)
        {
            ShowError($"Projekt med ID {projectId} hittades inte.");
            Console.WriteLine("Tryck på valfri tangent för att återgå... ");
            Console.ReadKey();
            return;
        }

        await ShowAndEditProjectDetails(projectService, customerService, projectManagerService, serviceService, selectedProject);


    }
    catch (Exception ex)
    {
        ShowError($"Fel vid redigering av projekt: {ex.Message}");
    }
}

// Radera projekt
async Task DeleteProject(IProjectService projectService)
{
    Console.Clear();
    Console.WriteLine("Radera projekt");

    var projects = await projectService.GetAllProjectsAsync();
    if (!projects.Any())
    {
        ShowError("Inga projekt hittades.");
        return;
    }

    foreach (var project in projects)
    {
        Console.WriteLine($"Projektnummer: {project.ProjectId} | Namn: {project.ProjectName}");
    }
    string input; int projectId;
    do
    {
        Console.Write("\nAnge projektnummer att radera (eller tryck Enter för att återgå): ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(input))
            return;

        if (int.TryParse(input, out projectId))
        {
            var project = await projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                ShowError($"Projekt med nummer {projectId} hittades inte.");
                return;
            }
            Console.WriteLine($"Är du säker på att du vill radera projekt '{project?.ProjectName}'? (J/N)");
            if (Console.ReadLine()?.Trim().ToLower() == "j")
            {
                await projectService.DeleteProjectAsync(projectId);
                Console.WriteLine("Projektet har raderats.");
            }
            else
            {
                Console.WriteLine("Åtgärden avbruten.");
            }
        }
        else
        {
            ShowError("Felaktigt id, försök igen.");
        }
    } while (string.IsNullOrEmpty(input) || !int.TryParse(input, out projectId));

    Console.ReadKey();
}

/* Validerings- och felmeddelande metoder samt en meny metod för att göra huvudkoden mer robust.
 Tagit hjälp av Chatgpt för att få fram vilka metoder som är logiska att använda.
*/

async Task<Customer> ChooseOrCreateCustomer(ICustomerService customerService)
{
    Console.WriteLine("Vill du använda en befintlig kund? (J/N)");
    if (Console.ReadLine()?.Trim().ToLower() == "j")
    {
        var customers = await customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Inga kunder hittades. Skapa ny kund");
            return GetValidCustomer();
        }

        Console.WriteLine("\nVälj en befintlig kund: ");
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.CustomerId} | Namn: {customer.Name}");
        }

        Console.Write("\nAnge kundens ID: ");
        if (int.TryParse(Console.ReadLine(), out int customerId))
        {
            var existingCustomer = await customerService.GetCustomerByIdAsync(customerId);
            if (existingCustomer != null)
            {
                return existingCustomer;
            }
            ShowError("Felaktigt ID. Ny kund skapas.");
        }
    }

    return GetValidCustomer();
}
async Task<ProjectManager> ChooseOrCreateProjectManager(IProjectManagerService projectManagerService)
{
    Console.WriteLine("Vill du använda en befintlig projektansvarig? (J/N)");
    if (Console.ReadLine()?.Trim().ToLower() == "j")
    {
        var projectManagers = await projectManagerService.GetAllProjectManagersAsync();
        if (!projectManagers.Any())
        {
            Console.WriteLine("Inga projektansvariga finns. Skapa ny projektansvarig.");
            return GetValidProjectManager();
        }

        Console.WriteLine("\nVälj en befintlig projektansvarig:");
        foreach (var manager in projectManagers)
        {
            Console.WriteLine($"ID: {manager.ProjectManagerId} | Namn: {manager.FullName}");
        }

        Console.Write("\nAnge projektansvarigs ID: ");
        if (int.TryParse(Console.ReadLine(), out int managerId))
        {
            var existingManager = await projectManagerService.GetProjectManagerByIdAsync(managerId);
            if (existingManager != null)
            {
                return existingManager;
            }
            ShowError("Felaktigt ID. Ny projektansvarig skapas.");
        }
    }
    return GetValidProjectManager();
}
async Task<Service> ChooseOrCreateService(IServiceService serviceService)
{
    Console.WriteLine("Vill du använda en befintlig tjänst? (J/N)");
    if(Console.ReadLine()?.Trim().ToLower() == "j")
    {
        var services = await serviceService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Inga tjänster finns. Skapa ny tjänst.");
            return GetValidService();
        }

        Console.WriteLine("\nVälj en befintlig tjänst:");
        foreach(var service in services)
        {
            Console.WriteLine($"ID: {service.ServiceId} | {service.Description}");
        }

        Console.WriteLine("\nAnge tjänstens ID: ");
        if(int.TryParse(Console.ReadLine(), out int serviceId))
        {
            var existingService = await serviceService.GetServiceByIdAsync(serviceId);
            if(existingService != null)
            {
                return existingService;
            }
            ShowError("Felaktigt ID. Ny tjänst skapas.");

        }
    }
    return GetValidService();
}

string GetValidInput(string prompt)
{
    string input;
    do
    {
        Console.Write(prompt);
        input = Console.ReadLine() ?? "".Trim();
        if (string.IsNullOrEmpty(input))
        {
            ShowError("Detta fält får inte vara tomt! Försök igen.");
        }
    } while (string.IsNullOrEmpty(input));

    return input;
}

DateTime GetValidDate(string prompt)
{
    DateTime date;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        if (DateTime.TryParse(input, out date))
        {
            return date;
        }
        ShowError("Ogiltigt datumformat. Ange yyyy-MM-dd.");
    }
}


decimal GetValidPrice(string prompt)
{
    decimal price;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        if (decimal.TryParse(input, out price) && price > 0)
        {
            return price;
        }
        ShowError("Ange ett giltigt pris (positivt tal).");
    }
}
Customer GetValidCustomer()
{
    var customer = new Customer
    {
        Name = GetValidInput("Ange kundens fullständiga namn: "),
        ContactInfo = GetValidInput("Ange kundens kontaktinfo: ")

    };
    return customer;

}

ProjectManager GetValidProjectManager()
{
    var projectManager = new ProjectManager
    {
        FullName = GetValidInput("Ange projektansvarigs fullständiga namn: "),
        Email = GetValidInput("Ange projektansvarigs e-post: ")
    };
    return projectManager;
}

ProjectStatus GetValidProjectStatus(string prompt)
{
    ProjectStatus status;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        if (Enum.TryParse(input, true, out status) && Enum.IsDefined(typeof(ProjectStatus), status))
        {
            return status;
        }
        ShowError("Ogiltig status. Ange EjPåbörjad, Pågående eller Avslutad.");
    }
}

Service GetValidService()
{
    var service = new Service
    {
        Description = GetValidInput("Ange tjänstens beskrivning: "),
        Price = GetValidPrice("Ange tjänstens pris: ")
    };
    return service;
}

/* Chatgpt-generarad
    Metod som visar menyn och prompt. Syfte att göra koden mer robust.*/
int ShowMenuAndGetOption()
{
    Console.Clear();
    Console.WriteLine("Projektadministration\n");

    Console.WriteLine("[1] Lista alla projekt");
    Console.WriteLine("[2] Skapa nytt projekt");
    Console.WriteLine("[3] Redigera projekt");
    Console.WriteLine("[4] Radera projekt");
    Console.WriteLine("[5] Avsluta program\n");
    Console.Write("Ange ditt val: ");

    if (int.TryParse(Console.ReadLine(), out int option))
    {
        return option;
    }
    else
    {
        return -1;
    }
}

void ShowError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n{message}\n");
    Console.ReadKey();
    Console.ResetColor();
}
