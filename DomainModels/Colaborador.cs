using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoTI3.DomainModels
{
    public class Colaborador
    {
        #region Atributos
        public enum Estado { 
            INATIVO = 0, 
            ATIVO = 1 
        };
        private string _ligacao;
        #endregion
        #region Propriedades
        public Guid _uidColaborador { get; set; }
        public Guid _departamento { get; set; }
        public Guid _perfil { get; set; }
        public string _nome { get; set; }
        public string _email { get; set; }
        public string _password { get; set; }
        public DateTime _dtNascimento { get; set; }
        public DateTime _dtRegisto { get; set; }
        public Estado _estado { get; set; }

        #endregion
        #region Métodos
        private void setSessionAsGuest()
        {
            this._uidColaborador = Guid.Parse(Program._uidDefault);
            this._nome = "";
            this._email = "";
            this._password = "";
            this._perfil = Guid.Parse(Program._uidDefault);
        }
        #endregion
        #region Construtor
        public Colaborador(string Conexao)
        {
            _ligacao = Conexao;
            _uidColaborador = Guid.NewGuid();
            _perfil = Guid.Parse(Program._uidDefault);
            _departamento = Guid.Parse(Program._uidDefault);
            _password = Program._passwordDefault;
            _email = "";
            _dtNascimento = Convert.ToDateTime("1900/01/01");
            _dtRegisto = DateTime.Now;
            _estado = Estado.INATIVO;
        }
        public Colaborador()
        {
            setSessionAsGuest();
        }
        #endregion
        #region Criar
        public Boolean criar()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Criar";
            comando.Parameters.AddWithValue("@uidColaborador", _uidColaborador);
            comando.Parameters.AddWithValue("@perfil", _perfil);
            comando.Parameters.AddWithValue("@departamento", _departamento);
            comando.Parameters.AddWithValue("@nome", _nome);
            comando.Parameters.AddWithValue("@email", _email);
            comando.Parameters.AddWithValue("@password", ComputeStringToSha256Hash(_password, _email));
            comando.Parameters.AddWithValue("@dtNascimento", _dtNascimento);
            comando.Parameters.AddWithValue("@estado", (int)_estado);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Ler
        public Colaborador ler(Guid guid)
        {
            DataTable colTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Ler";
            comando.Parameters.AddWithValue("@uidColaborador", guid);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(colTable);
            con.Close();
            con.Dispose();
            if(colTable.Rows.Count == 1)
            {
                DataRow value = colTable.Rows[0];
                _uidColaborador = Guid.Parse("" + value["uidColaborador"]);
                _perfil = Guid.Parse("" + value["uidfkPerfil"]);
                _departamento = Guid.Parse("" + value["uidfkDepartamento"]);
                _nome = "" + value["nome"];
                _email = "" + value["email"];
                _dtNascimento = Convert.ToDateTime(value["dtNascimento"]);
                _dtRegisto = Convert.ToDateTime(value["dtRegisto"]);
                _estado = (Estado)Convert.ToInt32(value["estado"]);
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
            comando.CommandText = "QColaborador_Editar";
            comando.Parameters.AddWithValue("@uidColaborador", _uidColaborador);
            comando.Parameters.AddWithValue("@perfil", _perfil);
            comando.Parameters.AddWithValue("@departamento", _departamento);
            comando.Parameters.AddWithValue("@nome", _nome);
            comando.Parameters.AddWithValue("@email", _email);
            comando.Parameters.AddWithValue("@estado", (int)_estado);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Atualizar Estado
        public Boolean atualizarEstado()
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Atualizar_Estado";
            comando.Parameters.AddWithValue("@uidColaborador", _uidColaborador);
            comando.Parameters.AddWithValue("@estado", (int)_estado);
            con.Open();
            comando.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return true;
        }
        #endregion
        #region Listar
        public DataTable listar(Estado ativos)
        {
            DataTable colTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Listar";
            comando.Parameters.AddWithValue("@estado", (int)ativos);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(colTable);
            con.Close();
            con.Dispose();

            return colTable;
        }
        #endregion
        #region Validar Duplicação Email
        public DataTable validar(string email)
        {
            DataTable colTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Validar";
            comando.Parameters.AddWithValue("@email", email);
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(colTable);
            con.Close();
            con.Dispose();

            return colTable;
        }
        #endregion
        #region Unset Session & Autenticação
        public Colaborador autenticar(string email, string senha)
        {
            DataTable colTable = new DataTable();
            SqlDataAdapter blocoNotas = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection con = new SqlConnection(_ligacao);
            comando.Connection = con;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "QColaborador_Autenticar";
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@senha", ComputeStringToSha256Hash(senha, email));
            blocoNotas.SelectCommand = comando;
            blocoNotas.Fill(colTable);
            con.Close();
            con.Dispose();
            if (colTable.Rows.Count == 1)
            {
                DataRow value = colTable.Rows[0];
                _uidColaborador = Guid.Parse("" + value["uidColaborador"]);
                _perfil = Guid.Parse("" + value["uidfkPerfil"]);
                _departamento = Guid.Parse("" + value["uidfkDepartamento"]);
                _nome = "" + value["nome"];
                _email = "" + value["email"];
            }
            return this;
        }
        public Colaborador sair(string uid)
        {           
            Colaborador ca = new Colaborador();
            return ca;
        }
        #endregion
        #region Encriptação
        public static string ComputeStringToSha256Hash(string senha, string email)
        {
            string password = senha + email;
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
        #endregion

    }
}
