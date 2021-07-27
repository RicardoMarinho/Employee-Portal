using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoTI3.DomainModels
{
    public class Departamento
    {
        #region Atributos
        private string _ligacao;
        #endregion
        #region Propriedades
        public Guid _uidDepartamento { get; set; }
        public string _departamento { get; set; }
        public DateTime _dtRegisto { get; set; }
        #endregion
        #region Construtor
        public Departamento(string Conexao)
        {
            _ligacao = Conexao;
            _uidDepartamento = Guid.NewGuid();
            _departamento = "";
            _dtRegisto = DateTime.Now;
        }
        #endregion
        #region Listar
        public DataTable listar()
        {
            DataTable dpt = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QDepartamento_Listar";
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(dpt);
            con.Close();
            con.Dispose();

            return dpt;
        }
        #endregion
        #region Criar
        public Boolean criar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QDepartamento_Criar";
            comando.Parameters.AddWithValue("@uidDepartamento", _uidDepartamento);
            comando.Parameters.AddWithValue("@departamento", _departamento);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Ler
        public Departamento ler(Guid guid)
        {
            DataTable dptTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QDepartamento_Ler";
            comando.Parameters.AddWithValue("@uidDepartamento", guid);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(dptTable);
            con.Close();
            con.Dispose();
            if (dptTable.Rows.Count == 1)
            {
                DataRow value = dptTable.Rows[0];
                _uidDepartamento = Guid.Parse(value["uidDepartamento"].ToString());
                _departamento = value["departamento"].ToString();
            }
            return this;
        }
        #endregion
    }


}
