using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoTI3.DomainModels;
using System.Data;

namespace ProjetoTI3.Models
{
    public class TipoRelatorio_Helper
    {
        #region Atributos
        private string _ligacao = "";
        #endregion
        #region Construtor
        public TipoRelatorio_Helper(string Conexao) => _ligacao = Conexao;
        #endregion
        #region Listar
        public List<TipoRelatorioModelRead> listar()
        {
            List<TipoRelatorioModelRead> trList = new List<TipoRelatorioModelRead>();
            TipoRelatorio tr = new TipoRelatorio(_ligacao);
            DataTable trTable = new DataTable();
            trTable = tr.listar();
            foreach (DataRow item in trTable.Rows)
            {
                TipoRelatorioModelRead trmr = new TipoRelatorioModelRead();
                trmr._uidTipoRelatorio = Guid.Parse("" + item["uidTipoRelatorio"]);
                trmr._designacao = "" + item["designacao"];
                trList.Add(trmr);
            }
            return trList;
        }
        #endregion
        #region Criar
        public Boolean criar(TipoRelatorioModelCreate obj)
        {
            Boolean result = false;
            TipoRelatorio tr = new TipoRelatorio(_ligacao);
            tr._designacao = obj._designacao;
            result = tr.criar();

            return result;
        }
        #endregion
    }
}
