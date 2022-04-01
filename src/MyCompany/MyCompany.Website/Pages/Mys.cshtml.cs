using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCompany.Website.Models;
using MyCompany.Website.Services;

namespace MyCompany.Website.Pages
{
    public class MysModel : PageModel
    {
        private readonly IMyService _MyService;

        public List<My> Mys { get; set; }

        public MysModel(IMyService MyService)
        {
            _MyService = MyService;
        }

        public void OnGet()
        {
            Mys = _MyService.GetMys();
        }
    }
}
