using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTI3.Models
{
    public class RelatorioModelRead
    {
        #region Propriedades
        public Guid _uidRelatorio { get; set; }
        public Guid _uidfkTipoRelatorio { get; set; }
        public Guid _uidfkColaborador { get; set; }
        public DateTime _data { get; set; }
        public TimeSpan _horaInicial { get; set; }
        public TimeSpan _horaFinal { get; set; }
        public TimeSpan _horaPausaInicial { get; set; }
        public TimeSpan _horaPausaFinal { get; set; }
        public string _tarefa { get; set; }
        #endregion
    }
}
