using System;

namespace NFe.Components
{
    /// <summary>
    /// Interface para a emissão de NFSe do padrão AGILI
    /// </summary>
    public interface IEmiteNFSeAGILI : IEmiteNFSe
    {
        string ClientID { get; set; }
        string ClientSecret { get; set; }
        string URLAPIBase { get; }
        string Token { get; set; }
        DateTime TokenExpire { get; set; }
    }
}