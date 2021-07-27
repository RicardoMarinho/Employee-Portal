using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjetoTI3.Models;
using ProjetoTI3.DomainModels;
using Microsoft.AspNetCore.Http;

namespace ProjetoTI3.Controllers
{
    public class ColaboradorController : Controller
    {
        private Colaborador conta;
        #region Autenticar
        [HttpGet]
        public IActionResult Login()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() == Program._uidDefault)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(ColaboradorModelSession obj)
        {
            setUserSession();
            Colaborador_Helper cah = new Colaborador_Helper(Program._conexao);
            conta = cah.autenticar(obj._email, obj._password);
            if (conta._nome != "")
            {
                HttpContext.Session.SetString("id", conta._uidColaborador.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Login"] = "Email ou password inválidos";
                return RedirectToAction("Login", "Colaborador");
            }

        }
        #endregion
        #region Terminar sessão
        [HttpGet]
        public IActionResult Sair()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                conta = ch.sair();
                HttpContext.Session.SetString("id", conta._uidColaborador.ToString());
            }
            return RedirectToAction("Login", "Colaborador");
        }
        #endregion
        #region Ação Listar Ativos e Inativos
        public IActionResult Listar()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                Departamento_Helper dh = new Departamento_Helper(Program._conexao);
                List<ColaboradorModelRead> colView = ch.listar(Colaborador.Estado.ATIVO);
                List<PerfilModelRead> lstPerfil = ph.listar();
                List<DepartamentoModelRead> lstDepartamento = dh.listar();
                ViewBag.Perfil = lstPerfil;
                ViewBag.Departamento = lstDepartamento;
                ViewBag.Current = "ListarColaborador";
                ViewBag.Estado = "Inativos";
                return View(colView);
            }
            return RedirectToAction("Login", "Colaborador");
        }
        public IActionResult ListarInativos()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                Departamento_Helper dh = new Departamento_Helper(Program._conexao);
                List<ColaboradorModelRead> colView = ch.listar(Colaborador.Estado.INATIVO);
                List<PerfilModelRead> lstPerfil = ph.listar();
                List<DepartamentoModelRead> lstDepartamento = dh.listar();
                ViewBag.Perfil = lstPerfil;
                ViewBag.Departamento = lstDepartamento;
                ViewBag.Current = "ListarColaborador";
                ViewBag.Estado = "Ativos";
                return View(colView);
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
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                ColaboradorModelEdit cme = ch.ler(id);
                return View(cme);
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
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                Departamento_Helper dh = new Departamento_Helper(Program._conexao);
                List<PerfilModelRead> lstPerfil = ph.listar();
                List<DepartamentoModelRead> lstDepartamento = dh.listar();
                ViewBag.Perfil = lstPerfil;
                ViewBag.Departamento = lstDepartamento;
                ViewBag.Current = "CriarColaborador";
                return View();
            }
            return RedirectToAction("Login", "Colaborador");
        }
        [HttpPost]
        public IActionResult Criar(ColaboraborModelCreate obj)
        {

            Boolean result = false;
            if (ModelState.IsValid)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                if (ch.validar(obj._email).Count > 0)
                {
                    TempData["Error"] = "Já existe um colaborador com este email registado";
                    return RedirectToAction("Criar", "Colaborador");
                }
                else
                {
                    result = ch.criar(obj);
                    TempData["Success"] = "Colaborador criado com sucesso!";
                    if (result) return RedirectToAction("Listar", "Colaborador");
                }

            }
            return RedirectToAction("Colaborador", "Erro");
        }
        #endregion
        #region Ação Editar
        [HttpGet]
        public IActionResult Editar(string id)
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                ColaboradorModelEdit cme;
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                cme = ch.ler(id);
                if (cme != null)
                {
                    Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                    Departamento_Helper dh = new Departamento_Helper(Program._conexao);
                    List<PerfilModelRead> lstPerfil = ph.listar();
                    List<DepartamentoModelRead> lstDepartamento = dh.listar();
                    ViewBag.Perfil = lstPerfil;
                    ViewBag.Departamento = lstDepartamento;
                    return View(cme);
                }
                else return RedirectToAction("Colaborador", "Erro");
            }
            return RedirectToAction("Login", "Colaborador");
        }
        [HttpPost]
        public IActionResult Editar(ColaboradorModelEdit obj)
        {
            Boolean result = false;
            if (ModelState.IsValid)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                var emailOriginal = ch.ler(obj._uidColaborador.ToString());

                if (emailOriginal._email != obj._email)
                {
                    if (ch.validar(obj._email).Count > 0)
                    {
                        TempData["Error"] = "Já existe um colaborador com este email registado";
                        return RedirectToAction("Listar", "Colaborador");
                    }
                    
                }
                result = ch.editar(obj);
                TempData["Success"] = "Colaborador " + obj._nome + " atualizado com sucesso!";
                if (result) return RedirectToAction("Listar", "Colaborador");
            }
            return RedirectToAction("Colaborador", "Erro");
        }
        #endregion
        #region Inativar
        public IActionResult Inativar(string id)
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                ColaboradorModelUpdateEstado cmue = new ColaboradorModelUpdateEstado();
                cmue._uidColaborador = Guid.Parse(id);
                cmue._estado = ColaboradorModelUpdateEstado.Estado.INATIVO;
                ch.atualizarEstado(cmue);
                return RedirectToAction("Listar", "Colaborador");
            }
            return RedirectToAction("Login", "Colaborador");
            
        }
        #endregion
        #region Ativar
        public IActionResult Ativar(string id)
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                ColaboradorModelUpdateEstado cmue = new ColaboradorModelUpdateEstado();
                cmue._uidColaborador = Guid.Parse(id);
                cmue._estado = ColaboradorModelUpdateEstado.Estado.ATIVO;
                ch.atualizarEstado(cmue);
                return RedirectToAction("Listar", "Colaborador");
            }
            return RedirectToAction("Login", "Colaborador");

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