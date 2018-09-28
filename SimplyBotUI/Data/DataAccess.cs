using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.FluentMap;
using MySql.Data.MySqlClient;
using SimplyBotUI.Data.Mapping;
using SimplyBotUI.Models;

namespace SimplyBotUI.Data
{
    internal class DataAccess
    {
        private readonly string _mapInfoConnectionString;
        private readonly string _playerRanksConnectionString;
        private MySqlConnection _mapInfoConnection;
        private MySqlConnection _playerRanksConnection;

        internal DataAccess()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new HighscoreMap());
                config.AddMap(new DetailsMap());
                config.AddMap(new JumpRankMap());
                config.AddMap(new HightowerPlayerMap());
                config.AddMap(new PersonalTimeMap());
            });
            var connectionStringLines = File.ReadAllLines(Constants.DatabaseInfoPath);
            _mapInfoConnectionString = connectionStringLines[0];
            _playerRanksConnectionString = connectionStringLines[1];
        }

        private async Task OpenMapInfoConnection()
        {
            _mapInfoConnection = new MySqlConnection(_mapInfoConnectionString);
            await _mapInfoConnection.OpenAsync();
        }


        private async Task CheckMapInfoConnection()
        {
            if (_mapInfoConnection == null || _mapInfoConnection.State == ConnectionState.Closed)
                await OpenMapInfoConnection();
        }

        private async Task OpenPlayerRanksConnection()
        {
            _playerRanksConnection = new MySqlConnection(_playerRanksConnectionString);
            await _playerRanksConnection.OpenAsync();
        }


        private async Task CheckPlayerRankConnection()
        {
            if (_playerRanksConnection == null || _playerRanksConnection.State == ConnectionState.Closed)
                await OpenPlayerRanksConnection();
        }

        internal void Close()
        {
            // TODO: Find a closing event to hook this to
            _mapInfoConnection.Close();
        }

        #region MapInfo Queries

        internal async Task<List<dynamic>> ExecuteMapInfoQuery(string query)
        {
            await CheckMapInfoConnection();

            return (await _mapInfoConnection.QueryAsync(query)).ToList();
        }

        internal async Task<List<HighscoreModel>> GetMapTimes(int classValue, string mapName)
        {
            await CheckMapInfoConnection();

            var query =
                $@"select * from highscores where timestamp>0 and class={classValue} and map like '%{mapName}%'";

            return (await _mapInfoConnection.QueryAsync<HighscoreModel>(query)).ToList();
        }

        internal async Task<string> GetFullMapName(string partialName)
        {
            await CheckMapInfoConnection();
            var query = $@"select map from details where map like '%{partialName}%'";
            return (await _mapInfoConnection.QueryAsync<string>(query)).First();
        }

        internal async Task<List<string>> GetMapsContainingName(string partialName)
        {
            await CheckMapInfoConnection();
            var query = $@"select map from details where map like '%{partialName}%'";
            var output = (await _mapInfoConnection.QueryAsync<DetailsModel>(query)).ToList().ConvertAll(x => x.Map)
                .ToList();
            if (output.Count < 30) return output;
            var extraCount = output.Count - 30;
            output = output.Take(30).ToList();
            output.Add($"{extraCount} more...");
            return output;
        }

        internal async Task<HighscoreModel> GetMapTime(int classValue, string mapName, string user)
        {
            await CheckMapInfoConnection();

            var query =
                $@"select * from highscores where timestamp>0 and class={classValue} and map like '%{mapName}%' and name like '%{user}%'";

            return (await _mapInfoConnection.QueryAsync<HighscoreModel>(query)).OrderBy(time => time.RunTime)
                .FirstOrDefault();
        }

        internal async Task<DetailsModel> GetMapDetails(string partialName)
        {
            await CheckMapInfoConnection();
            var query = $@"select * from details where map like '%{partialName}%'";
            var result = (await _mapInfoConnection.QueryAsync<DetailsModel>(query)).First();
            return result;
        }

        internal async Task<List<JumpRankModel>> GetTopDemo(int count)
        {
            return await GetTop("dem", count);
        }

        internal async Task<List<JumpRankModel>> GetTopSolly(int count)
        {
            return await GetTop("sol", count);
        }

        internal async Task<List<JumpRankModel>> GetTopConc(int count)
        {
            return await GetTop("conc", count);
        }

        internal async Task<List<JumpRankModel>> GetTopEngi(int count)
        {
            return await GetTop("eng", count);
        }

        internal async Task<List<JumpRankModel>> GetTopPyro(int count)
        {
            return await GetTop("pyro", count);
        }

        internal async Task<List<JumpRankModel>> GetTopOverall(int count)
        {
            return await GetTop("general", count);
        }

        private async Task<List<JumpRankModel>> GetTop(string type, int count)
        {
            await CheckMapInfoConnection();

            var query = $@"select * from JumpRanks order by {type} desc limit {count}";
            var result = await _mapInfoConnection.QueryAsync<JumpRankModel>(query);
            return result.ToList();
        }

        internal async Task<List<HighscoreModel>> GetRecentRecords(int count)
        {
            await CheckMapInfoConnection();

            var query =
                $@"select * from highscores where timestamp>0 order by timestamp desc limit {count}";

            return (await _mapInfoConnection.QueryAsync<HighscoreModel>(query)).ToList();
        }

        #endregion

        #region PlayerRank Queries

        internal async Task<List<dynamic>> ExecutePlayRanksQuery(string query)
        {
            await CheckPlayerRankConnection();

            return (await _playerRanksConnection.QueryAsync(query)).ToList();
        }

        internal async Task<List<HightowerPlayerModel>> GetTopHightowerRank(int count)
        {
            await CheckPlayerRankConnection();

            var query = $@"select * from players order by points desc limit {count}";

            return (await _playerRanksConnection.QueryAsync<HightowerPlayerModel>(query)).ToList();
        }

        #endregion
    }
}