# Documentação Técnica

## Visão Geral do Projeto

A aplicação **MixBalancer** é uma API desenvolvida em .NET 8 com objetivo de gerenciar jogadores, times e partidas, provendo funcionalidades de autenticação, criação de times, organização de partidas com balanceamento automático, registro de placares e status. Ela segue uma arquitetura limpa, empregando camadas bem definidas e separando preocupações de domínio, aplicação, infraestrutura e apresentação (API).

## Tecnologias Utilizadas

- **Plataforma e Framework**: .NET 8
- **Linguagem**: C#
- **Banco de Dados**: PostgreSQL  
  - Acesso e Mapeamento: Entity Framework Core
- **Autenticação e Autorização**: JWT (JSON Web Tokens)
- **Documentação da API**: Swagger (OpenAPI)
- **Arquitetura**: Clean Architecture  
  - **Domain**: Contém as entidades de negócio (Player, Team, Match) e interfaces de contratos.
  - **Application**: Contém serviços, lógica de negócio, DTOs e casos de uso.
  - **Infrastructure**: Implementação do contexto de banco, repositórios e providers.
  - **API**: Controladores (Controllers) e endpoints expostos via HTTP.

## Entidades e Relacionamentos

**Player (Jogador)**  
- Campos típicos: `Id`, `UserName`, `Email`, `PasswordHash`, `SkillLevel` (opcional para balanceamento)  
- Cada usuário autenticado representa um jogador.

**Team (Time)**  
- Campos típicos: `Id`, `Name`, `OwnerId` (chave estrangeira para Player)  
- Um jogador (Owner) pode criar múltiplos times.  
- Um time pode ter múltiplos jogadores (relação muitos-para-muitos entre Player e Team).

**Match (Partida)**  
- Campos típicos: `Id`, `TeamAId`, `TeamBId`, `ScoreTeamA`, `ScoreTeamB`, `Status` (Scheduled, Ongoing, Finished), `MatchDate`.  
- Uma partida envolve dois ou mais times e registra resultados e status.

**Relações:**
- **Player - Team**: muitos-para-muitos (um jogador pode estar em vários times; um time pode ter vários jogadores).  
- **Team - Match**: um-para-muitos (um time pode participar de várias partidas).  
- **Player - Match**: relação indireta via Time (jogadores participam de partidas através dos times).

## Diagrama de Relacionamentos (Exemplo Simplificado)

```plaintext
  Player
    |\
    | \ (many-to-many)
    |  \
    |   Team -------- Match
    |   /               ^
    |  /                |
    | / (many-to-many) 
    v/                  |
   (Players participate in Matches via Teams)
```

## Endpoints da API

**Autenticação & Cadastro**  
- `POST /api/auth/register` - Registra um novo jogador.  
- `POST /api/auth/login` - Autentica um jogador e retorna um JWT.

**Players**  
- `GET /api/players/{id}` - Obtém detalhes de um jogador (requer autenticação).

**Teams (Times)**  
- `POST /api/teams` - Cria um novo time (requer JWT do jogador proprietário).
- `GET /api/teams/{id}` - Obtém detalhes de um time.
- `PUT /api/teams/{id}` - Atualiza dados do time (somente proprietário).
- `DELETE /api/teams/{id}` - Remove um time (somente proprietário).
- `POST /api/teams/{id}/add-player` - Adiciona um jogador a um time.
- `POST /api/teams/{id}/remove-player` - Remove um jogador de um time.

**Matches (Partidas)**  
- `POST /api/matches` - Cria uma nova partida. A payload inclui times participantes.  
- `GET /api/matches/{id}` - Obtém detalhes da partida, status e placar.  
- `PUT /api/matches/{id}` - Atualiza o status ou placar da partida.  
- `GET /api/matches` - Lista partidas existentes.

## Fluxo de Operação (Exemplo Básico)

1. **Registro do Jogador**: `POST /api/auth/register`.
2. **Criação de Time**: `POST /api/teams`.
3. **Adição de Jogadores ao Time**: `POST /api/teams/{id}/add-player`.
4. **Organização de Partida**: `POST /api/matches` (fornece times, a API balanceia automaticamente).
5. **Finalização da Partida**: `PUT /api/matches/{id}` para enviar placar final.

## Configuração do Ambiente

**Requisitos:**
- .NET 8 SDK instalado.
- Banco de Dados PostgreSQL.

**Passos:**
1. Clonar o repositório do projeto.
2. Configurar a string de conexão no `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=mixbalancer_db;Username=postgres;Password=postgres"
   }
    ```
3. Configurar variáveis de ambiente para JWT:
    - `JWT__SecretKey`
    - `JWT__Issuer`
    - `JWT__Audience`

4. Executar migrações:
    - `dotnet ef database update`

# Documentação Não Técnica

## Descrição do Público-Alvo
A **MixBalancer** é destinada a jogadores que desejam organizar partidas equilibradas, onde equipes são balanceadas com base no nível de habilidade dos membros. O público-alvo inclui tanto jogadores casuais quanto organizadores de campeonatos amadores, que precisam gerenciar jogadores, times e resultados sem complicações.

## Funcionalidades Principais
- **Cadastro e Autenticação de Jogadores:** Permite que qualquer pessoa crie uma conta e acesse a aplicação, tornando-se um jogador.
- **Criação e Gerenciamento de Times:** Jogadores podem criar e editar seus times, convidar outros jogadores e formar grupos de jogo.
- **Organização de Partidas Balanceadas:** A API oferece um algoritmo de balanceamento para distribuir jogadores entre equipes de modo justo, evitando que um time fique muito mais forte que o outro.
- **Registro de Resultados e Status das Partidas:** Ao término de cada partida, placares e status podem ser salvos, gerando um histórico de desempenho.

## Diferenciais
- **Simplicidade:** Uma API direta e fácil de integrar com qualquer aplicação front-end, como websites e aplicativos mobile.
- **Balanceamento Automático:** Algoritmo interno que garante partidas mais justas, considerando nível de habilidade dos jogadores.
- **Fluxo Intuitivo:** Do registro do jogador à conclusão de uma partida, as etapas são claras e fáceis de seguir.

## Possíveis Melhorias Futuras
- **Sistema de Ranking:** Implementar um ranking dos jogadores baseado no histórico de vitórias, derrotas e desempenho individual.
- **Estatísticas Avançadas:** Oferecer métricas detalhadas (por exemplo: K/D ratio, pontuação média por partida, performance do time).
- **Integração com Sistemas de Comunicação:** Conectar a API ao Discord ou outras plataformas para facilitar a comunicação entre membros do time.
