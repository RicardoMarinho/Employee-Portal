using System;
namespace ProjetoTI3.Models
{
    public class ColaboradorModelUpdateEstado
    {
        #region Atributos
        public enum Estado { INATIVO, ATIVO };
        #endregion
        #region Propriedades
        public Guid _uidColaborador { get; set; }
        public Estado _estado { get; set; }
        #endregion
    }
}
