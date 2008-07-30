using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace uninfe
{
    public partial class FormSobre : Form
    {
        public FormSobre()
        {
            InitializeComponent();

            //Atualizar o texto da licença de uso
            this.textBox_licenca.Text  = "SOFTWARE: UniNFe\r\n";
            this.textBox_licenca.Text += "AUTOR: Unimake Soluções Corporativas LTDA\r\n\r\n";
            this.textBox_licenca.Text += "Importante – Leia Atentamente: A autorização de utilização do SOFTWARE é uma aceitação legal entre você e o AUTOR do SOFTWARE acima identificado.\r\n\r\n";
            this.textBox_licenca.Text += "1. AUTORIZAÇÃO DE UTILIZAÇÃO: O AUTOR concede a você a autorização para uso de forma ilimitada, não exclusiva, não transferível, livre de royalties para uso do executável do SOFTWARE, em seu computador, tal como previsto neste termo.\r\n\r\n";
            this.textBox_licenca.Text += "2. DISTRIBUIÇÃO: Você está por meio deste termo licenciado para fazer cópias do SOFTWARE de acordo com suas necessidades; ceder cópias exatas do SOFTWARE original para terceiros; e distribuí-lo, desde que não modificado; por qualquer meio eletrônico (Internet, e-mail, software de comunicação instantânea, CD-ROMs, etc.).\r\n\r\n";
            this.textBox_licenca.Text += "3. MANUTENÇÃO: O AUTOR não é responsável por prover qualquer tipo de manutenção ou atualização do SOFTWARE, no entanto qualquer manutenção ou atualização provida pelo mesmo estará coberta por este termo.\r\n\r\n";
            this.textBox_licenca.Text += "4. ISENÇÃO DE RESPONSABILIDADE: O AUTOR não se responsabiliza pela utilização deste SOFTWARE por qualquer pessoa, bem como por qualquer falha ou problema decorrente de sua utilização.\r\n\r\n";
            this.textBox_licenca.Text += "5. CÓDIGO FONTE: O código fonte deste SOFTWARE, desenvolvido em C#, é livre para modificações por qualquer pessoa, mas, uma vez modificado, estará a partir de então totalmente desvinculado do AUTOR original.";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.unimake.com.br/nfe");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:nfe@unimake.com.br");
        }
    }
}
