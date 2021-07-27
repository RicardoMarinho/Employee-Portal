using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTI3.DomainModels;
using ProjetoTI3.Models;

namespace ProjetoTI3.Controllers
{
    public class RelatorioController : Controller
    {
        private Colaborador conta;

        #region Ação Listar
        public IActionResult Listar()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                TipoRelatorio_Helper trh = new TipoRelatorio_Helper(Program._conexao);
                Relatorio_Helper rh = new Relatorio_Helper(Program._conexao);
                List<TipoRelatorioModelRead> trList = trh.listar();
                List<RelatorioModelRead> relView = rh.listar(ViewBag.Conta._uidColaborador.ToString());
                ViewBag.TipoRelatorio = trList;
                ViewBag.Current = "ListarRelatorio";
                return View(relView);
            }
            return RedirectToAction("Login", "Colaborador");


        }
        #endregion
        #region Ação Ler
        public IActionResult Ler(string id)
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Relatorio_Helper rh = new Relatorio_Helper(Program._conexao);
                RelatorioModelEdit rme = rh.ler(id);
                return View(rme);
            }
            return RedirectToAction("Login", "Colaborador");

        }
        #endregion
        #region Ação Criar
        [HttpGet]
        public IActionResult Criar()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                TipoRelatorio_Helper trh = new TipoRelatorio_Helper(Program._conexao);
                List<TipoRelatorioModelRead> lstTipoRelatorio = trh.listar();
                ViewBag.TipoRelatorio = lstTipoRelatorio;
                ViewBag.Current = "CriarRelatorio";
                return View();
            }
            return RedirectToAction("Login", "Colaborador");
        }
        [HttpPost]
        public IActionResult Criar(RelatorioModelCreate obj)
        {
            setUserSession();
            Boolean result = false;
            if (ModelState.IsValid)
            {
                TempData["Success"] = "Relatório registado com sucesso!";
                Relatorio_Helper rh = new Relatorio_Helper(Program._conexao);
                result = rh.criar(obj);
                if (result) return RedirectToAction("Listar", "Relatorio");
            }
            return RedirectToAction("Relatorio", "Erro");
        }
        #endregion
        #region Ação Editar
        [HttpGet]
        public IActionResult Editar(string id)
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault) { 
                RelatorioModelEdit rme;
            Relatorio_Helper rh = new Relatorio_Helper(Program._conexao);
            rme = rh.ler(id);
            if (rme != null)
            {
                TipoRelatorio_Helper trh = new TipoRelatorio_Helper(Program._conexao);
                List<TipoRelatorioModelRead> lstTipoRelatorio = trh.listar();
                ViewBag.Departamento = lstTipoRelatorio;
                return View(rme);
            }else return RedirectToAction("Relatorio", "Erro");
            }
            return RedirectToAction("Login", "Colaborador");
        }
        [HttpPost]
        public IActionResult Editar(RelatorioModelEdit obj)
        {
            setUserSession();
            Boolean result = false;
            if (ModelState.IsValid)
            {
                Relatorio_Helper rh = new Relatorio_Helper(Program._conexao);
                result = rh.editar(obj);
                if (result) return RedirectToAction("Listar", "Relatorio");
            }
            return RedirectToAction("Relatorio", "Erro");
        }
        #endregion
        #region Set Session
        private void setUserSession()
        {
            Perfil_Helper ph = new Perfil_Helper(Program._conexao);
            Departamento_Helper dh = new Departamento_Helper(Program._conexao);
            string uid = "";
            try
            {
                uid = HttpContext.Session.GetString("id");
            }
            catch
            {
                HttpContext.Session.SetString("id", "");
                uid = "";
            }
            conta = Program.getSessionColaborador(uid);
            HttpContext.Session.SetString("id", conta._uidColaborador.ToString());
            ViewBag.Conta = (Colaborador)conta;
            if (ViewBag.Conta._perfil.ToString() != Program._uidDefault && ViewBag.Conta._departamento.ToString() != Program._uidDefault)
            {
                ViewBag.NomePerfil = ph.ler(ViewBag.Conta._perfil.ToString());
                ViewBag.NomeDepartamento = dh.ler(ViewBag.Conta._departamento.ToString());
            }
        }
        #endregion

    }
}