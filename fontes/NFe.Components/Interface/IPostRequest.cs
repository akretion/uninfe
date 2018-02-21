using System.Collections.Generic;

namespace NFe.Components.Interface
{
    public interface IPostRequest : IRequest
    {
        /// <summary>
        /// Faz o post e retorna uma string  com o resultado
        /// </summary>
        /// <param name="url">url base para utilizar dentro do post</param>
        /// <param name="postData">dados a serem enviados junto com o post</param>
        /// <returns></returns>
        string PostForm(string url, IDictionary<string, string> postData);

        /// <summary>
        /// Faz o post e retorna uma string  com o resultado
        /// </summary>
        /// <param name="url">url base para utilizar dentro do post</param>
        /// <param name="postData">dados a serem enviados junto com o post</param>
        /// <returns></returns>
        string PostForm(string url, IDictionary<string, string> postData = null, IList<string> headers = null);
    }
}