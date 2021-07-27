using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoTI3.Controllers
{
    public class ErroController : Controller
    {
        #region Ações
        public IActionResult Departamento()
        {
            return View();
        }
        public IActionResult Perfil()
        {
            return View();
        }
        public IActionResult Colaborador()
        {
            return View();
        }
        #endregion
    }
}