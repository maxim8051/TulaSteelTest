using Microsoft.AspNetCore.Mvc;
using MvcTest.Data;
using System.Linq;
using MvcTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Hosting;
using MvcTest.Utils;
using System.Threading.Tasks;

namespace MvcTest.Controllers
{
    public class WorkerController : Controller
    {

        private readonly DataContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public WorkerController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }


        //Вывод таблицы 
        [HttpGet]
        public async Task<IActionResult> Index(int currentPageIndex=1,  string searchBy = "", string searchValue = "")
        {

            return View(await GetWorkerList(currentPageIndex, searchBy, searchValue));
        }


        private async Task<PageInfo> GetWorkerList(int currentPage, string searchBy, string searchValue)
        {
            double pageCount;
            int maxRowsPerPage = 7;
            PageInfo pageModel = new PageInfo();   
            
            if (string.IsNullOrEmpty(searchValue))
            {
                pageModel.Workers = await _context.Workers.Include(w => w.Department)
                                                   .OrderByDescending(w => w.Id)
                                                   .Skip((currentPage - 1) * maxRowsPerPage)
                                                   .Take(maxRowsPerPage).ToListAsync();

                pageCount = (double)((decimal)_context.Workers.Count() / Convert.ToDecimal(maxRowsPerPage));
            }
            else
            {
                switch (searchBy)
                {
                    case "FullName":
                        pageModel.Workers = await _context.Workers.Include(w => w.Department)
                                                    .Where(w => w.FullName.Contains(searchValue))
                                                    .OrderByDescending(w => w.Id)
                                                    .Skip((currentPage - 1) * maxRowsPerPage)
                                                    .Take(maxRowsPerPage).ToListAsync();

                        pageCount = (double)((decimal)_context.Workers.Where(w => w.FullName.Contains(searchValue))
                                                                             .Count() / Convert.ToDecimal(maxRowsPerPage));
                        break;

                    case "Phone":
                        pageModel.Workers = await _context.Workers.Include(w => w.Department)
                                                    .Where(w => w.Phone.StartsWith(searchValue))
                                                    .OrderByDescending(w => w.Id)
                                                    .Skip((currentPage - 1) * maxRowsPerPage)
                                                    .Take(maxRowsPerPage).ToListAsync();

                        pageCount = (double)((decimal)_context.Workers.Where(w => w.Phone.StartsWith(searchValue))
                                                                             .Count() / Convert.ToDecimal(maxRowsPerPage));

                        break;

                    default:
                        {
                            pageModel.Workers = await _context.Workers.Include(w => w.Department)
                                                   .OrderByDescending(w => w.Id)
                                                   .Skip((currentPage - 1) * maxRowsPerPage)
                                                   .Take(maxRowsPerPage).ToListAsync();

                            pageCount = (double)((decimal)_context.Workers.Where(w => w.FullName.Contains(searchValue))
                                                                             .Count() / Convert.ToDecimal(maxRowsPerPage));

                            break;
                        }
                }
            }
            
            pageModel.PageCount = (int)Math.Ceiling(pageCount);
            pageModel.CurrentPageIndex = currentPage;
            return pageModel;
        
        }

        //Создание сотрудника
        [HttpGet]
        public IActionResult Create()
        {
            SelectList departments = new SelectList(_context.Department, "Id", "Title");
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(WorkerViewModel model)
        {
            WorkerUtils utils = new WorkerUtils(webHostEnvironment);
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = utils.UploadedFile(model);
                    Worker worker = new Worker 
                    { 
                        Name = model.Name,
                        Surname = model.Surname,
                        Patronymic = model.Patronymic,
                        FullName = $"{model.Surname} {model.Name} {model.Patronymic}",
                        Phone = model.Phone,
                        DepartmentID = model.DepartmentID,
                        PathPhoto = uniqueFileName
                    };
                    _context.Workers.Add(worker);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Сотрудник добавален.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Данные сотрудника введены некорректно.";
                    return Create();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Create();
            }
        }

        //Обновление информации о сотруднике
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                Worker model = _context.Workers.SingleOrDefault(w => w.Id == id);
                var worker = new WorkerViewModel
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    Phone = model.Phone,
                    DepartmentID = model.DepartmentID,
                    PathPhoto = model.PathPhoto
                };

                if (worker != null)
                {
                    SelectList departments = new SelectList(_context.Department, "Id", "Title");
                    ViewBag.Departments = departments;
                    return View(worker);
                }
                else
                {
                    TempData["ErrorMessage"] = "Сотрудник с данным ID не найден.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Edit(WorkerViewModel model)
        {
            WorkerUtils utils = new WorkerUtils(webHostEnvironment);
            try
            {
                if (ModelState.IsValid)
                {
                    string pathPhoto = null;
                    if (model.Photo != null && utils.CheckExt(model.Photo.FileName))
                    {   
                        System.IO.File.Delete(utils.GetPathPhoto(model.PathPhoto));
                        pathPhoto = utils.UploadedFile(model);
                    }
                    else
                    {
                        pathPhoto = model.PathPhoto;
                    }


                    Worker worker = new Worker
                    {   
                        Id = model.Id,
                        Name = model.Name,
                        Surname = model.Surname,
                        Patronymic = model.Patronymic,
                        FullName = $"{model.Surname} {model.Name} {model.Patronymic}",
                        Phone = model.Phone,
                        DepartmentID = model.DepartmentID,
                        PathPhoto = pathPhoto
                    };

                    _context.Workers.Update(worker);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Информация о сотруднике обновлена.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Данные сотрудника введены некорректно.";
                    return Edit(model.Id);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Edit(model.Id);
            }
        }


        //Удаление информации о сотруднике
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                Worker worker = _context.Workers.Include(w => w.Department).SingleOrDefault(w => w.Id == id);

                if (worker != null)
                {
                    return View(worker);
                }
                else
                {
                    TempData["ErrorMessage"] = "Сотрудник с данным ID не найден.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Delete(Worker model)
        {
            WorkerUtils utils = new WorkerUtils(webHostEnvironment);
            try
            {
                var worker = _context.Workers.SingleOrDefault(w => w.Id == model.Id);

                if (worker != null)
                {
                    _context.Workers.Remove(worker);
                    _context.SaveChanges();
                    System.IO.File.Delete(utils.GetPathPhoto(worker.PathPhoto));
                    TempData["successMessage"] = "Информация о сотруднике удалена.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Сотрудник с данным ID не найден.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Delete(model.Id);
            }
        }



    }
}
