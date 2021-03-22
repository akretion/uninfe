﻿using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Xml
{
    /// <summary>
    /// Classe Base para criação de classes de serialização de XML
    /// </summary>
    [ComVisible(true)]
    public abstract class XMLBase
    {
        #region Protected Properties

        /// <summary>
        /// Lista de NameSpaces
        /// </summary>
        protected List<XMLUtility.TNameSpace> NameSpaces { get; }

        #endregion Protected Properties

        #region Public Constructors

        /// <summary>
        /// Construtor base
        /// </summary>
        public XMLBase()
        {
            var attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            if(attribute == null)
            {
                return;
            }

            NameSpaces = new List<XMLUtility.TNameSpace>
            {
                new XMLUtility.TNameSpace() { Prefix = "", NS = attribute.Namespace }
            };
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <returns>String contendo o XML</returns>
        public virtual XmlDocument GerarXML() => XMLUtility.Serializar(this, NameSpaces);

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        [ComVisible(false)]
        public virtual T LerXML<T>(XmlDocument doc)
            where T : new() => XMLUtility.Deserializar<T>(doc);

        #endregion Public Methods
    }
}