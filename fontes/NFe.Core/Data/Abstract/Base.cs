using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ERP.Net.Data.Cadastro.Pessoa;
using ERP.Net.Model;
using Newtonsoft.Json;
using Unimake;
using Unimake.Data.Generic;
using Unimake.Data.Generic.Definitions;
using Unimake.Data.Generic.Definitions.Attributes;
using Unimake.Data.Generic.Schema;
using Unimake.Data.Generic.Model;

namespace ERP.Net.Data.Abstract
{
    /// <summary>
    /// Classe de base para acesso à base de dados
    /// </summary>
    [DBClassDefinitionAttribute(GenericDbType.Integer64)]
    public abstract class Base: Unimake.Data.Generic.Data.Abstract.TableDataBase,
        ITableBaseModel
    {
        #region Private Fields

        private DateTime _dataHoraAlteracao;

        private DateTime _dataHoraCadastro;

        private GUID _empresaID;

        private GUID _usuarioAlteracaoID;

        private GUID _usuarioCadastroID;

        private long guid = 0;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Data e hora que o cadastro do registro foi alterado.
        /// </summary>
        public DateTime DataHoraAlteracao
        {
            get
            {
                if(!_dataHoraAlteracao.IsValid())
                    _dataHoraAlteracao = DateTime.Now;

                return _dataHoraAlteracao;
            }
            set { _dataHoraAlteracao = value; }
        }

        /// <summary>
        /// Data e hora que o cadastro do registro foi efetuado.
        /// </summary>
        public virtual DateTime DataHoraCadastro
        {
            get
            {
                if(!_dataHoraCadastro.IsValid())
                    _dataHoraCadastro = DateTime.Now;

                return _dataHoraCadastro;
            }
            set { _dataHoraCadastro = value; }
        }

        /// <summary>
        ///<para> Campo código do sistema. É um código que o usuário dá para o cadastro que está sendo</para>
        ///<para> efetuado. O campo código deve obedecer a uma das 3 regras seguintes:</para>
        ///<para>     1. Livre</para>
        ///<para>         É permitida a duplicação na tabela. </para>
        ///<para>     2. Único na Empresa</para>
        ///<para>         É permitida a duplicação na tabela somente se for empresas diferentes.</para>
        ///<para>     3. Único na Tabela</para>
        ///<para>         Não é permitida a repetição do mesmo dentro da tabela.</para>
        ///<para> </para>
        ///<para> 1-Caso não seja informado o código, o sistema deverá usar o ID, sem formatação, como código, descartando as regras acima.</para>
        ///<para> 2-Este campo só deve existir se a tabela criada tiver necessidade de um código definido pelo usuário.</para>
        ///<para> caso contrário aplica-se a ideia descrita no item anterior.</para>
        ///<para> 3-O conteúdo deste campo, quando existir na tabela, é obrigatório.</para>
        /// </summary>
        public virtual EGUID? EGUID { get; set; }

        /// <summary>
        /// Campo de relacionamento com a empresa, cad_PessoaEmpresa, ou seja, empresa responsável
        /// pelo cadastro do registro. Controle interno, não deve sofrer interferência do usuário.
        /// </summary>
        [FieldDefinition(false)]
        [XmlIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        public Empresa Empresa
        {
            get { return Lazy.Get<Empresa>("Empresa"); }
            set
            {
                _empresaID = value.IsNullOrEmpty() ? 0 : value.GUID;
                Lazy.Set("Empresa", () =>
                {
                    return value;
                });
            }
        }

        /// <summary>
        /// </summary>
        [FieldDefinition("GUIDEmpresa")]
        public GUID EmpresaID
        {
            get { return _empresaID = Empresa.IsNullOrEmpty() ? _empresaID : Empresa.GUID; }
            set
            {
                _empresaID = value;
                Lazy.Set("Empresa", () =>
                {
                    return new Empresa(_empresaID);
                });
            }
        }

        /// <summary>
        /// Chave primária da tabela. É um tipo GUID (long)
        /// </summary>
        [FieldDefinition(true, true)]
        public virtual GUID GUID
        {
            get { return guid; }
            set
            {
                guid = value;
                base.ID = value;
            }
        }

        /// <summary>
        /// Retorna o identificador do registro
        /// </summary>
        [FieldDefinition("GUID", false)]
        public new virtual long ID
        {
            get { return GUID; }
            set { GUID = value; }
        }

        /// <summary>
        /// Retorna true se for um novo registro
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        public override bool New
        {
            get
            {
                return base.New || GUID.IsNullOrEmpty();
            }
            set
            {
                if(value)
                    GUID = GUID.Empty;

                base.New = value;
            }
        }

        /// <summary>
        /// Data e hora da última alteração desta classe
        /// </summary>
        [FieldDefinition(false)]
        public virtual DateTime Timestamp
        {
            get { return DataHoraAlteracao; }
            set { DataHoraAlteracao = value; }
        }

        IID Unimake.Data.Generic.Model.ITableBaseModel.ID
        {
            get { return GUID; }
            set { GUID = new GUID(value); }
        }

        /// <summary>
        /// </summary>
        /// <summary>
        /// Usuário que efetuou o cadastro do registro
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        [FieldDefinition(false)]
        public Usuario UsuarioAlteracao
        {
            get { return Lazy.Get<Usuario>("UsuarioAlteracao") ?? Settings.UsuarioAtual; }
            set
            {
                _usuarioAlteracaoID = value.IsNullOrEmpty() ? 0 : value.GUID;
                Lazy.Set("UsuarioAlteracao", () =>
                {
                    return value;
                });
            }
        }

        /// <summary>
        /// </summary>
        [FieldDefinition("GUIDUsuarioAlteracao")]
        public GUID UsuarioAlteracaoID
        {
            get { return _usuarioAlteracaoID = UsuarioAlteracao.IsNullOrEmpty() ? _usuarioAlteracaoID : UsuarioAlteracao.GUID; }
            set
            {
                _usuarioAlteracaoID = value;
                Lazy.Set("UsuarioAlteracao", () =>
                {
                    return new Usuario(_usuarioAlteracaoID);
                });
            }
        }

        /// <summary>
        /// Usuário que efetuou o cadastro do registro
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        [FieldDefinition(false)]
        public Usuario UsuarioCadastro
        {
            get { return Lazy.Get<Usuario>("UsuarioCadastro") ?? Settings.UsuarioAtual; }
            set
            {
                _usuarioCadastroID = value.IsNullOrEmpty() ? 0 : value.GUID;
                Lazy.Set("UsuarioCadastro", () =>
                {
                    return value;
                });
            }
        }

        /// <summary>
        /// </summary>
        [FieldDefinition("GUIDUsuarioCadastro")]
        public GUID UsuarioCadastroID
        {
            get { return _usuarioCadastroID = UsuarioCadastro.IsNullOrEmpty() ? _usuarioCadastroID : UsuarioCadastro.GUID; }
            set
            {
                _usuarioCadastroID = value;
                Lazy.Set("UsuarioCadastro", () =>
                {
                    return new Usuario(_usuarioCadastroID);
                });
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Cria o objeto com as propriedades padrões
        /// </summary>
        public Base()
        {
            DataHoraCadastro = DateTime.Now;
            DataHoraAlteracao = DateTime.Now;
            EGUID = Net.EGUID.Empty;
            SetStrategies();
        }

        /// <summary>
        /// Cria o objeto com as propriedades padrões
        /// </summary>
        /// <param name="guid">Identificador do registro</param>
        public Base(GUID guid)
                : this()
        {
            DbContext.Populate(this, guid);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compara este objeto com o outro e retorna verdadeiro se ambos forem iguais
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns></returns>
        public virtual bool Equals(Base obj)
        {
            if(obj == null)
                return false;

            return this.GUID == obj.GUID;
        }

        /// <summary>
        /// Compara este objeto com o outro e retorna verdadeiro se ambos forem iguais
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Base);
        }

        /// <summary>
        /// Busca um registro com o GUID informado e retorna se encontrar
        /// </summary>
        /// <typeparam name="T1">Tipo do objeto de retorno</typeparam>
        /// <param name="guid">GUID do registro para pesquisa</param>
        /// <returns></returns>
        public T1 Get<T1>(GUID guid)
            where T1 : IBaseModel, Unimake.Data.Generic.Model.ITableBaseModel, new()
        {
            return (T1)DbContext.Get(guid, this);
        }

        /// <summary>
        /// Prepara os valores e retorna os em um formato de exibição para o usuário
        /// <para>Como padrão retorna os três primeiros campos do select que foi criado</para>
        /// </summary>
        /// <param name="where">Filtro, se necessário. Não é obrigatório e pode ser nulo</param>
        /// <returns>Retorna os valores em um um formato de exibição para o usuário</returns>
        public override IDisplayValues GetDisplayValues(Where where = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um hashcode deste objeto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GUID.GetHashCode();
        }

        /// <summary>
        /// Retorna o tablehash da tabela
        /// </summary>
        /// <returns>string com o tablehash do objeto</returns>
        public override int GetTableHash()
        {
            return Unimake.Convert.ToInt(Unimake.Utilities.GetTableHash(this.GetType()));
        }

        /// <summary>
        /// Executa uma estratégia antes de salvar
        /// </summary>
        /// <param name="executeBefore">Método que será executado antes desta ação acontecer</param>
        /// <param name="updating">Se verdadeiro, está sendo realizado uma atualização</param>
        public override void PerformBeforeSave(Func<bool> executeBefore, bool updating)
        {
            if(!updating)
            {
                if(GUID.IsNullOrEmpty())
                    GUID = GUID.Create();

                UsuarioAlteracao = UsuarioCadastro;
            }
            else
                UsuarioAlteracao = Settings.UsuarioAtual;

            if(EGUID.IsNullOrEmpty())
            {
                if(!(this is Data.Configuracao.NumeroEGUID))
                {
                    EGUID = EGUIDExtensions.Create(EGUID, this);
                }
            }

            if(Empresa.IsNullOrEmpty())
                Empresa = Settings.EmpresaAtual;

            if(UsuarioCadastro.IsNullOrEmpty())
                UsuarioCadastro = Settings.UsuarioAtual;

            base.PerformBeforeSave(executeBefore, updating);
        }

        /// <summary>
        /// Método utilizado para preencher esta instância com os dados do dataReader
        /// </summary>
        /// <param name="dataReader">
        /// DataReader com os dados que deverão ser passados para esta instância
        /// </param>
        public override void Populate(DataReader dataReader)
        {
            base.Populate(dataReader);

            #region Campos de base

            #region GUID

            if(dataReader.GetOrdinal("p_GUID") > -1)
            {
                GUID = dataReader.GetValue<long>("p_GUID");
            }

            #endregion GUID

            #region EGUID

            if(dataReader.GetOrdinal("p_EGUID") > -1)
            {
                EGUID = dataReader.GetValue<string>("p_EGUID");
            }

            #endregion EGUID

            #region DataHoraCadastro

            if(dataReader.GetOrdinal("p_DataHoraCadastro") > -1)
            {
                DataHoraCadastro = dataReader.GetValue<DateTime>("p_DataHoraCadastro");
            }

            #endregion DataHoraCadastro

            #region DataHoraCadastro

            if(dataReader.GetOrdinal("p_DataHoraCadastro") > -1)
            {
                DataHoraCadastro = dataReader.GetValue<DateTime>("p_DataHoraCadastro");
            }

            #endregion DataHoraCadastro

            #region DataHoraAlteracao

            if(dataReader.GetOrdinal("p_DataHoraAlteracao") > -1)
            {
                DataHoraAlteracao = dataReader.GetValue<DateTime>("p_DataHoraAlteracao");
            }

            #endregion DataHoraAlteracao

            #region GUIDUsuarioAlteracao

            if(dataReader.GetOrdinal("p_GUIDUsuarioAlteracao") > -1)
            {
                UsuarioAlteracaoID = dataReader.GetValue<long>("p_GUIDUsuarioAlteracao");
            }

            #endregion GUIDUsuarioAlteracao

            #region GUIDUsuarioCadastro

            if(dataReader.GetOrdinal("p_GUIDUsuarioCadastro") > -1)
            {
                UsuarioCadastroID = dataReader.GetValue<long>("p_GUIDUsuarioCadastro");
            }

            #endregion GUIDUsuarioCadastro

            #region GUIDEmpresa

            if(dataReader.GetOrdinal("p_GUIDEmpresa") > -1)
            {
                EmpresaID = dataReader.GetValue<long>("p_GUIDEmpresa");
            }

            #endregion GUIDEmpresa

            #endregion Campos de base
        }

        /// <summary>
        /// Preenche os dados deste registro com o GUID informado, se existir
        /// </summary>
        /// <param name="guid">GUID de um registro na base de dados</param>
        public void Populate(GUID guid)
        {
            if(guid.IsNullOrEmpty()) return;
            DbContext.Populate(this, guid);
        }

        /// <summary>
        /// Prepara o comando antes de executar um select
        /// </summary>
        /// <param name="command">comando que será preparado antes da execução</param>
        /// <param name="where">Filtro where adicional</param>
        public override void PrepareReader(Command command, Where where)
        {
            base.PrepareReader(command, where);
            string selectFields = "";
            string fieldEmpresa = GetField("GUIDEmpresa");

            if(!String.IsNullOrEmpty(fieldEmpresa))
            {
                selectFields += fieldEmpresa;

                string parameterName = Parameter.FixParameterName(fieldEmpresa, "base");
                where.Add($"{fieldEmpresa.Split(new string[] { " AS " })[0]} = {parameterName}", new Parameter
                {
                    ParameterName = parameterName,
                    Value = Settings.EmpresaAtual.ID
                });
            }

            selectFields += GetField("GUID");
            selectFields += GetField("EGUID");
            selectFields += GetField("DataHoraCadastro");
            selectFields += GetField("DataHoraAlteracao");
            selectFields += GetField("GUIDUsuarioCadastro");
            selectFields += GetField("GUIDUsuarioAlteracao");

            string s = where.FirstOrDefault(w => w.StartsWith("GUID", StringComparison.InvariantCultureIgnoreCase));

            if(s != null)
            {
                string tableName = DbContext.FindTableDefinition(this).TableName;
                where.Remove(s);
                where.Add(tableName + "." + s);
            }

            command.CommandText = command.CommandText.Replace("{selectFields}", selectFields + " {selectFields}");
        }

        /// <summary>
        /// Salva o registro e retorna um GUID do registro salvo
        /// </summary>
        /// <returns>Retorna o identificador da entidade</returns>
        public new virtual GUID Save()
        {
            GUID result = new GUID(DbContext.Save(this));
            return result;
        }

        /// <summary>
        /// Representação em texto deste objeto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(GUID.IsNullOrEmpty()) return null;
            return GUID.ToString();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Ajusta todas as estratégias definidas para esta classe
        /// </summary>
        protected virtual void SetStrategies()
        {
            Strategy.Strategy.SetStrategy(this);
        }

        #endregion Protected Methods

        #region Private Methods

        private string GetField(string fieldName)
        {
            IList<TableDefinitionAttribute> tablesDefs = DbContext.FindTableDefinitions(this);
            string tableName = "";

            foreach(TableDefinitionAttribute table in tablesDefs)
            {
                tableName = table.TableName;
                if(Tables.TableList[tableName].Fields.Contains(fieldName))
                {
                    return $"{tableName}.{fieldName} AS p_{fieldName}, ";
                }
            }

            return "";
        }

        #endregion Private Methods
    }
}