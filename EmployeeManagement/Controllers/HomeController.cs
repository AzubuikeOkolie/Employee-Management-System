using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeRepository _employeRepository;
        [Obsolete]
        private IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1041:Provide ObsoleteAttribute message", Justification = "<Pending>")]
        public HomeController(IEmployeRepository employeRepository,
                               IHostingEnvironment hostingEnvironment,
                               ILogger<HomeController> logger)
        {
            _employeRepository = employeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeRepository.GetAllEmployees();
            return View(model);

        }
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {

            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            Employee employee = _employeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 400;
                return View("EmployeeNotFound", id.Value);
            }



            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }
        [HttpGet]
       
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
       
        public ViewResult Edit(int id)
        {
            Employee employee = _employeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
       
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.ExistingPhotoPath != null)
                {
                   string filePath = Path.Combine(hostingEnvironment.WebRootPath, 
                        "Images", model.ExistingPhotoPath); 
                    System.IO.File.Delete(filePath);
                }
               employee.PhotoPath  = ProcesUploadedFile(model);

               

                _employeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcesUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null )
            {
               
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                   

               
            }

            return uniqueFileName;
        }

        [HttpPost]
       
        public IActionResult Create(EmployeeCreateViewModel model )
        {
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                if (model.Photo != null )
                {

                    string uploadsFolder = ProcesUploadedFile(model);
                        

                    
                } 

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
        }
            return View();
        }
    } 
}

