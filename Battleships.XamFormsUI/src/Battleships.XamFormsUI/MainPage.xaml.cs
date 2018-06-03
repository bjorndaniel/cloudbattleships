using Battleships.Model.Helpers;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.XamFormsUI
{
    public partial class MainPage : ContentPage
    {

        private MainPageViewModel _viewModel;
        private HttpClient _client;
        private HubConnection _hubConnection;
        private List<Label> _gameBoardLabels = new List<Label>();
        private List<Label> _firingBoardLabels = new List<Label>();
        private List<Grid> _gameBoardBox = new List<Grid>();
        private List<Grid> _firingBoardBox = new List<Grid>();
        private ConnectionSettings _connectionSettings;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
            _client = new HttpClient();
            _connectionSettings = new ConnectionSettings();
            _viewModel.GameBoard.CollectionChanged += GameBoard_CollectionChanged;
            _viewModel.FiringBoard.CollectionChanged += FiringBoard_CollectionChanged;
            MessagingCenter.Subscribe<WinnerMessage>(this, string.Empty, async message =>
            {
                await DisplayAlert("Game ended!", message.Message, "Ok");
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SetupGrids();
            await InitSignalR();
        }

        private void SetupGrids()
        {
            for (var i = 0; i < 10; i++)
            {
                GameBoard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                GameBoard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25) });
                FiringBoard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                FiringBoard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25) });
            }

            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    var g = new Grid { ClassId = $"GG_{row}_{col}", BackgroundColor = Color.White};
                    var text = new Label { ClassId = $"Label_{row}_{col}", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                    _gameBoardLabels.Add(text);
                    _gameBoardBox.Add(g);
                    g.Children.Add(text);
                    GameBoard.Children.Add(g, col - 1, row - 1);
                    g = new Grid { ClassId = $"FG_{row}_{col}", BackgroundColor = Color.White};
                    text = new Label { ClassId = $"FLabel_{row}_{col}", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                    _firingBoardLabels.Add(text);
                    _firingBoardBox.Add(g);
                    g.Children.Add(text);
                    var tgr = new TapGestureRecognizer
                    {
                        NumberOfTapsRequired = 1
                    };
                    tgr.Tapped += Tgr_Tapped;
                    g.GestureRecognizers.Add(tgr);
                    FiringBoard.Children.Add(g, col - 1, row - 1);
                }
            }
        }

        private void GameBoard_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var p in _viewModel.GameBoard)
            {
                var l = _gameBoardLabels.First(_ => _.ClassId == $"Label_{p.Row}_{p.Column}");
                l.Text = p.Status;
                var g = _gameBoardBox.First(_ => _.ClassId == $"GG_{p.Row}_{p.Column}");
                g.BackgroundColor = p.IsHit ? Color.Red : Color.White;
            }
        }

        private void FiringBoard_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var p in _viewModel.FiringBoard)
            {
                var l = _firingBoardLabels.First(_ => _.ClassId == $"FLabel_{p.Row}_{p.Column}");
                l.Text = p.Status;
                var g = _firingBoardBox.First(_ => _.ClassId == $"FG_{p.Row}_{p.Column}");
                g.BackgroundColor = p.Status == "X" ? Color.Green : Color.White;
            }
        }

        private async Task InitSignalR()
        {
            try
            {
                var connString = await _client.GetStringAsync($"{_connectionSettings.BaseUrl}{_connectionSettings.Negotiate}");
                var connectionInfo = JsonConvert.DeserializeObject<NegotiateDTO>(connString);
                var connectionBuilder = new HubConnectionBuilder()
                    .WithUrl(
                        connectionInfo.Endpoint,
                        options =>
                        {
                            options.AccessTokenProvider = async () =>
                            {
                                await Task.Delay(500); return connectionInfo.AccessKey;
                            };
                        }
                    );
                _hubConnection = connectionBuilder.Build();
                _hubConnection.On<MessageDTO>("ReceiveMessage", (message) =>
                {
                    Device.BeginInvokeOnMainThread(() => { _viewModel.Messages.Insert(0, message); });
                });
                _hubConnection.On<UpdateGameDTO>("GameUpdated", (message) =>
                {
                    Device.BeginInvokeOnMainThread(() => { _viewModel.UpdateGame(message); });
                });
                _hubConnection.Closed += (e) => { _hubConnection = null; return null; };
                await _hubConnection.StartAsync();
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => { _viewModel.Messages.Insert(0, new MessageDTO { User = "System", Message = e.ToString() }); });
            }
        }

        private Frame CreateTapFrame(int row, int col)
        {
            var frame = new Frame()
            {
                WidthRequest = 25,
                HeightRequest = 25,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HasShadow = false,
                ClassId = $"{row};{col}"
            };
            var tgr = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1
            };
            tgr.Tapped += Tgr_Tapped;
            frame.GestureRecognizers.Add(tgr);
            return frame;

        }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
            if (sender is Grid grid)
            {
                _firingBoardBox.ForEach(_ => _.BackgroundColor = Color.White);
                var row = grid.ClassId.Split(new[] { '_' })[1];
                var col = grid.ClassId.Split(new[] { '_' })[2];
                _viewModel.Column = int.Parse(col);
                _viewModel.Row = int.Parse(row);
                grid.BackgroundColor = Color.Yellow;
            }
        }
    }
}
