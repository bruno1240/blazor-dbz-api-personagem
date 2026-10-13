using System.Net;
using System.Text.Json;
using BlazorDbzPersonagem.Models;

namespace BlazorDbzPersonagem.Services;

public sealed class DragonBallApiException(string message, Exception? inner = null)
    : Exception(message, inner);

public sealed class DragonBallApiService(HttpClient http, ILogger<DragonBallApiService> logger)
{
    /// <summary>
    /// Busca um personagem pelo id.
    /// Retorna null quando o personagem não existe; lança <see cref="DragonBallApiException"/>
    /// para falhas de infraestrutura (rede, timeout, resposta inválida).
    /// </summary>
    public async Task<Personagem?> ObterPersonagemAsync(int id, CancellationToken ct = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        try
        {
            using var resposta = await http.GetAsync($"characters/{id}", ct);

            // A API responde 400 ou 404 para ids inexistentes; ambos significam "não encontrado" para o usuário.
            if (resposta.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.BadRequest)
            {
                return null;
            }

            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadFromJsonAsync<Personagem>(ct)
                ?? throw new DragonBallApiException("A API retornou uma resposta vazia.");
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Cancelamento pedido pelo chamador (nova busca ou página fechada): não é erro.
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Timeout ao consultar o personagem {Id}", id);
            throw new DragonBallApiException("A API demorou demais para responder. Tente novamente.", ex);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Falha HTTP ao consultar o personagem {Id}", id);
            throw new DragonBallApiException("Não foi possível conectar à API do Dragon Ball.", ex);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Resposta inválida da API para o personagem {Id}", id);
            throw new DragonBallApiException("A API retornou dados em um formato inesperado.", ex);
        }
    }
}
