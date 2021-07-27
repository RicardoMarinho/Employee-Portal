using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoTI3.DomainModels
{
    public class TipoRelatorio
    {
        #region Atributos
        private string _ligacao;
        #endregion
        #region Propriedades
        public Guid _uidTipoRelatorio { get; set; }
        public string _designacao { get; set; }
        public DateTime _dtRegisto { get; set; }
        #endregion
        #region Construtor
        public TipoRelatorio(string Conexao)
        {
            _ligacao = Conexao;
            _uidTipoRelatorio = Guid.NewGuid();
            _designacao = "";
            _dtRegisto = DateTime.Now;
        }
        #endregion
        #region Listar
        public DataTable listar()
        {
            DataTable trTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QTipoRelatorio_Listar";
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(trTable);
            con.Close();
            con.Dispose();

            return trTable;
        }
        #endregion
        #region Criar
        public Boolean criar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QTipoRelatorio_Criar";
            comando.Parameters.AddWithValue("@uidTipoRelatorio", _uidTipoRelatorio);
            comando.Parameters.AddWithValue("@designacao", _designacao);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion  
    }
}
