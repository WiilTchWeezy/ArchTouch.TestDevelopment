
using ArcTouch.TestDevelopment.Service;
using ArcTouch.TestDevelopment.Service.ApiObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ArcTouch.TestDevelopment.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private IPageDialogService _dialogService;
        public ObservableCollection<Movie> UpcomingMovies { get; set; }

        private bool _runningOperation;
        public bool RunningOperation
        {
            get { return _runningOperation; }
            set { SetProperty(ref _runningOperation, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainPageViewModel(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            UpcomingMovies = new ObservableCollection<Movie>();
            GetUpcomingMovies();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        private async Task GetUpcomingMovies()
        {
            try
            {
                RunningOperation = true;
                var response = await APIHelper.GetUpcomingMovies();
                if (response.Error != null)
                {
                    await _dialogService.DisplayAlertAsync("ArcTouch - Movies", response.Error.Message, "Cancel");
                }
                else
                {
                    foreach (var item in response.Response.results)
                    {
                        item.movieImage = APIHelper.GetMovieImage(item.backdrop_path);
                        UpcomingMovies.Add(item);
                    }
                }
                RunningOperation = false;
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync("ArcTouch - Movies", ex.Message, "Cancel");
            }
            finally
            {
                RunningOperation = false;
            }
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
    }
}
