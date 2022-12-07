﻿using BaseDeProjetos.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;

namespace BaseDeProjetos.Controllers
{
    public class ProducaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProducaoController(ApplicationDbContext context)
        {
            _context = context;
        }
        private List<Producao> DefinirCasaParaVisualizar(string? casa)
        {
            Instituto enum_casa;

            if (Enum.IsDefined(typeof(Instituto), casa))
            {
                HttpContext.Session.SetString("_Casa", casa);
                enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
            }
            else
            {
                enum_casa = Instituto.Super;
            }


            List<Producao> publicacoes = new List<Producao>();

            List<Producao> lista = enum_casa == Instituto.Super ?
            _context.Producao.ToList() :
            _context.Producao.Where(p => p.Casa.Equals(enum_casa)).ToList();

            publicacoes.AddRange(lista);

            ViewData["Area"] = casa;

            return publicacoes.ToList();
        }

        // GET: Producao
        public IActionResult Index(string casa, string searchString = "", string ano = "")
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            if (string.IsNullOrEmpty(casa))
            {
                casa = usuario.Casa.ToString();

            }
            List<Producao> lista = DefinirCasaParaVisualizar(casa);
            lista = FunilHelpers.VincularCasaProducao(usuario, lista);
            lista = FunilHelpers.PeriodizarProduções(ano, lista);
            lista = FunilHelpers.FiltrarProduções(searchString, lista);
            return View(lista.ToList());
        }

        // GET: Producao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producao = await _context.Producao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producao == null)
            {
                return NotFound();
            }

            return View(producao);
        }

        // GET: Producao/Create
        public IActionResult Create()
        {
            List<Empresa> empresas = _context.Empresa.ToList();
            List<Projeto> projetos = _context.Projeto.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
            return View();
        }

        // POST: Producao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grupo,Casa,Titulo,Descricao,Autores,StatusPub,Data,Local,DOI,Imagem,Projeto,Empresa,Responsavel,NumPatente")] Producao producao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producao);
        }

        // GET: Producao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producao = await _context.Producao.FindAsync(id);
            if (producao == null)
            {
                return NotFound();
            }
            List<Empresa> empresas = _context.Empresa.ToList();
            List<Projeto> projetos = _context.Projeto.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
            return View(producao);
        }

        // POST: Producao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grupo,Casa,Titulo,Descricao,Autores,StatusPub,Data,Local,DOI,Imagem,Projeto,Empresa,Responsavel,NumPatente")] Producao producao)
        {
            if (id != producao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducaoExists(producao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producao);
        }

        // GET: Producao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producao = await _context.Producao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producao == null)
            {
                return NotFound();
            }

            return View(producao);
        }

        // POST: Producao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producao = await _context.Producao.FindAsync(id);
            _context.Producao.Remove(producao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducaoExists(int id)
        {
            return _context.Producao.Any(e => e.Id == id);
        }
    }
}
