using System;
using Unimake.Data.Generic.Definitions;

namespace ERP.Net
{
    /// <summary>
    /// Identificador de chave primária das tabelas
    /// </summary>
    public struct GUID : IConvertible, ICloneable, IComparable, IID
    {
        #region Public Fields

        /// <summary>
        /// Valor do GUID quando vazio
        /// </summary>
        public static readonly GUID Empty = new GUID(0);

        /// <summary>
        /// Valor do GUID quando utilizado para cadastros não definidos.
        /// </summary>
        public static readonly GUID Undefined = new GUID(1000000000000000000);

        #endregion Public Fields

        #region Private Fields

        private long guid;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// ID vazio
        /// </summary>
        IID IID.Empty
        {
            get { return GUID.Empty; }
        }

        /// <summary>
        /// Retorna o tamanho do EGUID
        /// </summary>
        public int Length
        {
            get
            {
                return guid.ToString().Length;
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Instancia um novo objeto GUID com o valor passado
        /// </summary>
        /// <param name="guid">valor do GUID</param>
        public GUID(long guid)
        {
            this.guid = guid;
        }

        /// <summary>
        /// Instancia um novo objeto GUID com o valor passado
        /// </summary>
        /// <param name="guid">valor do GUID</param>
        public GUID(string guid)
        {
            long result = 0;
            long.TryParse(guid, out result);
            this.guid = result;
        }

        /// <summary>
        /// Instancia um novo objeto GUID com o valor passado
        /// </summary>
        /// <param name="guid">valor do GUID</param>
        public GUID(IID guid)
        {
            long result = 0;
            long.TryParse(guid == null ? "" : guid.ToString(), out result);
            this.guid = result;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Cria um novo GUID no padrão Globally Unique Identifier
        /// </summary>
        /// <returns></returns>
        public static GUID Create()
        {
            //todo definir de acordo com o settings
            string sbase = "01";
            var bytes = Guid.NewGuid().ToByteArray();
            Array.Resize(ref bytes, 17);
            long bigInt = BitConverter.ToInt64(bytes, 0);
            string s = bigInt.ToString().Substring(0, 12) + sbase;
            long result = long.Parse(s);
            return result;
        }

        /// <summary>
        /// Converte o valor definido em "value"
        /// </summary>
        /// <param name="value"></param>
        public static GUID Create(object value)
        {
            return new GUID(value == null ? "" : value.ToString());
        }

        /// <summary>
        /// Converte uma string no tipo GUID
        /// </summary>
        /// <param name="guid">Valor do tipo string que será trasnformado</param>
        /// <returns>Um novo GUID</returns>
        public static implicit operator GUID(long guid)
        {
            if (guid <= 0)
                return GUID.Empty;

            return new GUID(guid);
        }

        /// <summary>
        /// Converte uma GUID no tipo string
        /// </summary>
        /// <param name="rhs">GUID que será covnertido</param>
        /// <returns>Retorna uma string com o valor do EGUID</returns>
        public static implicit operator long(GUID rhs)
        {
            if (rhs != null)
                return rhs.guid;

            return Create();
        }

        /// <summary>
        /// Converte uma GUID no tipo string
        /// </summary>
        /// <param name="rhs">GUID que será covnertido</param>
        /// <returns>Retorna uma string com o valor do EGUID</returns>
        public static implicit operator string(GUID rhs)
        {
            if (rhs != null)
                return rhs.guid.ToString();

            return Create().ToString();
        }

        /// <summary>
        /// Retorna true se o GUID informado for nulo ou vazio
        /// </summary>
        /// <param name="guid">GUID que deverá ser comparado</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(GUID guid)
        {
            return guid == null || guid == GUID.Empty;
        }

        /// <summary>
        /// valida o GUID passado
        /// </summary>
        /// <param name="guid">GUID a ser validade</param>
        /// <returns>true se o guid for válido</returns>
        public static bool IsValid(GUID guid)
        {
            return !IsNullOrEmpty(guid);
        }

        /// <summary>
        /// Compara se o valor GUID é diferente do outro GUID
        /// </summary>
        /// <param name="lhs">Primeiro valor da comparação</param>
        /// <param name="rhs">Segundo valor da comparação</param>
        /// <returns>Verdadeiro se os valores forem diferntes</returns>
        public static bool operator !=(GUID lhs, GUID rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Compara se o valor GUID é igual ao outro GUID
        /// </summary>
        /// <param name="lhs">Primeiro valor da comparação</param>
        /// <param name="rhs">Segundo valor da comparação</param>
        /// <returns>Verdadeiro se os valores forem iguais</returns>
        public static bool operator ==(GUID lhs, GUID rhs)
        {
            if (System.Object.ReferenceEquals(rhs, lhs))
                return true;

            if (((object)rhs == null) || ((object)lhs == null))
                return false;

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Retorna uma cópia do objeto GUID
        /// </summary>
        /// <returns>Cópia deste objeto</returns>
        public GUID Clone()
        {
            return new GUID((long)this);
        }

        /// <summary>
        /// Realiza a comparação deste objeto com o objeto passado e retorna
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            GUID otherGUID = (GUID)obj;

            return guid.CompareTo(obj.ToString());
        }

        public IID CreateNewID(object id)
        {
            return new GUID(id?
                .ToString());
        }

        /// <summary>
        /// Retorna verdadeiro se o objeto for igual à este
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns>Verdadeiro se o objeto for igual à este</returns>
        public bool Equals(GUID obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            return obj.guid == guid;
        }

        /// <summary>
        /// Retorna verdadeiro se o objeto for igual à este
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns>Verdadeiro se o objeto for igual à este</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((GUID)obj);
        }

        /// <summary>
        /// Retorna o hashcode deste objeto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        /// Retorna uma cópia do objeto GUID
        /// </summary>
        /// <returns>Cópia deste objeto</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Compara os dois objetos e retorna
        /// </summary>
        /// <param name="obj">Objeto da comparação</param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            return this.CompareTo(obj);
        }

        /// <summary>
        /// Cria um novo objeto do tipo GUID
        /// </summary>
        /// <typeparam name="T">Define o Tipo do objeto, neste caso é uma string</typeparam>
        /// <returns>Um novo objeto GUID</returns>
        T IID.CreateNewID<T>()
        {
            return (T)(object)Create();
        }

        /// <summary>
        /// Cria um novo objeto do tipo GUID
        /// </summary>
        /// <typeparam name="T">Define o Tipo do objeto, neste caso é uma string</typeparam>
        /// <param name="id">ID passado para ser usado como base na criação de um novo</param>
        /// <returns>Um novo objeto GUID</returns>
        T IID.CreateNewID<T>(T id)
        {
            return (T)(object)Create(id);
        }

        /// <summary>
        /// Retorna true se este GUID informado for nulo ou vazio
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty()
        {
            return IsNullOrEmpty(this);
        }

        /// <summary>
        /// Retorna verdadeiro se este objeto for válido
        /// </summary>
        /// <returns></returns>
        bool IID.IsValid()
        {
            return this.IsValid();
        }

        /// <summary>
        /// Retorna true se o GUID for válido
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return GUID.IsValid(this);
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ToBoolean();
        }

        public bool ToBoolean()
        {
            bool ret = false;
            int i = 0;
            string valor = ToString(null);
            if (string.IsNullOrEmpty(valor)) return false;
            if (Char.IsNumber(valor, 0))
            {
                int.TryParse(valor, out i);
                i = Math.Abs(i);
                if (i == 0)
                    ret = false;
                else
                    return true;
            }
            else
                bool.TryParse(valor, out ret);
            return ret;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ToByte();
        }

        public byte ToByte()
        {
            byte ret = 0;
            byte.TryParse(ToString(null), out ret);
            return ret;
        }

        public char ToChar(IFormatProvider provider)
        {
            return ToChar();
        }

        public char ToChar()
        {
            char ret = new char();
            char.TryParse(ToString(null), out ret);
            return ret;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ToDateTime();
        }

        public DateTime ToDateTime()
        {
            DateTime ret = new DateTime();
            DateTime.TryParse(ToString(null), out ret);
            return ret;
        }

        /// <summary>
        /// Converte o GUID no tipo esperado pelo banco de dados
        /// </summary>
        /// <returns></returns>
        public object ToDbValue()
        {
            return IsValid() ? guid : (object)null;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ToDecimal();
        }

        public decimal ToDecimal()
        {
            decimal ret = 0;
            decimal.TryParse(ToString(null), out ret);
            return ret;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ToDouble();
        }

        public double ToDouble()
        {
            double ret = 0;
            double.TryParse(ToString(null), out ret);
            return ret;
        }

        public TEnum ToEnum<TEnum>()
        {
            return (TEnum)Enum.Parse(typeof(TEnum), ToString(null));
        }

        public int ToInt()
        {
            return ToInt32();
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ToInt16();
        }

        public short ToInt16()
        {
            Int16 ret = 0;
            Int16.TryParse(ToString(null), out ret);
            return ret;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ToInt32();
        }

        public int ToInt32()
        {
            Int32 ret = 0;
            Int32.TryParse(ToString(null), out ret);
            return ret;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ToInt64();
        }

        public long ToInt64()
        {
            Int64 ret = 0;
            Int64.TryParse(ToString(null), out ret);
            return ret;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ToSByte();
        }

        public sbyte ToSByte()
        {
            sbyte ret = 0;
            sbyte.TryParse(ToString(null), out ret);
            return ret;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ToSingle();
        }

        public float ToSingle()
        {
            float ret = 0;
            float.TryParse(ToString(null), out ret);
            return ret;
        }

        /// <summary>
        /// Retorna representação do objeto em string
        /// </summary>
        /// <returns>Retorna representação do objeto em string</returns>
        public override string ToString()
        {
            return guid.ToString();
        }

        /// <summary>
        /// Converte em string respeitando o formatprovider
        /// </summary>
        /// <param name="provider">Formato esperado</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ToType(conversionType);
        }

        public object ToType(Type conversionType)
        {
            return this;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ToUInt16();
        }

        public ushort ToUInt16()
        {
            UInt16 ret = 0;
            UInt16.TryParse(ToString(null), out ret);
            return ret;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ToUInt32();
        }

        public uint ToUInt32()
        {
            UInt32 ret = 0;
            UInt32.TryParse(ToString(null), out ret);
            return ret;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ToUInt64();
        }

        public ulong ToUInt64()
        {
            UInt64 ret = 0;
            UInt64.TryParse(ToString(null), out ret);
            return ret;
        }

        #endregion Public Methods
    }
}