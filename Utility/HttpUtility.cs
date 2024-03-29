﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using GatewayApiClient.EnumTypes;

namespace GatewayApiClient.Utility {

    internal class HttpUtility {

        /// <summary>
        /// Envia uma requisição para o gateway
        /// </summary>
        /// <typeparam name="TResponse">Tipo de resposta do gateway</typeparam>
        /// <param name="serviceUri">Url do serviço do gateway</param>
        /// <param name="httpVerb">HttpMethod utilizado na chamada ao serviço</param>
        /// <param name="contentType">Formato da resposta (XML ou JSON)</param>
        /// <param name="header">Cabeçalho da requisição</param>
        /// <returns></returns>
        public HttpResponse<TResponse> SubmitRequest<TResponse>(string serviceUri, HttpVerbEnum httpVerb, HttpContentTypeEnum contentType, NameValueCollection header) {

            Uri uri = new Uri(serviceUri);

            // Envia o request.
            HttpResponse<TResponse> response = this.SendHttpWebRequest<TResponse>(httpVerb, contentType, uri.ToString(), header);

            return response;
        }

        /// <summary>
        /// Envia uma requisição para o gateway
        /// </summary>
        /// <typeparam name="TRequest">Tipo de requisição enviada para o gateway</typeparam>
        /// <typeparam name="TResponse">Tipo de resposta do gateway</typeparam>
        /// <param name="request">Requisição enviada para o gateway</param>
        /// <param name="serviceUri">Url do serviço do gateway</param>
        /// <param name="httpVerb">HttpMethod utilizado na chamada ao serviço</param>
        /// <param name="contentType">Formato da requisição e da resposta (XML ou JSON)</param>
        /// <param name="header">Cabeçalho da requisição</param>
        /// <returns></returns>
        public HttpResponse<TResponse, TRequest> SubmitRequest<TRequest, TResponse>(TRequest request, string serviceUri, HttpVerbEnum httpVerb, HttpContentTypeEnum contentType, NameValueCollection header) {

            Uri uri = new Uri(serviceUri);

            HttpResponse<TResponse, TRequest> response = this.SendHttpWebRequest<TResponse, TRequest>(request, httpVerb, contentType, serviceUri.ToString(), header);

            return response;
        }
        
        /// <summary>
        /// Cria uma requisição http com um objeto do especificado em TRequest e retorna um objeto do tipo especificado em TResponse.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="httpVerbEnum"></param>
        /// <param name="serviceEndpoint"></param>
        /// <param name="headerData"></param>
        /// <param name="allowInvalidCertificate"></param>
        /// <param name="ignoreResponse"></param>
        /// <returns></returns>
        private HttpResponse<TResponse, TRequest> SendHttpWebRequest<TResponse, TRequest>(TRequest request, HttpVerbEnum httpVerbEnum, HttpContentTypeEnum httpContentType, string serviceEndpoint, NameValueCollection headerData = null) {

            // Serializa o request como json.
            string rawRequest = this.SerializeObject<TRequest>(request, httpContentType);

            // Se o header é nulo, instancia um novo.
            headerData = headerData ?? new NameValueCollection();

            // Envia o request.
            HttpResponse httpResponse = this.SendHttpWebRequest(rawRequest, httpVerbEnum, httpContentType, serviceEndpoint, headerData);

            // Deserializa a resposta.
            TResponse responseObject = this.DeserializeObject<TResponse>(httpResponse.RawResponse, httpContentType);

            HttpResponse<TResponse, TRequest> response = new HttpResponse<TResponse, TRequest>(request, rawRequest, responseObject, httpResponse.RawResponse, httpResponse.HttpStatusCode);

            return response;
        }

        /// <summary>
        /// Cria uma requisição http e retorna um objeto do tipo especificado em TResponse.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="httpVerbEnum"></param>
        /// <param name="serviceEndpoint"></param>
        /// <param name="headerData"></param>
        /// <param name="allowInvalidCertificate"></param>
        /// <param name="ignoreResponse"></param>
        /// <returns></returns>
        private HttpResponse<TResponse> SendHttpWebRequest<TResponse>(HttpVerbEnum httpVerbEnum, HttpContentTypeEnum httpContentType, string serviceEndpoint, NameValueCollection headerData = null) {

            // Se o header é nulo, instancia um novo.
            headerData = headerData ?? new NameValueCollection();

            // Envia o request.
            HttpResponse httpResponse = this.SendHttpWebRequest("", httpVerbEnum, httpContentType, serviceEndpoint, headerData);

            // Deserializa a resposta.
            TResponse responseObject = this.DeserializeObject<TResponse>(httpResponse.RawResponse, httpContentType);

            HttpResponse<TResponse> response = new HttpResponse<TResponse>(responseObject, httpResponse.RawResponse, httpResponse.HttpStatusCode);

            return response;
        }

