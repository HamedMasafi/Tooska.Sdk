using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Tooska.Payment;

public delegate void OpenCompletedEventHandler(Browser sender, Uri uri);

public delegate void ErrorEventHandler(Browser sender, Uri uri, Exception ex);

public class PostData : NameValueCollection
{
    // ReSharper disable once UnusedMember.Global
    public void Add(string name, int value)
    {
        Set(name, value.ToString());
    }

#pragma warning disable CS8765
    public override void Add(string name, string value)
    {
        Set(name, value);
    }
#pragma warning restore CS8765

    public override string ToString()
    {
        var s = "";
        foreach (var key in AllKeys)
        {
            if (!string.IsNullOrEmpty(s))
                s += "&";

            s += key + "=" + HttpUtility.UrlEncode(this[key]);
        }

        return s;
    }

    public byte[] GetBytes()
    {
        return Encoding.UTF8.GetBytes(this.ToString());
    }

    public StringContent ToJsonContent()
    {
        var json = JsonSerializer.Serialize(this);
        return new StringContent(json);
    }
}

public struct CallUrlResult
{
    public HttpStatusCode Code { get; init; }
    public string Content { get; init; }
}

public static class Opener
{
    public static async Task<CallUrlResult> Open(string url, string data)
    {
        HttpClient _client = new();

        var result = await _client.PostAsync(url, new StringContent(data));

        return new CallUrlResult()
        {
            Code = result.StatusCode,
            Content = await result.Content.ReadAsStringAsync()
        };
    }
    
    public static async Task<CallUrlResult> Open(string url, NameValueCollection data)
    {
        HttpClient _client = new();

        var json = JsonSerializer.Serialize(data);

        var result = await _client.PostAsync(url, new StringContent(json));

        return new CallUrlResult()
        {
            Code = result.StatusCode,
            Content = await result.Content.ReadAsStringAsync()
        };
    }
}
public class Browser : IDisposable
{
    private const string ContentType = "application/x-www-form-urlencoded";
    private const string HeaderAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
    public HttpStatusCode StatusCode { get; set; }
    public event OpenCompletedEventHandler OpenCompleted;
    public event ErrorEventHandler Error;

    readonly CookieContainer _cookies = new CookieContainer();

    private HttpWebRequest _request;

    private Dictionary<string, string> _headers = new Dictionary<string, string>();

    public string UserAgent { get; set; }
    public string Content { get; set; }
    public Uri Uri { get; set; }

    public Exception Exception { get; set; }

    public Browser()
    {
        Content = "";
        Uri = null;
        UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36";
        _client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
    }

    public void AddHeader(string name, string value)
    {
        _headers[name] = value;
    }

    public void SetCookie(string name, string value, string domain)
    {
        _cookies.Add(new Cookie(name, value)
        {
            Domain = domain
        });
    }

    private readonly HttpClient _client = new();

    public async Task<bool> Open2(string address, PostData? data = null)
    {
        HttpResponseMessage result;

        if (data == null)
            result = await _client.GetAsync(address);
        else
            result = await _client.PostAsync(address, data.ToJsonContent());

        if (result.StatusCode == HttpStatusCode.OK)
        {
            Content = await result.Content.ReadAsStringAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> Open3(string address, PostData? data = null)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, address);
        foreach (var header in _headers)
            requestMessage.Headers.Add(header.Key, header.Value);

        requestMessage.Headers.Add("User-Agent", UserAgent);

        var res = await _client.SendAsync(requestMessage);
        return true;
    }

    public struct OpenData
    {
        public string Url { get; set; }
        public string Content { get; set; }
    }


    public async Task<List<OpenData>> ConcurrentOpen(IEnumerable<string> urls)
    {
        var data = urls.Select(url => new
            {
                Url = url,
                Task = _client.GetAsync(url)
            })
            .ToList();

        await Task.WhenAll(data.Select(d => d.Task));

        var responses = data.Select
        (d => new
            {
                d.Url,
                Content = d.Task.Result.Content.ReadAsStringAsync()
            }
        );

        await Task.WhenAll(responses.Select(d => d.Content));

        return responses.Select(r => new OpenData()
            {
                Url = r.Url,
                Content = r.Content.Result
            })
            .ToList();
    }

    public async Task<bool> Open(string address, PostData? data = null)
    {
        Exception = null;
        _request = (HttpWebRequest) WebRequest.Create(address);

        byte[] byteArray = null;
        if (data == null)
        {
            _request.Method = "GET";
        }
        else
        {
            _request.Method = "POST";

            byteArray = data.GetBytes();
            _request.ContentLength = byteArray.Length;

            _request.ContentType = ContentType;
        }

        _request.UserAgent = UserAgent;
        _request.CookieContainer = _cookies;
        _request.KeepAlive = true;
        _request.Accept = HeaderAccept;
        //_request.ProtocolVersion = HttpVersion.Version11;
        _request.Referer = Uri == null ? "" : Uri.ToString();

        foreach (var header in _headers)
        {
            _request.Headers.Add(header.Key, header.Value);
        }
        // 
        //_request.Headers.Add("Accept-Encoding", "gzip, deflate");
        //_request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
        //_request.Headers.Add("Cache-Control", "max-age=0");
        //_request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

        try
        {
            if (data != null)
            {
                await using var dataStreamBody = _request.GetRequestStream();
                await dataStreamBody.WriteAsync(byteArray, 0, byteArray.Length);
                dataStreamBody.Close();
            }

            using var response = await _request.GetResponseAsync();
            await using var dataStream = response.GetResponseStream();
            if (dataStream == null)
                return false;

            using var reader = new StreamReader(dataStream); //, Encoding.GetEncoding(1256));

            Uri = response.ResponseUri;
            Content = /*HttpUtility.UrlDecode*/(await reader.ReadToEndAsync());


            //var b = Encoding.ASCII.GetBytes(Content);
            ////"ISO-8859-1").GetBytes(_Content);
            ////"windows-1256").GetBytes(_Content);
            //Content = Encoding.UTF8.GetString(b);
            //_Content = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(_Content));

            //reader.Close();
            //dataStream.Close();
            //response.Close();

            StatusCode = HttpStatusCode.OK;
            OpenCompleted?.Invoke(this, _request.Address);
            return true;
        }
        catch (WebException ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                    StatusCode = response.StatusCode;
            }
            else
            {
                StatusCode = HttpStatusCode.BadRequest;
            }

            Exception = ex;
            Error?.Invoke(this, _request.Address, ex);
            Uri = _request.Address;
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Exception = ex;
            Error?.Invoke(this, _request.Address, ex);
            Uri = _request.Address;
            return false;
        }
    }

    void IDisposable.Dispose()
    {
        Content = null;
    }
}