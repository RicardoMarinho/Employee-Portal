using Microsoft.AspNetCore.Mvc;
using ProjetoTI3.Models;
using ProjetoTI3.DomainModels;
using Microsoft.AspNetCore.Http;

namespace ProjetoTI3.Controllers
{
    public class HomeController : Controller
    {
        private Colaborador conta;
        #region View Index
        public IActionResult Index()
        {
            setUserSession();
            if (ViewBag.Conta._uidColaborador.ToString() != Program._uidDefault)
            {
                Perfil_Helper ph = new Perfil_Helper(Program._conexao);
                Departamento_Helper dh = new Departamento_Helper(Program._conexao);
                Colaborador_Helper ch = new Colaborador_Helper(Program._conexao);
                //ViewBag.Colaborador = ch.ler(ViewBag.Conta._uidColaborador);
                ViewBag.nCol = ch.contar(Colaborador.Estado.ATIVO);
                ViewBag.nDpt = dh.contar();
                ViewBag.nPerfil = ph.contar();
                ViewBag.Current = "Index";

                return View();
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