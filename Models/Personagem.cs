namespace BlazorDbzPersonagem.Models;

// Espelha apenas os campos usados da resposta de GET /api/characters/{id}.
// Campos ausentes no JSON ficam com valor padrão em vez de quebrar a desserialização.
public sealed record Personagem
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Race { get; init; }
    public string? Image { get; init; }

    // A API envia "ki" como string formatada (ex.: "60.000.000"), não como número.
    public string? Ki { get; init; }

    public Planeta? OriginPlanet { get; init; }
    public IReadOnlyList<Transformacao> Transformations { get; init; } = [];
}

public sealed record Planeta
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsDestroyed { get; init; }
}

public sealed record Transformacao
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Ki { get; init; }
    public string? Image { get; init; }
}
