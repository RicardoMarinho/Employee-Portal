using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTI3.Models
{
    public class ColaboraborModelCreate
    {
        #region Propriedades
        public Guid _departamento { get; set; }
        public Guid _perfil { get; set; }
        public string _nome { get; set; }
        public string _email { get; set; }
        public DateTime _dtNascimento { get; set; }

        #endregion
    }
}
