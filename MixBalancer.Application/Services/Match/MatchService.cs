using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.Dtos.Player;
using MixBalancer.Application.Dtos.Team;
using MixBalancer.Application.DTOs;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Enums;
using MixBalancer.Domain.Interfaces;
using System.Net.Http.Json;

namespace MixBalancer.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly HttpClient _httpClient;

        public MatchService(IMatchRepository matchRepository, ITeamRepository teamRepository, IPlayerRepository playerRepository, HttpClient httpClient)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _httpClient = httpClient;
        }

        public async Task<ServiceResult> CreateMatchAsync(CreateMatchDto model)
        {
            var teamA = await _teamRepository.GetByIdAsync(model.TeamAId);
            var teamB = await _teamRepository.GetByIdAsync(model.TeamBId);

            if (teamA == null || teamB == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "One or both teams not found." };

            var match = new Match
            {
                Id = Guid.NewGuid(),
                TeamAId = model.TeamAId,
                TeamBId = model.TeamBId,
                Date = model.Date,
                Status = MatchStatus.Created,
                ScoreTeamA = 0,
                ScoreTeamB = 0,
                ManagedByUserId = model.ManagedByUserId  // Atribuindo o owner da partida
            };

            await _matchRepository.AddAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<MatchResultDto>> GetMatchByIdAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return new ServiceResult<MatchResultDto> { IsSuccess = false, ErrorMessage = "Match not found." };

            var matchDto = new MatchResultDto
            {
                Id = match.Id,
                Date = match.Date,
                Status = match.Status,
                ScoreTeamA = match.ScoreTeamA,
                ScoreTeamB = match.ScoreTeamB,
                TeamA = new TeamResultDto
                {
                    Id = match.TeamA.Id,
                    Name = match.TeamA.Name,
                    Players = match.TeamA.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = match.TeamA.AverageSkillLevel,
                    ManagedByUserId = match.TeamA.ManagedByUserId
                },
                TeamB = new TeamResultDto
                {
                    Id = match.TeamB.Id,
                    Name = match.TeamB.Name,
                    Players = match.TeamB.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = match.TeamB.AverageSkillLevel,
                    ManagedByUserId = match.TeamB.ManagedByUserId
                }
            };

            return new ServiceResult<MatchResultDto> { IsSuccess = true, Data = matchDto };
        }

        public async Task<ServiceResult> UpdateMatchAsync(Guid id, UpdateMatchDto model)
        {
            var match = await _matchRepository.GetByIdAsync(id);

            if (model.Status == MatchStatus.Finished)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Unable to update. Match is already finished." };

            if (model.Status == MatchStatus.Cancelled)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Unable to update. Match is cancelled." };

            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            if (model.Status != match.Status)
                match.Status = model.Status;

            if (model.ScoreTeamA.HasValue)
                match.ScoreTeamA = model.ScoreTeamA.Value;

            if (model.ScoreTeamB.HasValue)
                match.ScoreTeamB = model.ScoreTeamB.Value;

            await _matchRepository.UpdateAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<IEnumerable<MatchResultDto>>> GetAllMatchesAsync(MatchStatus? status, DateTime? date)
        {
            var matches = await _matchRepository.GetAllAsync(status, date);

            var matchDtos = matches.Select(m => new MatchResultDto
            {
                Id = m.Id,
                Date = m.Date,
                Status = m.Status,
                ScoreTeamA = m.ScoreTeamA,
                ScoreTeamB = m.ScoreTeamB,
                TeamA = new TeamResultDto
                {
                    Id = m.TeamA.Id,
                    Name = m.TeamA.Name,
                    Players = m.TeamA.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = m.TeamA.AverageSkillLevel,
                    ManagedByUserId = m.TeamA.ManagedByUserId
                },
                TeamB = new TeamResultDto
                {
                    Id = m.TeamB.Id,
                    Name = m.TeamB.Name,
                    Players = m.TeamB.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = m.TeamB.AverageSkillLevel,
                    ManagedByUserId = m.TeamB.ManagedByUserId
                }
            });

            return new ServiceResult<IEnumerable<MatchResultDto>> { IsSuccess = true, Data = matchDtos };
        }

        public async Task<ServiceResult> CancelMatchAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            match.Status = MatchStatus.Cancelled;

            await _matchRepository.UpdateAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<IEnumerable<MatchHistoryDto>>> GetPlayerMatchHistory(Guid playerId)
        {
            var matchHistory = await _matchRepository.GetPlayerMatchHistory(playerId);
            if (matchHistory == null)
                return new ServiceResult<IEnumerable<MatchHistoryDto>> { IsSuccess = false, ErrorMessage = "Match History not found." };

            var matchHistoryDto = matchHistory.Select(mh => new MatchHistoryDto
            {
                Date = mh.Date,
                KD = mh.KD,
                Map = mh.Map,
                Result = mh.Result
            });

            return new ServiceResult<IEnumerable<MatchHistoryDto>> { IsSuccess = true, Data = matchHistoryDto };
        }

        public async Task<ServiceResult> AddPlayerToMatchAsync(Guid matchId, AddPlayerDto model)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            // Verificar se o time é válido (se o time A ou time B existe)
            var team = (model.TeamId == match.TeamAId) ? match.TeamA : (model.TeamId == match.TeamBId) ? match.TeamB : null;
            if (team == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Team not found in the match." };

            // Verificar se o jogador já está no time
            if (team.Players.Any(p => p.Id == model.PlayerId))
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player already in the team." };

            var player = await _playerRepository.GetByIdAsync(model.PlayerId);
            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found." };

            // Adicionar jogador ao time
            team.Players.Add(player);
            await _matchRepository.UpdateAsync(match);  // Salvar as alterações na partida

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> BalanceTeamsAsync(Guid matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            // Certifique-se de que a partida tenha jogadores suficientes em ambas as equipes
            if (match.TeamA.Players.Count == 0 || match.TeamB.Players.Count == 0)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Teams need to have players before balancing." };

            // Combine os jogadores dos dois times e classifique-os por `SkillLevel`
            var allPlayers = match.TeamA.Players.Concat(match.TeamB.Players)
                .OrderByDescending(p => p.SkillLevel)
                .ToList();

            // Balanceamento: A ideia aqui é distribuir os jogadores de maneira equilibrada
            // Alternando entre os dois times para balancear as habilidades
            var teamAPlayers = new List<Player>();
            var teamBPlayers = new List<Player>();

            for (int i = 0; i < allPlayers.Count; i++)
            {
                if (i % 2 == 0)
                    teamAPlayers.Add(allPlayers[i]);
                else
                    teamBPlayers.Add(allPlayers[i]);
            }

            // Atribuindo os jogadores balanceados aos times
            match.TeamA.Players = teamAPlayers;
            match.TeamB.Players = teamBPlayers;

            // Atualizar os times na partida
            await _matchRepository.UpdateAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> SetMatchIdCS2Async(Guid matchId, string matchIdCS2)
        {
            Match match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            // 1. Verifique se o Match ID CS2 está válido e se corresponde a uma partida real no CS2 via Steam API
            var steamMatchData = await GetMatchDataFromSteam(matchIdCS2);
            if (steamMatchData == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Failed to retrieve match data from CS2." };

            // 2. Atribua o MatchIdCS2 à partida no MixBalancer
            match.MatchIdCS2 = matchIdCS2;

            // 3. Atualize a partida no banco de dados
            await _matchRepository.UpdateAsync(match);

            // 4. (Opcional) Envie informações de volta à Steam se necessário (se houver algum processo de registro adicional)
            // Nesse ponto, se necessário, você pode enviar dados adicionais à Steam, como o status da partida ou qualquer outra coisa.

            return new ServiceResult { IsSuccess = true };
        }

        // Função para buscar os dados da partida na Steam API
        private async Task<object> GetMatchDataFromSteam(string matchIdCS2)
        {
            var steamApiUrl = $"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1?match_id={matchIdCS2}&key={"teste"}";

            var response = await _httpClient.GetAsync(steamApiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Here, you would process the response from the Steam API
                var matchData = await response.Content.ReadFromJsonAsync<object>();
                return matchData;
            }

            return null;
        }
    }
}