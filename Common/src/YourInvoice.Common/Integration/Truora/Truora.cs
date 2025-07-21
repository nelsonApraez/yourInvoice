using Newtonsoft.Json;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using System.Reflection;

namespace yourInvoice.Common.Integration.Truora
{
    public class Truora : ITruora
    {
        private readonly ICatalogBusiness _catalog;
        private const string uriToken = "https://identity.truora.com/?token=";

        public Truora(ICatalogBusiness catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public async Task<Dictionary<string, string>> CreateApiKeyAsync(Guid generalInformationId)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(TruoraCatalog.CatalogName);

            //consultar valores
            var url = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraUrlAccount);
            var uri = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraApiKeysUri);
            var token = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraApiKey);
            var flow_id = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraFlowId);
            var redirectUrl = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraRedirectUrl);

            CreateApiKeyRequest parameters = new();
            parameters.flow_id = flow_id.Descripton;
            parameters.redirect_url = redirectUrl.Descripton;
            parameters.account_id = generalInformationId.ToString();

            //realizar consulta
            var content = await PostAsync<CreateApiKeyRequest, CreateApiKeyResponse>(url?.Descripton, uri?.Descripton, token?.Descripton, parameters);

            Dictionary<string,string> result = new ();
            result.Add("UrlGenerated", uriToken + content.api_key);
            return result;
        }

        public async Task<ProcessInfoResponse> GetProcessAsync(string processId)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(TruoraCatalog.CatalogName);

            //consultar valores
            var url = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraUrlIdentity);
            var uri = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraProcessesUri);
            var token = catalogs.FirstOrDefault(x => x.Name == TruoraCatalog.TruoraApiKey);

            var uriText = uri.Descripton.Replace("{{process_id}}", processId); 

            //realizar consulta
            var content = await GetAsync<ProcessInfoResponse>(url?.Descripton, uriText, token?.Descripton);

            return content;
        }



        private async Task<R> PostAsync<T, R>(string url, string uri, string token, T parameters)
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
                headers.Add("Truora-API-Key", $"{token}");

                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{url}{uri}");

                var collection = ConvertToKeyValueList(parameters);
                var content2 = new FormUrlEncodedContent(collection);
                request.Content = content2;

                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode.ToString();

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

        private async Task<R> GetAsync<R>(string url, string uri, string token)
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
                headers.Add("Truora-API-Key", $"{token}");

                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{url}{uri}");

                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode.ToString();

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

        private static List<KeyValuePair<string, string>> ConvertToKeyValueList(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "El objeto no puede ser nulo");

            var keyValueList = new List<KeyValuePair<string, string>>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    keyValueList.Add(new KeyValuePair<string, string>(property.Name, value.ToString()));
                }
            }

            return keyValueList;
        }
    }
}
