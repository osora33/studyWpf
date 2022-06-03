using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfNaverMovieFinder.models;

namespace WpfNaverMovieFinder
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        List<YoutubeItem> youtubeItems;     //Youtube API 검색결과 담을 리스트

        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string movieName) : this()
        {
            lblMovieName.Content = $"{movieName} 예고편";      //
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            youtubeItems = new List<YoutubeItem>();
            SearchYoutubeApi();
        }

        private async void SearchYoutubeApi()
        {
            await LoadDataCollection();
            lsvYoutubeSearch.ItemsSource = youtubeItems;
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyD13E6Fyx0Cgq2uzfnnfuzT8VxgdAqVavw",
                    ApplicationName = this.GetType().ToString()
                });

            var request = youtubeService.Search.List("snippet");
            request.Q = lblMovieName.Content.ToString();
            request.MaxResults = 10;

            var response = await request.ExecuteAsync();
            //MessageBox.Show(response.ToString());

            foreach (var item in response.Items)
            {
                if (item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem(
                        item.Snippet.Title, item.Snippet.ChannelTitle,
                        //$"https://www.youtube.com/watch?v={item.Id.VideoId}");
                        $"https://www.youtube-nocookie.com/embed/{item.Id.VideoId}");

                    //썸네일 이미지
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url,
                        UriKind.RelativeOrAbsolute));
                    youtubeItems.Add(youtube);
                }
            }
        }

        private void lsvYoutubeSearch_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(lsvYoutubeSearch.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유튜브", "예고편을 볼 영화를 선택하세요");
                return;
            }
            if (lsvYoutubeSearch.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유튜브", "예고편을 하나만 선택하세요");
                return;
            }

            if (lsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = lsvYoutubeSearch.SelectedItem as YoutubeItem;
                brsYoutubeWatch.Address = video.URL;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            brsYoutubeWatch.Address = string.Empty;
            brsYoutubeWatch.Dispose();      //리소스 해제
        }
    }
}