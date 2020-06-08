using System;
using System.Collections.Generic;
using JokeGenerator.ConsolePresentation;

namespace JokeGenerator.Application
{
    /// <summary>
    /// Presentation contract. Any UI should contain this contract to have same behavior of the application
    /// </summary>
    internal interface IPresentationBehavior
    {
        void SetRandomName(string firstname, string lastname);
        void LoadingInformation();
        ConsoleResults MakeChoose();
        void WriteCategories(IList<string> categories);
        string InputName();
        string InputLastName();
        ConsoleResults WannaSpecifyCategory();
        string EnterCategory(IList<string> categories);
        int HowManyJokes();
        void WriteJokes(IList<string> jokes);
        int ExpectedExceptionMessage(ApplicationException appex);
    }
}