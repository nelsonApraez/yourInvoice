///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Newtonsoft.Json;
using RestSharp;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;

namespace yourInvoice.Common.Integration.ZapSign
{
    public class Zapsign : IZapsign
    {
        private readonly ICatalogBusiness _catalog;

        public Zapsign(ICatalogBusiness catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public async Task<ZapsignFileResponse> CreateDocAsync(ZapsignFileRequest parameters)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(ZapSignCatalog.CatalogName);

            //consultar valores
            var url = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignUrl);
            var uri = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignCreateUri);
            var token = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignToken);

            //realizar consulta
            var content = Post<ZapsignFileRequest, ZapsignFileResponse>(url?.Descripton, uri?.Descripton, token?.Descripton, parameters);

            return content;
        }

        public ZapsignFileResponse CreateDoc(string url, string uri, string token, ZapsignFileRequest parameters)
        {
            //realizar consulta
            var content = Post<ZapsignFileRequest, ZapsignFileResponse>(url, uri, token, parameters);

            return content;
        }

        public async Task<ZapsignFileAttachmentResponse> AddAttachmentAsync(string principalDocToken, ZapsignFileAttachmentRequest parameter)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(ZapSignCatalog.CatalogName);

            //consultar valores
            var url = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignUrl);
            var uri = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignAttachmentUri);
            var token = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignToken);

            //realizar consulta
            var uriText = uri.Descripton.Replace("{{original_doc_token}}", principalDocToken);
            var content = Post<ZapsignFileAttachmentRequest, ZapsignFileAttachmentResponse>(url?.Descripton, uriText, token?.Descripton, parameter);

            return content;
        }

        public ZapsignFileAttachmentResponse AddAttachment(string url, string uri, string token, ZapsignFileAttachmentRequest parameter)
        {
            //realizar consulta
            var content = Post<ZapsignFileAttachmentRequest, ZapsignFileAttachmentResponse>(url, uri, token, parameter);

            return content;
        }

        public async Task<ZapsignFileDetailResponse> GetDetailAsync(string principalDocToken)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(ZapSignCatalog.CatalogName);

            //consultar valores
            var url = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignUrl);
            var uri = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignDetailDocUri);
            var token = catalogs.FirstOrDefault(x => x.Name == ZapSignCatalog.ZapSignToken);

            //realizar consulta
            var uriText = uri.Descripton.Replace("{{doc_token}}", principalDocToken);
            var content = Get<ZapsignFileDetailResponse>(url?.Descripton, uriText, token?.Descripton);

            return content;
        }

        public ZapsignFileDetailResponse GetDetail(string url, string uri, string token)
        {
            //realizar consulta
            var content = Get<ZapsignFileDetailResponse>(url, uri, token);

            return content;
        }

        private R Post<T, R>(string url, string uri, string token, T parameters)
        {
            try
            {
                if (url == null)
                    throw new ArgumentException("La url no debe ser nulo ");

                if (uri == null)
                    throw new ArgumentException("La uri no debe ser nulo ");

                if (token == null)
                    throw new ArgumentException("El token no debe ser nulo ");

                if (object.Equals(parameters, default(T)))
                    throw new ArgumentException("Los parametros no deben ser nulos ");

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                headers.Add("Authorization", $"Bearer {token}");
                headers.Add("Cache-Control", "no-cache");

                RestClient client = new RestClient();
                RestRequest request = new RestRequest($"{url}{uri}", Method.Post);
                request.AddBody(parameters);

                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }

                RestResponse restResponse = client.ExecutePost(request);
                var content = restResponse.IsSuccessful ? restResponse.Content : restResponse.ErrorException?.Message;
                var statusCode = restResponse.StatusCode.ToString();

                R docData = default(R);
                if (statusCode.ToLower() == "ok")
                    docData = JsonConvert.DeserializeObject<R>(content);

                return docData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private R Get<R>(string url, string uri, string token)
        {
            try
            {
                if (url == null)
                    throw new ArgumentException("La url no debe ser nulo ");

                if (uri == null)
                    throw new ArgumentException("La uri no debe ser nulo ");

                if (token == null)
                    throw new ArgumentException("El token no debe ser nulo ");

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                headers.Add("Authorization", $"Bearer {token}");
                headers.Add("Cache-Control", "no-cache");

                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest(uri, Method.Get);

                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }

                RestResponse restResponse = client.ExecuteGet(request);
                var content = restResponse.IsSuccessful ? restResponse.Content : restResponse.ErrorException?.Message;
                var statusCode = restResponse.StatusCode.ToString();

                R docData = default(R);
                if (statusCode.ToLower() == "ok")
                    docData = JsonConvert.DeserializeObject<R>(content);

                return docData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}