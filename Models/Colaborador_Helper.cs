using System;
using System.Collections.Generic;
using ProjetoTI3.DomainModels;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoTI3.Models
{
    public class Colaborador_Helper
    {
        #region Atributos
        private string _ligacao = "";
        #endregion
        #region Construtor
        public Colaborador_Helper(string Conexao)
        {
            _ligacao = Conexao;
        }
        #endregion
        #region Ler
        public ColaboradorModelEdit ler(string id)
        {
            Guid idColaborador;
            Boolean guidOK = false;
            ColaboradorModelEdit cme;
            Colaborador col = new Colaborador(_ligacao);
            try
            {
                idColaborador = Guid.Parse(id);
                guidOK = true;
            }
            catch { }
            if (guidOK)
            {
                col = col.ler(Guid.Parse(id));
            }
            if (col._nome == "" || guidOK == false) cme = null;
            else
            {
                cme = new ColaboradorModelEdit();
                cme._uidColaborador = col._uidColaborador;
                cme._perfil = col._perfil;
                cme._departamento = col._departamento;
                cme._nome = col._nome;
                cme._email = col._email;
                cme._dtNascimento = col._dtNascimento;
                cme._estado = (ColaboradorModelEdit.Estado)col._estado;
            }
            return cme;
        }
        public ColaboradorModelUpdateEstado lerEstado(string id)
        {
            Guid idColaborador;
            Boolean guidOK = false;
            ColaboradorModelUpdateEstado cme;
            Colaborador col = new Colaborador(_ligacao);
            try
            {
                idColaborador = Guid.Parse(id);
                guidOK = true;
            }
            catch { }
            if (guidOK)
            {
                col = col.ler(Guid.Parse(id));
            }
            if (col._nome == "" || guidOK == false) cme = null;
            else
            {
                cme = new ColaboradorModelUpdateEstado();
                cme._uidColaborador = col._uidColaborador;
                cme._estado = (ColaboradorModelUpdateEstado.Estado)col._estado;
            }
            return cme;
        }
        #endregion
        #region Listar
        public List<ColaboradorModelRead> listar(Colaborador.Estado estado)
        {
            List<ColaboradorModelRead> colList = new List<ColaboradorModelRead>();
            Colaborador col = new Colaborador(_ligacao);
            DataTable colTable = new DataTable();
            colTable = col.listar(estado);
            foreach (DataRow value in colTable.Rows)
            {
                ColaboradorModelRead cmr = new ColaboradorModelRead();
                cmr._uidColaborador = Guid.Parse("" + value["uidColaborador"]);
                cmr._perfil = Guid.Parse("" + value["uidfkPerfil"]);
                cmr._departamento = Guid.Parse("" + value["uidfkDepartamento"]);
                cmr._nome = "" + value["nome"];
                cmr._email = "" + value["email"];
                cmr._estado = (ColaboradorModelRead.Estado)Convert.ToInt32(value["estado"]);
                colList.Add(cmr);
            }
            return colList;
        }
        #endregion
        #region Criar
        public Boolean criar(ColaboraborModelCreate obj)
        {
            Boolean result = false;
            Colaborador col = new Colaborador(_ligacao);
            col._nome = obj._nome;
            col._email = obj._email;
            col._estado = Colaborador.Estado.ATIVO;
            col._password = Program._passwordDefault;
            col._perfil = obj._perfil;
            col._departamento = obj._departamento;
            col._dtNascimento = obj._dtNascimento;
            result = col.criar();

            return result;
        }
        #endregion
        #region Editar
        public Boolean editar(ColaboradorModelEdit obj)
        {
            Boolean result = false;
            Colaborador col = new Colaborador(_ligacao);
            col.ler(obj._uidColaborador);
            if(col._nome != "")
            {
                col._uidColaborador = obj._uidColaborador;
                col._nome = obj._nome;
                col._perfil = obj._perfil;
                col._departamento = obj._departamento;
                col._dtNascimento = obj._dtNascimento;
                col._email = obj._email;
                col._estado = (Colaborador.Estado)obj._estado;
                result = col.editar();
            }
            return result;
        }
        #endregion
        #region Atualizar Estado
        public Boolean atualizarEstado(ColaboradorModelUpdateEstado obj)
        {
            Boolean result = false;
            Colaborador col = new Colaborador(_ligacao);
            col.ler(obj._uidColaborador);
            if (col._nome != "")
            {
                col._uidColaborador = obj._uidColaborador;
                col._estado = (Colaborador.Estado)obj._estado;
                result = col.atualizarEstado();
            }
            return result;
        }
        #endregion
        #region Validar
        public List<ColaboradorModelRead> validar(string email)
        {
            List<ColaboradorModelRead> colList = new List<ColaboradorModelRead>();
            Colaborador col = new Colaborador(_ligacao);
            DataTable colTable = new DataTable();
            colTable = col.validar(email);
            foreach (DataRow value in colTable.Rows)
            {
                ColaboradorModelRead cmr = new ColaboradorModelRead();
                cmr._email = "" + value["email"];
                colList.Add(cmr);
            }
            return colList;
        }
        #endregion
        #region Contar
        public int contar(Colaborador.Estado estado)
        {
            Colaborador col = new Colaborador(_ligacao);
            int nCol;
            nCol = (int)col.listar(estado).Rows.Count;
            return nCol;
        }
        #endregion
        #region Get Conta Ativa
        public Colaborador getConta(string id)
        {
            Colaborador c = new Colaborador();       //Ok, já é Guest
            if (id == null) return c;               //Sai por aqui se Guest
            //Senão continua para validação do utilizador e leitura de perfil
            Guid uidAValidar;
            Boolean guidValidado = false;
            try
            {
                uidAValidar = Guid.Parse(id);
                guidValidado = true;
            }
            catch
            { }
            if (guidValidado)
            {
                DataTable colTable = new DataTable();
                SqlDataAdapter comando = new SqlDataAdapter();
                comando.SelectCommand = new SqlCommand();
                comando.SelectCommand.Connection = new SqlConnection(_ligacao);
                comando.SelectCommand.CommandType = CommandType.StoredProcedure;
                comando.SelectCommand.CommandText = "QColaborador_Ler";
                comando.SelectCommand.Parameters.AddWithValue("@uidColaborador", Guid.Parse(id));
                comando.Fill(colTable);
                comando.SelectCommand.Connection.Close();
                comando.SelectCommand.Connection.Dispose();
                if (colTable.Rows.Count == 1)
                {
                    DataRow dr = colTable.Rows[0];
                    c._uidColaborador = Guid.Parse(dr["uidColaborador"].ToString());
                    c._nome = dr["nome"].ToString();
                    c._email = dr["email"].ToString();
                    c._perfil = Guid.Parse(dr["uidfkPerfil"].ToString());
                    c._departamento = Guid.Parse(dr["uidfkDepartamento"].ToString());
                }
            }
            //Neste ponto, o ca pode ser um perfil ok (se uid for valido) ou ser guest
            return c;
        }
        #endregion
        #region Autenticar e Sair
        public Colaborador autenticar(string email, string senha)
        {
            Colaborador c = new Colaborador();
            DataTable colTable = new DataTable();
            SqlDataAdapter comando = new SqlDataAdapter();
            comando.SelectCommand = new SqlCommand();
            comando.SelectCommand.Connection = new SqlConnection(_ligacao);
            comando.SelectCommand.CommandType = CommandType.StoredProcedure;
            comando.SelectCommand.CommandText = "QColaborador_Autenticar";
            comando.SelectCommand.Parameters.AddWithValue("@email", email);
            comando.SelectCommand.Parameters.AddWithValue("@senha", ComputeStringToSha256Hash(senha, email));
            comando.Fill(colTable);
            comando.SelectCommand.Connection.Close();
            comando.SelectCommand.Connection.Dispose();
            if (colTable.Rows.Count == 1)
            {
                DataRow dr = colTable.Rows[0];
                c._uidColaborador = Guid.Parse(dr["uidColaborador"].ToString());
                c._nome = dr["nome"].ToString();
                c._email = dr["email"].ToString();
                c._perfil = Guid.Parse(dr["uidfkPerfil"].ToString());
                c._departamento = Guid.Parse(dr["uidfkDepartamento"].ToString());
            }
            return c;
        }
        public Colaborador sair()
        {         
            Colaborador c = new Colaborador();
            return c;
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
