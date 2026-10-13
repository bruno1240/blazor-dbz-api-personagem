# blazor-dbz-api-personagem

Lista de exercícios — Desenvolvimento Web (Prof. Daniel Henrique Matos de Paiva).

Aplicação Blazor (.NET 10, Interactive Server) que pede o id de um personagem, consome a
[Dragon Ball API](https://dragonball-api.com) (`GET /api/characters/{id}`) e exibe **Nome**,
**Planeta** e **Transformações**.

## Estrutura

| Arquivo                                  | Responsabilidade                                        |
|------------------------------------------|---------------------------------------------------------|
| `Models/Personagem.cs`                   | Records que mapeiam o JSON da API                       |
| `Services/DragonBallApiService.cs`       | HttpClient tipado: chamada HTTP e tratamento de erros   |
| `Components/Pages/BuscarPersonagem.razor`   | Formulário do id, validação e exibição do resultado     |
| `appsettings.json`                       | URL base da API (`DragonBallApi:BaseUrl`)               |

## Comportamento

- Id validado (obrigatório, entre 1 e 10000) antes de chamar a API.
- Id inexistente: mensagem "Nenhum personagem encontrado".
- Falha de rede ou timeout (10 s): mensagem amigável na tela, com detalhe no log.
- Uma nova busca cancela a requisição anterior que ainda estiver em andamento.

## Como rodar

Requer .NET SDK 10.0.

```bash
dotnet watch run
```

Acesse a URL exibida no terminal (ex.: `http://localhost:5080`), informe um id (ex.: 1 = Goku) e clique em **Buscar**.
