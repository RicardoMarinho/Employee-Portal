using System;
namespace ProjetoTI3.Models
{
    public class ColaboradorModelEdit
    {
        #region Atributos
        public enum Estado { INATIVO, ATIVO };
        #endregion
        #region Propriedades
        public Guid _uidColaborador { get; set; }
        public Guid _departamento { get; set; }
        public Guid _perfil { get; set; }
        public string _nome { get; set; }
        public string _email { get; set; }
        public DateTime _dtNascimento { get; set; }
        public Estado _estado { get; set; }
        #endregion
    }
}
