using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics;
using WeatherArchive.Models;

namespace WeatherArchive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Archive()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
                try
                {
                    DataTable dtTable = new DataTable();
                    List<string> rowList = new List<string>();
                    ISheet sheet;

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                        sheet = xssWorkbook.GetSheetAt(0);
                        IRow headerRow = sheet.GetRow(0);



                        stream.Position = 0;
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            while (reader.Read()) //Each row of the file
                            {
                                users.Add(new UserModel { Name = reader.GetValue(0).ToString(), Email = reader.GetValue(1).ToString(), Phone = reader.GetValue(2).ToString() });
                            }
                        }
                    }

                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}