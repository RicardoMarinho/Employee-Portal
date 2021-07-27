using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoTI3.DomainModels;
using System.Data;

namespace ProjetoTI3.Models
{
    public class Perfil_Helper
    {
        #region Atributos
        private string _ligacao = "";
        #endregion
        #region Construtor
        public Perfil_Helper(string Conexao) => _ligacao = Conexao;
        #endregion
        #region Listar
        public List<PerfilModelRead> listar()
        {
            List<PerfilModelRead> pfList = new List<PerfilModelRead>();
            Perfil pf = new Perfil(_ligacao);
            DataTable pfTable = new DataTable();
            pfTable = pf.listar();
            foreach (DataRow item in pfTable.Rows)
            {
                PerfilModelRead pmr = new PerfilModelRead();
                pmr._uidPerfil = Guid.Parse("" + item["uidPerfil"]);
                pmr._perfil = "" + item["perfil"];
                pfList.Add(pmr);
            }
            return pfList;
        }
        #endregion
        #region Criar
        public Boolean criar(PerfilModelCreate obj)
        {
            Boolean result = false;
            Perfil pf = new Perfil(_ligacao);
            pf._perfil = obj._perfil;
            result = pf.criar();

            return result;
        }
        #endregion
        #region Contar
        public int contar()
        {
            Perfil pf = new Perfil(_ligacao);
            int nPerfil;
            nPerfil = (int)pf.listar().Rows.Count;
            return nPerfil;
        }

        #endregion
        #region Ler
        public PerfilModelEdit ler(string id)
        {
            Guid idPerfil;
            Boolean guidOK = false;
            PerfilModelEdit pme;
            Perfil pf = new Perfil(_ligacao);
            try
            {
                idPerfil = Guid.Parse(id);
                guidOK = true;
            }
            catch { }
            if (guidOK)
            {
                pf = pf.ler(Guid.Parse(id));
            }
            if (pf._perfil == "" || guidOK == false) pme = null;
            else
            {
                pme = new PerfilModelEdit();
                pme._uidPerfil = pf._uidPerfil;
                pme._perfil = pf._perfil;
            }
            return pme;
        }
        #endregion
    }
}
