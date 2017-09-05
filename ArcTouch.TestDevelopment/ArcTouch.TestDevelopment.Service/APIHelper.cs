using ArcTouch.TestDevelopment.Service.ApiObjects;
using ArcTouch.TestDevelopment.Service.ApiResponses;
using ArcTouch.TestDevelopment.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArcTouch.TestDevelopment.Service
{
    public static class APIHelper
    {
        private const string apiAddress = "https://api.themoviedb.org/3";
        private const string apiImageAddress = "https://image.tmdb.org/t/p/w500";
        private const string apiKey = "1f54bd990f1cdfb230adb312546d765d";

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static async Task<TResponse> GetResponseAsync<TResponse, TObject>(HttpResponseMessage httpResponseMessage) where TResponse : APIResponseBase<TObject>, new() where TObject : new()
        {
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException("Sua sessão expirou");

            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                var error = await httpResponseMessage.Content.ReadAsAsync<Error>();
                throw new NotFoundException(error.Message);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = await httpResponseMessage.Content.ReadAsAsync<Error>();
                throw new BadRequestException(error.Message);
            }

            try
            {
                if (httpResponseMessage.IsSuccessStatusCode == false)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        return new TResponse()
                        {
                            Response = new TObject()
                        };
                    }

                    return new TResponse
                    {
                        Error = await httpResponseMessage.Content.ReadAsAsync<Error>()
                    };

                }
                else
                {
                    return new TResponse
                    {
                        Response = await httpResponseMessage.Content.ReadAsAsync<TObject>(),
                    };
                }
            }
            catch (Exception ex)
            {
                var response = new TResponse
                {
                    Error = new Error { Message = ex.Message }
                };
                return response;
            }
        }

        private static TResponse GetErrorResponse<TResponse, TObject>(Exception ex) where TResponse : APIResponseBase<TObject>, new() where TObject : new()
        {
            if (ex.Message.ToLower().Contains("network is unreachable") || ex.Message.ToLower().Contains("nameresolution"))
            {
                return new TResponse
                {
                    Error = new Error { Message = "Por favor, verifique sua conexão com a internet." }
                };
            }
            return new TResponse
            {
                Error = new Error { Message = ex.Message }
            };
        }

        public static async Task<UpcomingMoviesResponse> GetUpcomingMovies(int page)
        {
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync(apiAddress + "/movie/upcoming?api_key=" + apiKey + "&page=" + page);
                return await GetResponseAsync<UpcomingMoviesResponse, UpcomingMovies>(response);
            }
        }

        public static async Task<GenreTvListResponse> GetGenreMovieList()
        {
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync(apiAddress + "/genre/movie/list?api_key=" + apiKey);
                return await GetResponseAsync<GenreTvListResponse, GenreTvList>(response);
            }
        }

        public static Uri GetMovieImage(string filepath)
        {
            return new Uri(apiImageAddress + filepath);
        }
    }
}
