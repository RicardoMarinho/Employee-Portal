using System;

namespace ProjetoTI3.Models
{
    public class ColaboradorModelRead
    {
        #region Atributos
        public enum Estado { 
            INATIVO = 0, 
            ATIVO = 1 };
        #endregion
        #region Propriedades
        public Guid _uidColaborador { get; set; }
        public Guid _departamento { get; set; }
        public Guid _perfil { get; set; }
        public string _nome { get; set; }
        public string _email { get; set; }
        public Estado _estado { get; set; }
        public DateTime _dtNascimento { get; set; }
        #endregion
    }
}
