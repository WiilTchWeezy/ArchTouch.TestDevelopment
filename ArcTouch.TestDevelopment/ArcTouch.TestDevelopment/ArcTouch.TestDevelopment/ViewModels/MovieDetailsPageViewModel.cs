using ArcTouch.TestDevelopment.Service.ApiObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcTouch.TestDevelopment.ViewModels
{
    public class MovieDetailsPageViewModel : BindableBase, INavigationAware
    {
        private Movie _selectedMovie;
        public MovieDetailsPageViewModel()
        {

        }

        private string _movieTitle;
        public string MovieTitle
        {
            get { return _movieTitle; }
            set { SetProperty(ref _movieTitle, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            _selectedMovie = (Movie)parameters["movie"];
            MovieTitle = _selectedMovie.title;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }
    }
}
