using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using Project_Sylveon.Pages.Partials;

namespace Project_Sylveon.Pages
{
    public class IndexModel : PageModel
    {
        public ISubModel CodeMenu { get; } = new CodeMenuSubModel();
        public ISubModel DraggableElementsMenu { get; } = new DraggableElementsMenuSubModel();
        public ISubModel GameGrid { get; } = new GameGridSubModel();

        public ISubModel[] SubModels
        {
            get => new ISubModel[] {
                CodeMenu,
                DraggableElementsMenu,
                GameGrid,
            };
        }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            foreach(ISubModel subModel in SubModels)
                subModel.OnGet(Request, Response);
        }

        public void OnPost()
        {
            foreach(ISubModel subModel in SubModels)
                subModel.OnPost(Request, Response);
        }

        public void OnPut()
        {
            foreach(ISubModel subModel in SubModels)
                subModel.OnPut(Request, Response);
        }

        public void OnPatch()
        {
            foreach(ISubModel subModel in SubModels)
                subModel.OnPatch(Request, Response);
        }

        public void OnDelete()
        {
            foreach(ISubModel subModel in SubModels)
                subModel.OnDelete(Request, Response);
        }
    }
}
