using System.Diagnostics;
using NewTiceAI.Extensions;
using NewTiceAI.Models;
using NewTiceAI.Models.ChartModels;
using NewTiceAI.Models.Enums;
using NewTiceAI.Models.ViewModels;
using NewTiceAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewTiceAI.Controllers
{
    public class HomeController : TABaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrganizationService _organizationService;
        private readonly IBTProjectService _projectService;
        private readonly IBTTicketService _ticketService;

        public HomeController(ILogger<HomeController> logger,
                              IOrganizationService organizationService,
                              IBTProjectService projectService,
                              IBTTicketService ticketService)
        {
            _logger = logger;
            _organizationService = organizationService;
            _projectService = projectService;
            _ticketService = ticketService;
        }


        //public async Task<IActionResult> DashboardCork()
        //{
        //    DashboardViewModel model = new();

        //    int _organizationId = User.Identity!.GetOrganizationId();
        //    model.Company = await _companyService.GetCompanyInfoById(_organizationId);
        //    model.Projects = await _projectService.GetAllProjectsBy_organizationIdAsync(_organizationId);
        //    model.Tickets = await _ticketService.GetAllTicketsBy_organizationIdAsync(_organizationId);
        //    model.Members = await _companyService.GetAllMembersAsync(_organizationId);

        //    return View(model);
        //}

        //[Authorize]
        //public async Task<IActionResult> Dashboard(string swalMessage = "")
        //{
        //    if (!string.IsNullOrEmpty(swalMessage))
        //    {
        //        ViewData["SwalMessage"] = swalMessage;
        //    }

        //    DashboardViewModel model = new();

        //    int _organizationId = User.Identity!.GetOrganizationId();
        //    model.Company = await _companyService.GetCompanyInfoById(_organizationId);
        //    model.Projects = await _projectService.GetAllProjectsBy_organizationIdAsync(_organizationId);
        //    model.Tickets = await _ticketService.GetAllTicketsBy_organizationIdAsync(_organizationId);
        //    model.Members = await _companyService.GetAllMembersAsync(_organizationId);    

        //    return View(model);
        //}

        [Authorize]
        public IActionResult Index(string? swalMessage = null)
        {
            ViewData["Swalmessage"] = swalMessage;

            DashboardViewModel model = new();

            int organizationId = User.Identity!.GetOrganizationId();
            //model.Organization = await _companyService.GetCompanyInfoById(_organizationId);
            //model.Accounts = await _projectService.GetAllProjectsBy_organizationIdAsync(organizationId);

            return View(model);
        }

        [Authorize]
        public IActionResult Dashboard_new(string? swalMessage = null)
        {
            ViewData["Swalmessage"] = swalMessage;  

            DashboardViewModel model = new();

            int organizationId = User.Identity!.GetOrganizationId();
            //model.Organization = await _companyService.GetCompanyInfoById(_organizationId);
            //model.Accounts = await _projectService.GetAllProjectsBy_organizationIdAsync(organizationId);

            return View(model);
        }

        public IActionResult SHRIndex()
        {
            return View();
        }

        public IActionResult Landing()
        {
            return View();
        }

        public IActionResult Landing_Cubic()
        {
            return View();
        }

        public IActionResult Landing_Ample()
        {
            return View();
        }

        public IActionResult Landing_Cork()
        {
            return View();
        }

        public IActionResult Landing_Seipkon()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<JsonResult> GglProjectTickets()
        {
            List<Project> projects = await _projectService.GetAllProjectsByOrgIdAsync(_organizationId);

            List<object> chartData = new();
            chartData.Add(new object[] { "ProjectName", "TicketCount" });

            foreach (Project prj in projects)
            {
                chartData.Add(new object[] { prj.Name!, prj.Tickets.Count() });
            }

            return Json(chartData);
        }


        [HttpPost]
        public async Task<JsonResult> GglProjectPriority()
        {
            List<Project> projects = await _projectService.GetAllProjectsByOrgIdAsync(_organizationId);

            List<object> chartData = new();
            chartData.Add(new object[] { "Priority", "Count" });


            foreach (string priority in Enum.GetNames(typeof(BTProjectPriorities)))
            {
                int priorityCount = (await _projectService.GetAllProjectsByPriorityAsync(_organizationId, priority)).Count();
                chartData.Add(new object[] { priority, priorityCount });
            }

            return Json(chartData);
        }


        [HttpPost]
        public async Task<JsonResult> AmCharts()
        {
            AmChartData amChartData = new();
            List<AmItem> amItems = new();

            List<Project> projects = (await _projectService.GetAllProjectsByOrgIdAsync(_organizationId)).Where(p => p.Archived == false).ToList();

            foreach (Project project in projects)
            {
                AmItem item = new();

                item.Project = project.Name;
                item.Tickets = project.Tickets.Count;
                item.Developers = (await _projectService.GetProjectMembersByRoleAsync(project.Id, nameof(BTRoles.Developer))).Count();

                amItems.Add(item);
            }

            amChartData.Data = amItems.ToArray();


            return Json(amChartData.Data);
        }

        [HttpPost]
        public async Task<JsonResult> PlotlyBarChart()
        {
            PlotlyBarData? plotlyData = new();
            List<PlotlyBar>? barData = new();

            List<Project> projects = await _projectService.GetAllProjectsByOrgIdAsync(_organizationId);

            //Bar One
            PlotlyBar barOne = new()
            {
                X = projects.Select(p => p.Name).ToArray()!,
                Y = projects.SelectMany(p => p.Tickets).GroupBy(t => t.ProjectId).Select(g => g.Count()).ToArray(),
                Name = "Tickets",
                Type = "bar"
            };

            //Bar Two
            PlotlyBar barTwo = new()
            {
                X = projects.Select(p => p.Name).ToArray()!,
                Y = projects.Select(async p => (await _projectService.GetProjectMembersByRoleAsync(p.Id, nameof(BTRoles.Developer))).Count).Select(c => c.Result).ToArray(),
                Name = "Developers",
                Type = "bar"
            };

            barData.Add(barOne);
            barData.Add(barTwo);

            plotlyData.Data = barData;

            return Json(plotlyData);
        }

    }
}