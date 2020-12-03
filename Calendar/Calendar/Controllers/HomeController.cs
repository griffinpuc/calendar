﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calendar.Models;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private dbContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new dbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEvent()
        {
            return View();
        }


    }
}
