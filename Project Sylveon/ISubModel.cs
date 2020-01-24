using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Project_Sylveon.Pages
{
    public interface ISubModel
    {
        void OnGet(HttpRequest request, HttpResponse response);
        void OnPost(HttpRequest request, HttpResponse response);
        void OnPut(HttpRequest request, HttpResponse response);
        void OnPatch(HttpRequest request, HttpResponse response);
        void OnDelete(HttpRequest request, HttpResponse response);
    }
}