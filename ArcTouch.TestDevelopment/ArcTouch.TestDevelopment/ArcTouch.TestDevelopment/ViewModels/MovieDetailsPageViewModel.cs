using ArcTouch.TestDevelopment.Service;
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

        private Uri _movieImage;
        public Uri MovieImage
        {
            get { return _movieImage; }
            set { SetProperty(ref _movieImage, value); }
        }

        private string _movieReview;
        public string MovieReview
        {
            get { return _movieReview; }
            set { SetProperty(ref _movieReview, value); }
        }//MovieReleaseDate

        private string _movieReleaseDate;
        public string MovieReleaseDate
        {
            get { return _movieReleaseDate; }
            set { SetProperty(ref _movieReleaseDate, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            _selectedMovie = (Movie)parameters["movie"];
            MovieTitle = _selectedMovie.title;
            MovieImage = APIHelper.GetMovieImage(_selectedMovie.poster_path);
            MovieReview = _selectedMovie.overview;
            MovieReleaseDate = _selectedMovie.release_date;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }
    }
}
