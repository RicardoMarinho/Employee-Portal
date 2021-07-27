using System;
using System.Collections.Generic;
using ProjetoTI3.DomainModels;
using System.Data;

namespace ProjetoTI3.Models
{
    public class Relatorio_Helper
    {
        #region Atributos
        private string _ligacao = "";
        #endregion
        #region Construtor
        public Relatorio_Helper(string Conexao)
        {
            _ligacao = Conexao;
        }
        #endregion
        #region Ler
        public RelatorioModelEdit ler(string id)
        {
            Guid idRelatorio;
            Boolean guidOK = false;
            RelatorioModelEdit rme;
            Relatorio rel = new Relatorio(_ligacao);
            try
            {
                idRelatorio = Guid.Parse(id);
                guidOK = true;
            }
            catch { }
            if (guidOK)
            {
                rel = rel.ler(Guid.Parse(id));
            }
            if (rel._tarefa == "" || guidOK == false) rme = null;
            else
            {
                rme = new RelatorioModelEdit();
                rme._uidfkColaborador = rel._uidfkColaborador;
                rme._uidfkTipoRelatorio = rel._uidfkTipoRelatorio;
                rme._uidRelatorio = rel._uidRelatorio;
                rme._tarefa = rel._tarefa;
                rme._data = rel._data;
                rme._horaFinal = rel._horaFinal;
                rme._horaInicial = rel._horaInicial;
                rme._horaPausaInicial = rel._horaPausaInicial;
                rme._horaPausaFinal = rel._horaPausaFinal;
            }
            return rme;
        }
        #endregion
        #region Listar
        public List<RelatorioModelRead> listar(string uid)
        {
            List<RelatorioModelRead> relList = new List<RelatorioModelRead>();
            Relatorio rel = new Relatorio(_ligacao);
            DataTable relTable = new DataTable();
            relTable = rel.listar(uid);
            foreach (DataRow value in relTable.Rows)
            {
                RelatorioModelRead rmr = new RelatorioModelRead();
                rmr._uidfkColaborador = Guid.Parse("" + value["uidfkColaborador"]);
                rmr._uidfkTipoRelatorio = Guid.Parse("" + value["uidfkTipoRelatorio"]);
                rmr._uidRelatorio = Guid.Parse("" + value["uidRelatorio"]);
                rmr._data = Convert.ToDateTime(value["data"]);
                rmr._horaFinal = TimeSpan.Parse("" + value["horaFinal"]);
                rmr._horaInicial = TimeSpan.Parse("" + value["horaInicial"]);
                rmr._horaPausaFinal = TimeSpan.Parse("" + value["horaPausaFinal"]);
                rmr._horaPausaInicial = TimeSpan.Parse("" + value["horaPausaInicial"]);
                rmr._tarefa = "" + value["tarefa"];
                relList.Add(rmr);
            }
            return relList;
        }
        #endregion
        #region Criar
        public Boolean criar(RelatorioModelCreate obj)
        {
            Boolean result = false;
            Relatorio rel = new Relatorio(_ligacao);
            rel._uidfkTipoRelatorio = obj._uidfkTipoRelatorio;
            rel._uidfkColaborador = obj._uidfkColaborador;
            rel._tarefa = obj._tarefa;
            rel._data = obj._data;
            rel._horaInicial = obj._horaInicial;
            rel._horaFinal = obj._horaFinal;
            rel._horaPausaInicial = obj._horaPausaInicial;
            rel._horaPausaFinal = obj._horaPausaFinal;
            result = rel.criar();

            return result;
        }
        #endregion
        #region Editar
        public Boolean editar(RelatorioModelEdit obj)
        {
            Boolean result = false;
            Relatorio rel = new Relatorio(_ligacao);
            rel.ler(obj._uidRelatorio);
            if(rel._tarefa != "")
            {
                rel._uidfkColaborador = obj._uidfkColaborador;
                rel._uidRelatorio = obj._uidRelatorio;
                rel._uidfkTipoRelatorio = obj._uidfkTipoRelatorio;
                rel._tarefa = obj._tarefa;
                rel._data = obj._data;
                rel._horaInicial = obj._horaInicial;
                rel._horaFinal = obj._horaFinal;
                rel._horaPausaInicial = obj._horaPausaInicial;
                rel._horaPausaFinal = obj._horaPausaFinal;
                result = rel.editar();
            }
            return result;
        }
        #endregion
        
    }
}
