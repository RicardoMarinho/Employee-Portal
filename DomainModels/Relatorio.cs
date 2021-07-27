using System;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoTI3.DomainModels
{
    public class Relatorio
    {
        #region Atributos
        private string _ligacao;
        #endregion
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
        public DateTime _dtRegisto { get; set; }
        #endregion
        #region Construtor
        public Relatorio(string Conexao)
        {
            _ligacao = Conexao;
            _uidfkColaborador = Guid.Parse(Program._uidDefault);
            _uidfkTipoRelatorio = Guid.Parse(Program._uidDefault);
            _uidRelatorio = Guid.NewGuid();
            _tarefa = "";
            _data = Convert.ToDateTime("1900/01/01");
            _horaFinal = new TimeSpan(9, 0, 0);
            _horaInicial = new TimeSpan(18, 0, 0);
            _horaPausaFinal = new TimeSpan(13, 0, 0);
            _horaPausaInicial = new TimeSpan(14, 0, 0);
            _dtRegisto = DateTime.Now;
        }
        #endregion
        #region Criar
        public Boolean criar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QRelatorio_Criar";
            comando.Parameters.AddWithValue("@uidRelatorio", _uidRelatorio);
            comando.Parameters.AddWithValue("@uidfkTipoRelatorio", _uidfkTipoRelatorio);
            comando.Parameters.AddWithValue("@uidfkColaborador", _uidfkColaborador);
            comando.Parameters.AddWithValue("@tarefa", _tarefa);
            comando.Parameters.AddWithValue("@data", _data);
            comando.Parameters.AddWithValue("@horaInicial", _horaInicial);
            comando.Parameters.AddWithValue("@horaFinal", _horaFinal);
            comando.Parameters.AddWithValue("@horaPausaInicial", _horaPausaInicial);
            comando.Parameters.AddWithValue("@horaPausaFinal", _horaPausaFinal);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Ler
        public Relatorio ler(Guid guid)
        {
            DataTable relTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QRelatorio_Ler";
            comando.Parameters.AddWithValue("@uidRelatorio", guid);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(relTable);
            con.Close();
            con.Dispose();
            if(relTable.Rows.Count == 1)
            {
                DataRow value = relTable.Rows[0];
                _uidRelatorio = Guid.Parse("" + value["uidRelatorio"]);
                _uidfkColaborador = Guid.Parse("" + value["uidfkColaborador"]);
                _uidfkTipoRelatorio = Guid.Parse("" + value["uidfkTipoRelatorio"]);
                _tarefa = "" + value["tarefa"];
                _data = Convert.ToDateTime(value["data"]);
                _horaFinal = TimeSpan.Parse("" + value["horaFinal"]);
                _horaPausaFinal = TimeSpan.Parse("" + value["horaPausaFinal"]);
                _horaInicial = TimeSpan.Parse("" + value["horaInicial"]);
                _horaPausaInicial = TimeSpan.Parse("" + value["horaPausaInicial"]);
                _dtRegisto = Convert.ToDateTime(value["dtRegisto"]);
            }
            return this;
        }
        #endregion
        #region Editar
        public Boolean editar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QRelatorio_Editar";
            comando.Parameters.AddWithValue("@uidRelatorio", _uidRelatorio);
            comando.Parameters.AddWithValue("@uidfkColaborador", _uidfkColaborador);
            comando.Parameters.AddWithValue("@uidfkTipoRelatorio", _uidfkTipoRelatorio);
            comando.Parameters.AddWithValue("@tarefa", _tarefa);
            comando.Parameters.AddWithValue("@data", _data);
            comando.Parameters.AddWithValue("@horaInicial", _horaInicial);
            comando.Parameters.AddWithValue("@horaFinal", _horaFinal);
            comando.Parameters.AddWithValue("@horaPausaInicial", _horaPausaInicial);
            comando.Parameters.AddWithValue("@horaPausaFinal", _horaPausaFinal);
            comando.Parameters.AddWithValue("@dtRegisto", _dtRegisto);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Listar
        public DataTable listar(string uid)
        {
            Guid guid = Guid.Parse(uid);
            DataTable relTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QRelatorio_Listar";
            comando.Parameters.AddWithValue("@uidfkColaborador", guid);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(relTable);
            con.Close();
            con.Dispose();

            return relTable;
        }
        #endregion

    }
}
