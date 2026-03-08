using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace DigitalPlus.Licensing
{
    [DataContract]
    internal class ActivateRequest
    {
        [DataMember(Name = "activationCode")]
        public string ActivationCode { get; set; }

        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [DataMember(Name = "installType")]
        public string InstallType { get; set; }
    }

    [DataContract]
    internal class HeartbeatRequest
    {
        [DataMember(Name = "companyId")]
        public string CompanyId { get; set; }

        [DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [DataMember(Name = "app")]
        public string App { get; set; }

        [DataMember(Name = "activeLegajos")]
        public int ActiveLegajos { get; set; }
    }

    [DataContract]
    internal class LicenseResponse
    {
        [DataMember(Name = "ticket")]
        public string Ticket { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }

    public class LicenseClient
    {
        private const string BaseUrl = "https://digitalplus-provision.azurewebsites.net/api/";
        private readonly string _apiKey;
        private const int TimeoutMs = 15000;

        public LicenseClient(string apiKey)
        {
            _apiKey = apiKey ?? string.Empty;
        }

        public bool TryActivate(string activationCode, string companyName, string machineId,
            string installType, out string ticketJson, out string signature, out string error)
        {
            ticketJson = null;
            signature = null;
            error = null;

            try
            {
                var req = new ActivateRequest
                {
                    ActivationCode = activationCode,
                    CompanyName = companyName,
                    MachineId = machineId,
                    InstallType = installType
                };

                var json = JsonHelper.Serialize(req);
                var responseJson = Post(BaseUrl + "license/activate", json, out var statusCode);

                if (statusCode == 200)
                {
                    var resp = JsonHelper.Deserialize<LicenseResponse>(responseJson);
                    ticketJson = resp.Ticket;
                    signature = resp.Signature;
                    return true;
                }

                error = "Error del servidor (codigo " + statusCode + ")";
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool TryHeartbeat(string companyId, string machineId, string app, int activeLegajos,
            out string ticketJson, out string signature, out string error)
        {
            ticketJson = null;
            signature = null;
            error = null;

            try
            {
                var req = new HeartbeatRequest
                {
                    CompanyId = companyId,
                    MachineId = machineId,
                    App = app,
                    ActiveLegajos = activeLegajos
                };

                var json = JsonHelper.Serialize(req);
                var responseJson = Post(BaseUrl + "license/heartbeat", json, out var statusCode);

                if (statusCode == 200)
                {
                    var resp = JsonHelper.Deserialize<LicenseResponse>(responseJson);
                    ticketJson = resp.Ticket;
                    signature = resp.Signature;
                    return true;
                }

                error = "Error del servidor (codigo " + statusCode + ")";
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private string Post(string url, string jsonBody, out int statusCode)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = TimeoutMs;

            if (!string.IsNullOrEmpty(_apiKey))
                request.Headers.Add("X-Api-Key", _apiKey);

            var bodyBytes = Encoding.UTF8.GetBytes(jsonBody);
            request.ContentLength = bodyBytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bodyBytes, 0, bodyBytes.Length);
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse errorResp)
            {
                statusCode = (int)errorResp.StatusCode;
                using (var reader = new StreamReader(errorResp.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
