﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using webtest1.Data;
using webtest1.Models;

namespace webtest1.Controllers
{
    public class AboutController : Controller
    {        
        private readonly ILogger<AboutController> _logger;     
        IVersionService _versionService;

        public AboutController(ILogger<AboutController> logger, IVersionService versionService)
        {
            _logger = logger;            
            _versionService = versionService;
        }

        public async Task<IActionResult> Index()
        {            
            var model = new AboutViewModel();
            model.DataVersion = await _versionService.GetVersion();
            return View(model);
        }
                        
        public async Task<IActionResult> About()
        {            
            return await Index();
        }        
        
    }
}
