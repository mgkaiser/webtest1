﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using webtest1.Data;
using webtest1.Models;

namespace webtest1.Controllers
{
    public class ProductController : Controller
    {        
        private readonly ILogger<ProductController> _logger;                
        private readonly IDataService<Product> _dataService;
        private readonly IDataService<Category> _categoryDataService;

        public ProductController(ILogger<ProductController> logger, IDataService<Product> dataService, IDataService<Category> categoryDataService)
        {
            _logger = logger;
            _dataService = dataService;            
            _categoryDataService = categoryDataService;
        }

        public async Task<IActionResult> Index()
        {
            return await ProductList();
        }
        
        public async Task<IActionResult> ProductList()
        {
            var viewModel = NewViewModel();
            viewModel.Data = await _dataService.Get();
            return View("List", viewModel);
        }

        public async Task<IActionResult> GetProduct(DataViewModel<Product> model)
        {
            switch (model.Action)
            {
                case "updatedata": return await UpdateData(model);                                                            
                case "insertdata": return await InsertData(model);                                                                                        
                case "edit": return await Edit(model);                                        
                case "insert": return Insert();                    
                case "delete": return await Delete(model);                             
                default: return await ProductList();
            }
            
        }       
        
        private DataViewModel<Product> NewViewModel()
        {        
            return new DataViewModel<Product>();            
        }    

        private async Task<IActionResult> UpdateData(DataViewModel<Product> model)
        {            
            if (ModelState.IsValid && await _dataService.Put(model.Id, model.Current))
            {                            
                return await ProductList();
            }
            else
            {
                return View("Edit", model);                                
            }
        }

        private async Task<IActionResult> InsertData(DataViewModel<Product> model)
        {            
            if (ModelState.IsValid && await _dataService.Post(model.Current))
            {                            
                return await ProductList();
            }
            else
            {
                return View("Insert", model);                                
            }
        }

        private async Task<IActionResult> Edit(DataViewModel<Product> model)
        {  
            var viewModel = NewViewModel();
            viewModel.Current = await _dataService.GetById(model.Id);
            return View("Edit", viewModel);                     
        }

        private IActionResult Insert()
        {                        
            return View("Insert", NewViewModel());
        }

        private async Task<IActionResult> Delete(DataViewModel<Product> model)
        {                        
            await _dataService.Delete(model.Id);
            return await ProductList();
        }

    }
}
