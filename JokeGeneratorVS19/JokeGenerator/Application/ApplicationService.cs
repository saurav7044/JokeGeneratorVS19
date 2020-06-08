using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokeGenerator.ConsolePresentation;
using JokesApiClient.Contracts;

namespace JokeGenerator.Application
{
    /// <summary>
    /// The responsibility of the class is to manage user actions from UI and to redirect commands to lowwer layers.
    /// The code left same as it was just moved all related with UI to presentation layer.
    /// </summary>
    internal sealed class ApplicationService
    {
        private IList<string> _categories;
        private readonly IJokeWebApiClient _jokeWebApiClient;
        private readonly IPresentationBehavior _presentationBehavior;
        
        /// <summary>
        /// This class doesn't depend of any implementation of remote facade it has and UI implementation by interfaces here is using
        /// </summary>
        public ApplicationService(
            IPresentationBehavior presentationBehavior,
            IJokeWebApiClient jokeWebApiClient)
        {
            _presentationBehavior = presentationBehavior ?? throw new ArgumentNullException(nameof(presentationBehavior));
            _jokeWebApiClient = jokeWebApiClient ?? throw new ArgumentNullException(nameof(jokeWebApiClient));
        }

        internal async Task<int> Run()
        {
            _presentationBehavior.LoadingInformation();
            if (_categories == null)
            {
                _categories = await _jokeWebApiClient.GetCategoriesAsync();
            }

            try
            {
                var key = ConsoleResults.Question;
                if (key == ConsoleResults.Question)
                {
                    var jokesParameters = new JokesParameters();
                    // initialize joke counter to 1 to by deafult display atleast 1 joke
                    jokesParameters.AddJokeAmount(1);
                    while (key != ConsoleResults.Q)
                    {
                        key = _presentationBehavior.MakeChoose();

                        switch (key)
                        {
                            case ConsoleResults.C:
                                _presentationBehavior.WriteCategories(_categories);
                                key = _presentationBehavior.WannaSpecifyCategory();
                                if (key == ConsoleResults.Y)
                                {
                                    var category = _presentationBehavior.EnterCategory(_categories);
                                    jokesParameters.AddCategory(category);
                                }
                                break;

                            case ConsoleResults.R:
                                var personInfo = await _jokeWebApiClient.GetPersonInfoAsync(1);
                                var personDto = personInfo.Single();
                                jokesParameters.AddName(personDto.Name, personDto.Surname);
                                _presentationBehavior.SetRandomName(personDto.Name, personDto.Surname);
                                break;

                            case ConsoleResults.J:
                                var jokes = await _jokeWebApiClient.GetJokesAsync(jokesParameters.JokeCount,
                                    jokesParameters.Category, jokesParameters.Name);
                                _presentationBehavior.WriteJokes(jokes.Select(_ => _.Value).ToList());
                                break;

                            case ConsoleResults.A:
                                jokesParameters.AddJokeAmount(_presentationBehavior.HowManyJokes());
                                break;

                            case ConsoleResults.N:
                                jokesParameters.AddName(
                                    _presentationBehavior.InputName(),
                                    _presentationBehavior.InputLastName());
                                break;

                            case ConsoleResults.Q:
                                break;
                        }

                    }
                }
            }
            catch (ApplicationException applicationException)
            {
                return _presentationBehavior.ExpectedExceptionMessage(applicationException);
            }

            return 0;
        }
    }
}
