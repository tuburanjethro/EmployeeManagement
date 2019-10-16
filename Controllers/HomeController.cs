using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        /* Note: Notice how we aren't passing a new instance of IEmployeeRepository into the constructor
         * Explain how this works.
         */
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("")]
        [Route("Index")]
        public ViewResult Index()
        {
           var model =  _employeeRepository.GetAllEmployees();
            return View(model);
        }

        [Route("Details/{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        [Route("Create")]
        public ViewResult Create()
        {
            return View();
        }
    }
}
