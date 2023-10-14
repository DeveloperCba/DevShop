using System.Net;
using System.Text;
using DevShop.Core.DomainObjects;
using DevShop.Core.DomainObjects.Exceptions;
using DevShop.WebAPI.Core.Extensions;
using DevShop.WebAPI.Core.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DevShop.WebAPI.Core.Services;

public abstract class BaseService
{
    private readonly INotify _notification;

    protected BaseService(INotify notification)
    {
        _notification = notification;
    }

    protected StringContent StringContentTextJson(object dado, string mediaType = "application/json")
    {
        return new StringContent(
            System.Text.Json.JsonSerializer.Serialize(dado),
            Encoding.UTF8,
            mediaType
        );
    }

    protected StringContent StringContent(object dado, string mediaType = "application/json", bool contractResolver = false)
    {
        var camelSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
        };

        if (contractResolver)
            camelSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        return new StringContent(
            JsonConvert.SerializeObject(dado, camelSettings),
            Encoding.UTF8,
            mediaType
        );
    }

    protected async Task<T> DeserializeObjectResponseTextJson<T>(HttpResponseMessage responseMessage)
    {
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return System.Text.Json.JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);

        }
        catch (Exception ex)
        {
            _notification.Handler(new NotificationMessage(ex.Message));
            throw new InternalServerErrorException("Ocorreu um erro interno.");
        }
    }


    protected T DeserializeObjectResponseTextJson<T>(HttpResponse response)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(response.Content);
        }
        catch (Exception ex)
        {
            _notification.Handler(new NotificationMessage(ex.Message));
            throw new InternalServerErrorException("Ocorreu um erro interno.");
        }
    }

    protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
            return response;
        }
        catch (Exception ex)
        {
            _notification.Handler(new NotificationMessage(ex.Message));
            throw new InternalServerErrorException("Ocorreu um erro interno.");
        }
    }

    protected T DeserializeObjectResponse<T>(HttpResponse response)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
        catch (Exception ex)
        {
            _notification.Handler(new NotificationMessage(ex.Message));
            throw new InternalServerErrorException("Ocorreu um erro interno.");
        }
    }
    protected async Task<T> DeserializeObjectResponseAsync<T>(HttpResponse response)
    {
        try
        {
            return await Task.Run(() =>
                JsonConvert.DeserializeObject<T>(response.Content)
            );
        }
        catch (Exception ex)
        {
            _notification.Handler(new NotificationMessage(ex.Message));
            throw new InternalServerErrorException("Ocorreu um erro interno.");
        }
    }

    protected bool HandleErrorsResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest) return false;

        switch ((int)response.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpRequestException(response.StatusCode);

            case 400:
                return false;
        }
        response.EnsureSuccessStatusCode();
        return true;
    }

    protected async Task<HttpResponse> GetResult(HttpResponseMessage responseMessage)
    {
        return new HttpResponse
        {
            StatusCode = responseMessage.StatusCode,
            Content = await responseMessage.Content.ReadAsStringAsync(),
            ContentBytes = await responseMessage.Content.ReadAsByteArrayAsync()
        };
    }

    protected ResultResponse ReturnOk()
    {
        return new ResultResponse();
    }


    protected bool RunValidation<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : class
    {
        var validator = validacao.Validate(entidade);

        if (validator.IsValid) return true;

        NotificationEvent(validator);

        return false;
    }

    protected void NotificationEvent(string mensagem) => _notification.Handler(new NotificationMessage(mensagem));


    protected void NotificationEvent(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            NotificationEvent(error.ErrorMessage);
    }

    protected T ReturnDefault<T>()
    {
        return default;
    }

    #region Métodos auxiliares para requisições internas
    protected HttpResponse CheckStatusCodeReturn(HttpResponse response, HttpStatusCode? waitStatusCode)
    {
        if (!waitStatusCode.HasValue)
            return response;

        if (waitStatusCode.Value != response.StatusCode)
        {
            //var erro = new LogErro(metodo: string.Concat("VERIFIQUE O CÓDIGO DE STATUS E RETORNA: ", this.GetType().Name), path: null, erro: $"Falha na verificação do código de status e retorno {response.StatusCode}.", erroCompleto: null, query: null);
            //System.Console.WriteLine($"Erro: {erro.Metodo}");
        }

        return response;
    }
    #endregion

    #region Métodos auxiliares para requisições.

    protected void AddHeaders(ref HttpClient client, Dictionary<string, string> headers = null)
    {
        if (headers == null)
            return;

        foreach (KeyValuePair<string, string> item in headers)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
        }
    }

    protected Uri BindParameters(string url, Dictionary<string, string> parameters = null)
    {
        if (parameters == null)
            return new Uri(url);

        return new Uri(QueryHelpers.AddQueryString(url, parameters));
    }

    #endregion
}