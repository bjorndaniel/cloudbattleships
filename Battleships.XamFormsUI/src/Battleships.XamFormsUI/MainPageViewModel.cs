using Battleships.Model;
using Battleships.Model.DTO;
using Battleships.Model.Helpers;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Battleships.XamFormsUI
{
    public class MainPageViewModel : BaseViewModel
    {
        private ObservableCollection<MessageDTO> _messages = new ObservableCollection<MessageDTO>();
        private string _message;
        private HttpClient _httpClient;
        private string _clientId = Guid.NewGuid().ToString();
        private GameDTO _game;
        private PlayerDTO _player;
        private bool _canInitGame = true;
        private ObservableCollection<PanelDTO> _gameBoard;
        private ObservableCollection<PanelDTO> _firingBoard;
        private ConnectionSettings _connectionSettings;

        public MainPageViewModel()
        {
            _connectionSettings = new ConnectionSettings();
            SendMessageCommand = new Command(async () => { await SendMessage(); });
            PlayGameCommand = new Command(async () => { await PlayGame(); });
            FireCommand = new Command(async () => { await Fire(); });
            PlayToEndCommand = new Command(async () => { await PlayToEnd(); });
            GithubCommand = new Command(OpenUrl);
            _httpClient = new HttpClient();
            Game = GameController.CreateGameDTO();
            Player = _game.Player1;
            GameBoard = new ObservableCollection<PanelDTO>(Player.GameBoard);
            FiringBoard = new ObservableCollection<PanelDTO>(Player.FiringBoard);
        }


        public string ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        public GameDTO Game
        {
            get { return _game; }
            set { SetProperty(ref _game, value); }
        }

        public ObservableCollection<PanelDTO> GameBoard
        {
            get => _gameBoard;
            set { SetProperty(ref _gameBoard, value); }
        }

        public ObservableCollection<PanelDTO> FiringBoard
        {
            get => _firingBoard;
            set { SetProperty(ref _firingBoard, value); }
        }

        public bool CanInitGame
        {
            get { return _canInitGame; }
            set { SetProperty(ref _canInitGame, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ObservableCollection<MessageDTO> Messages
        {
            get => _messages;
            set
            {
                SetProperty(ref _messages, value);
            }
        }

        public PlayerDTO Player
        {
            get => _player;
            set { SetProperty(ref _player, value); }
        }

        public int Column { get; set; } = -1;

        public int Row { get; set; } = -1;

        public ICommand PlayGameCommand { get; private set; }

        public ICommand SendMessageCommand { get; private set; }

        public ICommand FireCommand { get; private set; }

        public ICommand PlayToEndCommand { get; private set; }
        public ICommand GithubCommand { get; private set; }


        public int TileSize => 25;

        private async Task SendMessage()
        {
            var message = new MessageDTO
            {
                User = Player.Name,
                Message = Message
            };
            var content = new StringContent(JsonConvert.SerializeObject(message));
            Message = "";
            await _httpClient.PostAsync($"{_connectionSettings.BaseUrl}{_connectionSettings.Messages}", content);
        }

        private async Task PlayGame()
        {
            var message = new InitGameDTO
            {
                User = Player.Name,
                ClientId = ClientId
            };
            var content = new StringContent(JsonConvert.SerializeObject(message));
            await _httpClient.PostAsync($"{_connectionSettings.BaseUrl}{_connectionSettings.InitGame}", content);
        }

        private async Task Fire()
        {
            var message = new FireDTO
            {
                GameId = Game.id,
                PlayerId = Player.Id,
                Column = Column,
                Row = Row
            };
            var content = new StringContent(JsonConvert.SerializeObject(message));
            await _httpClient.PostAsync($"{_connectionSettings.BaseUrl}{_connectionSettings.Fire}", content);
        }

        private async Task PlayToEnd()
        {
            var message = new FireDTO
            {
                GameId = Game.id,
            };
            var content = new StringContent(JsonConvert.SerializeObject(message));
            await _httpClient.PostAsync($"{_connectionSettings.BaseUrl}{_connectionSettings.PlayToEnd}", content);
        }

        private void OpenUrl()
        {
            Device.OpenUri(new Uri("https://github.com/bjorndaniel/cloudbattleships"));

        }
        internal void UpdateGame(UpdateGameDTO message)
        {
            bool update = message.Game.Player1.ClientId == ClientId || message.Game.Player2.ClientId == ClientId;
            if (update)
            {
                var player = message.Game.Player1.ClientId == ClientId ? message.Game.Player1 : message.Game.Player2;
                Player = player;
                GameBoard.Clear();
                player.GameBoard.ForEach(GameBoard.Add);
                FiringBoard.Clear();
                player.FiringBoard.ForEach(FiringBoard.Add);
                Game = message.Game;
                CanInitGame = false;
                Column = -1;
                Row = -1;
                if (Game.GameOver)
                {
                    MessagingCenter.Send(new WinnerMessage { Message = Game.StatusText }, string.Empty);
                }
            }

        }
        
    }

}
