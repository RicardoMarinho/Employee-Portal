using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTI3.DomainModels;
using ProjetoTI3.Models;

namespace ProjetoTI3.Controllers
{
    public class TipoRelatorioController : Controller
    {
        private Colaborador conta;
        #region View Listar
        public IActionResult Listar()
        {
            setUserSession();
            if(ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                TipoRelatorio_Helper trh = new TipoRelatorio_Helper(Program._conexao);
                List<TipoRelatorioModelRead> trList = trh.listar();
                ViewBag.Current = "ListarTipoRelatorio";
                return View(trList);
            }
            return RedirectToAction("Login", "Colaborador");
            
        }
        #endregion
        #region View Criar
        [HttpGet]
        public IActionResult Criar()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                ViewBag.Current = "CriarTipoRelatorio";
                return View();
            }
            return RedirectToAction("Login", "Colaborador");

        }
        [HttpPost]
        public IActionResult Criar(TipoRelatorioModelCreate obj)
        {
            setUserSession();
            Boolean result = false;
            if (ModelState.IsValid)
            {
                TipoRelatorio_Helper trh = new TipoRelatorio_Helper(Program._conexao);
                result = trh.criar(obj);
                TempData["Success"] = "Tipo de relatório criado com sucesso!";
                if (result) return RedirectToAction("Listar", "TipoRelatorio");
            }
            return RedirectToAction("TipoRelatorio", "Erro");
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