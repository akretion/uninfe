﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// NFe
    /// </summary>
    public class NFe
    {
        public Ide ide { get; private set; }
        public Emit emit { get; private set; }
        public Dest dest { get; private set; }
        public Avulsa avulsa { get; private set; }
        public Entrega entrega { get; private set; }

        public infNFe infNFe;
        public Retirada retirada { get; private set; }
        public List<autXML> autXML { get; private set; }
        public List<Det> det { get; private set; }
        public Total Total;
        public Transp Transp { get; private set; }
        public Cobr Cobr { get; private set; }
        public InfAdic InfAdic { get; private set; }
        public Exporta exporta;
        public Compra compra;
        public Cana cana;
        public protNFe protNFe { get; private set; }

        /// <summary>
        /// NFC-e
        /// </summary>
        public List<pag> pag { get; private set; }
        public qrCode qrCode { get; private set; }

        public NFe()
        {
            ide = new Ide();
            emit = new Emit();
            dest = new Dest();
            avulsa = new Avulsa();
            entrega = new Entrega();
            autXML = new List<ConvertTxt.autXML>();
            retirada = new Retirada();
            det = new List<Det>();
            Transp = new Transp();
            Cobr = new Cobr();
            InfAdic = new InfAdic();
            cana = new Cana();
            protNFe = new protNFe();
            ///
            /// NFC-e
            pag = new List<pag>();
            qrCode = new qrCode();

            infNFe.Versao = (decimal)2.0;
        }
    }
}
