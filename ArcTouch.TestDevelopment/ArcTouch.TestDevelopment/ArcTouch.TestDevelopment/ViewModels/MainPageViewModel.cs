
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
        private INavigationService _navigationService;

        public ObservableCollection<Movie> UpcomingMovies { get; set; }
        public int Page { get; set; } = 1;
        public bool HasMorePages { get; set; }

        public List<Gender> Genders { get; set; }

        #region BindablePropeties
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

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                if (SetProperty(ref _selectedMovie, value))
                    if (_selectedMovie != null)
                        MovieDetails();
            }
        }
        #endregion

        #region Commands
        public DelegateCommand NewPageCommand { get; set; }

        private void NewPage()
        {
            if (HasMorePages)
            {
                Page++;
                GetUpcomingMoviesAsync(true);
            }
        }
        #endregion

        public MainPageViewModel(IPageDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            UpcomingMovies = new ObservableCollection<Movie>();
            Genders = new List<Gender>();
            NewPageCommand = new DelegateCommand(NewPage);
            Title = "ArcTouch - Movies";
            GetUpcomingMoviesAsync();
        }

        private void MovieDetails()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("movie", SelectedMovie);
            _navigationService.NavigateAsync("MovieDetailsPage", navigationParams);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        private async Task GetUpcomingMoviesAsync(bool fromPagination = false)
        {
            try
            {
                RunningOperation = true;
                if (fromPagination == false)
                    await GetGenre();
                var response = await APIHelper.GetUpcomingMovies(Page);
                if (response.Error != null)
                {
                    await _dialogService.DisplayAlertAsync("ArcTouch - Movies", response.Error.Message, "Cancel");
                }
                else
                {
                    if (response.Response.results.Count > 0)
                    {
                        HasMorePages = true;
                        foreach (var item in response.Response.results)
                        {
                            item.movieImage = APIHelper.GetMovieImage(item.backdrop_path);
                            if (item.genre_ids.Length > 0 && Genders.Count > 0)
                                item.genderDescription = Genders.Where(x => x.id == item.genre_ids[0]).Select(x => x.name).FirstOrDefault();
                            UpcomingMovies.Add(item);
                        }
                    }
                    else
                    {
                        HasMorePages = false;
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

        private async Task GetGenre()
        {
            try
            {
                RunningOperation = true;
                var response = await APIHelper.GetGenreMovieList();
                if (response.Error != null)
                {
                    await _dialogService.DisplayAlertAsync("ArcTouch - Movies", response.Error.Message, "Cancel");
                }
                else
                {
                    foreach (var item in response.Response.genres)
                    {
                        Genders.Add(item);
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

    }
}