        /// <summary>
        /// Envio de HttpWebRequest
        /// </summary>
        /// <param name="dataToSend"></param>
        /// <param name="httpVerbEnum"></param>
        /// <param name="httpContentTypeEnum"></param>
        /// <param name="serviceEndpoint"></param>
        /// <param name="headerData"></param>
        /// <param name="allowInvalidCertificate"></param>
        /// <param name="ignoreResponse"></param>
        /// <returns></returns>
        private HttpResponse SendHttpWebRequest(string dataToSend, HttpVerbEnum httpVerbEnum, HttpContentTypeEnum httpContentTypeEnum, string serviceEndpoint, NameValueCollection headerData, bool allowInvalidCertificate = false) {

            if (string.IsNullOrWhiteSpace(serviceEndpoint)) { throw new ArgumentException("serviceEndpoint"); }

            // Cria um objeto webRequest para referenciando a Url do serviço informado
            Uri serviceUri = new Uri(serviceEndpoint);

            HttpWebRequest httpWebRequest = WebRequest.Create(serviceUri) as HttpWebRequest;
            httpWebRequest.Method = httpVerbEnum.ToString().ToUpper();
            httpWebRequest.ContentType = httpContentTypeEnum == HttpContentTypeEnum.Json ? "application/json" : "text/xml";
            httpWebRequest.Accept = httpContentTypeEnum == HttpContentTypeEnum.Json ? "application/json" : "text/xml";

            if (headerData != null && headerData.AllKeys.Contains("User-Agent") == true) {
                httpWebRequest.UserAgent = headerData["User-Agent"];
                headerData.Remove("User-Agent");
            }

            // Verifica se certificados inválidos devem ser aceitos.
            if (allowInvalidCertificate == true) {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            // Verifica se deverão ser enviados dados/chaves de cabeçalho
            if (headerData != null && headerData.Count > 0) {

                // Insere cada chave enviado no cabeçalho da requisição
                foreach (string key in headerData.Keys) { httpWebRequest.Headers.Add(key, headerData[key].ToString()); }
            }

            if (string.IsNullOrWhiteSpace(dataToSend) == false) {

                // Cria um array de bytes dos dados que serão postados
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(dataToSend);

                // Configura o tamanho dos dados que serão enviados
                httpWebRequest.ContentLength = byteData.Length;

                // Escreve os dados na stream do WebRequest
                using (Stream stream = httpWebRequest.GetRequestStream()) {
                    stream.Write(byteData, 0, byteData.Length);
                }
            }

            string rawResponse = string.Empty;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            try {
                // Dispara a requisição e recebe o resultado da mesma
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse) {

                    // Recupera a stream com a resposta da solicitação
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    rawResponse = streamReader.ReadToEnd();
                    statusCode = response.StatusCode;
                }
            }
            catch (WebException ex) {

                HttpWebResponse response = (HttpWebResponse)ex.Response;
                StreamReader test = new StreamReader(response.GetResponseStream());
                rawResponse = test.ReadToEnd();
                statusCode = response.StatusCode;
            }

            return new HttpResponse(rawResponse, statusCode);
        }

        #region Serialization

        /// <summary>
        /// Serializa um objeto para uma string Json ou Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="httpContentType"></param>
        /// <returns></returns>
        private string SerializeObject<T>(T obj, HttpContentTypeEnum httpContentType) {

            if (obj == null) { return null; }

            string serializedString = string.Empty;

            // Classe de serialização Json
            XmlObjectSerializer serializer = null;
            if (httpContentType == HttpContentTypeEnum.Json) {
                serializer = new DataContractJsonSerializer(typeof(T));
            }
            else if (httpContentType == HttpContentTypeEnum.Xml) {
                serializer = new DataContractSerializer(typeof(T));
            }

            // Serializa o objeto para o memoryStream
            MemoryStream memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, obj);

            // Recupera a string correspondente ao objeto serializado
            serializedString = Encoding.UTF8.GetString(memoryStream.ToArray());

            memoryStream.Close();

            return serializedString;
        }

        /// <summary>
        /// Realiza a deserialização de uma string Json ou Xml para um objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <param name="httpContentType"></param>
        /// <returns></returns>
        private T DeserializeObject<T>(string serializedObject, HttpContentTypeEnum httpContentType) {

            // Classe de serialização Json
            XmlObjectSerializer deserializer = null;

            if (httpContentType == HttpContentTypeEnum.Json) {
                deserializer = new DataContractJsonSerializer(typeof(T));
            }
            else if (httpContentType == HttpContentTypeEnum.Xml) {
                deserializer = new DataContractSerializer(typeof(T));
            }

            // Recupera os bytes correspondentes a string
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject));

            // Realiza a deserialização da string para o objeto informado
            T obj = (T)deserializer.ReadObject(ms);

            return obj;
        }

        #endregion
    }
}
