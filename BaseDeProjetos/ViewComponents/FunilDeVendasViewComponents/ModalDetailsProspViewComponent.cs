﻿using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalDetailsProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDetailsProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Prospeccao prospeccao = await _context.Prospeccao.FindAsync(id);
            return View(prospeccao);
        }
    }
}