using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoTI3.DomainModels;
using System.Data;
using ProjetoTI3.Models;
using Microsoft.AspNetCore.Http;

namespace ProjetoTI3.Controllers
{
    public class PerfilController : Controller
    {
        private Colaborador conta;
        #region View Listar
        public IActionResult Listar()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                List<PerfilModelRead> pfList = ph.listar();
                ViewBag.Current = "ListarPerfil";
                return View(pfList);
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
                ViewBag.Current = "CriarPerfil";
                return View();
            }
            return RedirectToAction("Login", "Colaborador");

        }
        [HttpPost]
        public IActionResult Criar(PerfilModelCreate obj)
        {
            setUserSession();
            Boolean result = false;
            if (ModelState.IsValid)
            {
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                result = ph.criar(obj);
                TempData["Success"] = "Perfil criado com sucesso!";
                if (result) return RedirectToAction("Listar", "Perfil");
            }
            return RedirectToAction("Perfil", "Erro");
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