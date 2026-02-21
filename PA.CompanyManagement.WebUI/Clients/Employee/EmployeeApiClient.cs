using Newtonsoft.Json;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Requests;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Responses;
using PA.CompanyManagement.EmployeeService.Application.Repositories;
using System.Text;
using System.Text.Json.Serialization;

namespace PA.CompanyManagement.WebUI.Clients.Employee
{
    public interface IEmployeeApiClient : IEmployeeRepository
    {
        
    }

    public class EmployeeApiClient : IEmployeeApiClient
    {
        private readonly HttpClient _client;

        public EmployeeApiClient(HttpClient client)
        {
            _client = client;
        }


        public async Task<EmployeeResponse> CreateAsync(EmployeeCreateRequest request)
        {
            try
            {
                string serializedContent = JsonConvert.SerializeObject(request);

                StringContent content = new StringContent(serializedContent, Encoding.UTF8,
                    "application/json");

                var response = await _client.PostAsync("", content);
                response.EnsureSuccessStatusCode();

                var stringContent = await response.Content.ReadAsStringAsync();
                if(string.IsNullOrWhiteSpace(stringContent))
                    return new EmployeeResponse();

                var data = JsonConvert.DeserializeObject<EmployeeResponse>(stringContent);

                return data!;


            }
            catch(Exception ex)
            {
                return new EmployeeResponse();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var response = await _client.DeleteAsync(id.ToString());
                response.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            try
            {

                var response = await _client.GetAsync("");
                response.EnsureSuccessStatusCode();

                var stringContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(stringContent))
                    return new List<EmployeeResponse>();

                //var jsonContent = await response.Content.ReadFromJsonAsync<List<EmployeeResponse>>();

                var data = JsonConvert.DeserializeObject<List<EmployeeResponse>>(stringContent);
                return data!;
            }
            catch (Exception ex)
            {
                return new List<EmployeeResponse>();
            }
        }

        public async Task<EmployeeResponse?> GetAsync(Guid id)
        {

            try
            {
                var response = await _client.GetAsync(id.ToString());
                response.EnsureSuccessStatusCode();
                var stringContent = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<EmployeeResponse?>(stringContent);
                return data;

            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public async Task<DetailedEmployeeResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                var response = await _client.GetAsync($"detailed/{id.ToString()}");
                response.EnsureSuccessStatusCode();
                var stringContent = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<DetailedEmployeeResponse?>(stringContent);
                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateAsync(EmployeeUpdateRequest request)
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(request);

                StringContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(request.Id.ToString(), content);
                response.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
