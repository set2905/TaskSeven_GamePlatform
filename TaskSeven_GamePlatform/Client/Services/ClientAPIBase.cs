using System.Net.Http.Json;

namespace TaskSeven_GamePlatform.Client.Services
{
    public abstract class ClientAPIBase
    {
        private readonly HttpClient httpClient;
        public ClientAPIBase(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        protected async Task<TReturn?> GetAsync<TReturn>(string relativeUri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(relativeUri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return await response.Content.ReadFromJsonAsync<TReturn>();
        }
        protected async Task<TReturn?> PostAsync<TReturn, TRequest>(string relativeUri, TRequest requestParam)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(relativeUri, requestParam);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return await response.Content.ReadFromJsonAsync<TReturn>(); ;
        }
        protected async Task PostAsync<TRequest>(string relativeUri, TRequest requestParam)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(relativeUri, requestParam);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
