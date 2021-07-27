using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoTI3.DomainModels;
using System.Data;

namespace ProjetoTI3.Models
{
    public class Departamento_Helper
    {
        #region Atributos
        private string _ligacao = "";
        #endregion
        #region Construtor
        public Departamento_Helper(string Conexao) => _ligacao = Conexao;
        #endregion
        #region Listar
        public List<DepartamentoModelRead> listar()
        {
            List<DepartamentoModelRead> dptList = new List<DepartamentoModelRead>();
            Departamento dpt = new Departamento(_ligacao);
            DataTable dptTable = new DataTable();
            dptTable = dpt.listar();
            foreach (DataRow item in dptTable.Rows)
            {
                DepartamentoModelRead dmr = new DepartamentoModelRead();
                dmr._uidDepartamento = Guid.Parse("" + item["uidDepartamento"]);
                dmr._departamento = "" + item["departamento"];
                dptList.Add(dmr);
            }
            return dptList;
        }
        #endregion
        #region Criar
        public Boolean criar(DepartamentoModelCreate obj)
        {
            Boolean result = false;
            Departamento dpt = new Departamento(_ligacao);
            dpt._departamento = obj._departamento;
            result = dpt.criar();

            return result;
        }
        #endregion
        #region Contar
        public int contar()
        {
            Departamento dpt = new Departamento(_ligacao);
            int nDpt;
            nDpt = (int)dpt.listar().Rows.Count;
            return nDpt;
        }
        #endregion
        #region Ler
        public DepartamentoModelEdit ler(string id)
        {
            Guid idPDepartamento;
            Boolean guidOK = false;
            DepartamentoModelEdit dme;
            Departamento dpt = new Departamento(_ligacao);
            try
            {
                idPDepartamento = Guid.Parse(id);
                guidOK = true;
            }
            catch { }
            if (guidOK)
            {
                dpt = dpt.ler(Guid.Parse(id));
            }
            if (dpt._departamento == "" || guidOK == false) dme = null;
            else
            {
                dme = new DepartamentoModelEdit();
                dme._uidDepartamento = dpt._uidDepartamento;
                dme._departamento = dpt._departamento;
            }
            return dme;
        }
        #endregion
    }
}
