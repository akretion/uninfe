namespace NFe.Components.Interface
{
    public interface IGetRequest : IRequest
    {
        /// <summary>
        /// Faz o Get e retorna uma string como resultado
        /// </summary>
        /// <param name="url">url base para utilizar dentro do get</param>
        /// <returns></returns>
        string GetForm(string url);
    }
}