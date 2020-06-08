using System;

namespace JokeGenerator.Application
{
    public sealed class JokesParameters
    {
        public string Name { get; private set; }
        public int JokeCount { get; private set; }
        public string Category { get; private set; }

        public JokesParameters AddJokeAmount(int jokeCount)
        {
            if (0 > jokeCount)
                throw new ArgumentOutOfRangeException(nameof(jokeCount));

            JokeCount = jokeCount;
            return this;
        }

        public JokesParameters AddCategory(string category)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
            return this;
        }

        public JokesParameters AddName(string firstName, string lastName)
        {
            Name = string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName)
                ? lastName
                : string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(firstName)
                    ? firstName
                    : $"{firstName} {lastName}";
            return this;
        }
    }
}