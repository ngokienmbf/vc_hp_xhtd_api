using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public FileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUploadAPI
        {
            public IFormFile files { get; set; }
        }

        // [HttpPost]
        // public IActionResult Post(List<IFormFile> files)
        // {
        //     try
        //     {
        //         if (files.Count == 0) return BadRequest();
        //         string directoryPath = Path.Combine()
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message.ToString());
        //     }
        // }
    }
}