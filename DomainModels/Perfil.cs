using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace ProjetoTI3.DomainModels
{
    public class Perfil
    {
        #region Atributos
        private string _ligacao;
        #endregion
        #region Propriedades
        public Guid _uidPerfil { get; set; }
        public string _perfil { get; set; }
        public DateTime _dtRegisto { get; set; }
        #endregion
        #region Construtor
        public Perfil(string Conexao)
        {
            _ligacao = Conexao;
            _uidPerfil = Guid.NewGuid();
            _perfil = "";
            _dtRegisto = DateTime.Now;
        }
        #endregion
        #region Listar
        public DataTable listar()
        {
            DataTable pfTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QPerfil_Listar";
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(pfTable);
            con.Close();
            con.Dispose();

            return pfTable;
        }
        #endregion
        #region Criar
        public Boolean criar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QPerfil_Criar";
            comando.Parameters.AddWithValue("@uidPerfil", _uidPerfil);
            comando.Parameters.AddWithValue("@perfil", _perfil);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion  
        #region Ler
        public Perfil ler(Guid guid)
        {
            DataTable pfTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QPerfil_Ler";
            comando.Parameters.AddWithValue("@uidPerfil", guid);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(pfTable);
            con.Close();
            con.Dispose();
            if (pfTable.Rows.Count == 1)
            {
                DataRow value = pfTable.Rows[0];
                _uidPerfil = Guid.Parse(value["uidPerfil"].ToString());
                _perfil = value["perfil"].ToString();
            }
            return this;
        }
        #endregion
    }
}
