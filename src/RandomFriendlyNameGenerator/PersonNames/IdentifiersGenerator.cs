﻿using System;
using System.Collections.Generic;
using System.Text;
using RandomFriendlyNameGenerator.Data;
using RandomFriendlyNameGenerator.RandomIndex;

namespace RandomFriendlyNameGenerator
{
    /// <summary>
    /// Generates non-human identifier strings
    /// </summary>
    public class IdentifiersGenerator
    {
        internal IdentifiersGenerator(List<string> adjectives, List<string> animals, List<string> firstNames, List<string> nationalities, List<string> nouns, List<string> professions)
        {
            this.adjectives = adjectives;
            this.animals = animals;
            this.firstNames = firstNames;
            this.nationalities = nationalities;
            this.nouns = nouns;
            this.professions = professions;
        }

        internal IdentifiersGenerator()
        {
            this.adjectives = Adjectives.Values;
            this.animals = Animals.Values;
            this.firstNames = FirstNames.Values;
            this.nationalities = Nationalities.Values;
            this.nouns = Nouns.Values;
            this.professions = Professions.Values;
        }

        private readonly IGenerateRandomIndex randomIndex = new RandomBasedGenerator();
        private readonly List<string> adjectives;
        private readonly List<string> animals;
        private readonly List<string> firstNames;
        private readonly List<string> nationalities;
        private readonly List<string> nouns;
        private readonly List<string> professions;

        public string Get(IdentifierComponents components = IdentifierComponents.Adjective | IdentifierComponents.Profession, NameOrderingStyle orderStyle = NameOrderingStyle.BobTheBuilderStyle, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();
            switch (orderStyle)
            {
                case NameOrderingStyle.SilentBobStyle:
                    return this.GetSilentBobStyle(components, separator, sb);
                case NameOrderingStyle.BobTheBuilderStyle:
                    return this.GetBobTheBuilderStyle(components, separator, sb);
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderStyle), orderStyle, null);
            }
            


        }

        private string GetBobTheBuilderStyle(IdentifierComponents components, string separator, StringBuilder sb)
        {
            if (components.HasFlag(IdentifierComponents.FirstName))
            {
                sb.Append(Helpers.GetToken(this.firstNames, this.randomIndex) + separator + "The" + separator);
            }

            if (components.HasFlag(IdentifierComponents.Adjective))
            {
                sb.Append(Helpers.GetToken(this.adjectives, this.randomIndex) + separator);
            }

            if (components.HasFlag(IdentifierComponents.Nationality))
            {
                sb.Append(Helpers.GetToken(this.nationalities, this.randomIndex) + separator);
            }

            this.GetTheNounPart(components, separator, sb);

            return sb.ToString().TrimEnd(separator + "The" + separator).TrimEnd(separator);

        }


        private string GetSilentBobStyle(IdentifierComponents components, string separator, StringBuilder sb)
        {
            if (components.HasFlag(IdentifierComponents.Adjective))
            {
                sb.Append(Helpers.GetToken(this.adjectives, this.randomIndex) + separator);
            }

            if (components.HasFlag(IdentifierComponents.Nationality))
            {
                sb.Append(Helpers.GetToken(this.nationalities, this.randomIndex) + separator);
            }

            this.GetTheNounPart(components, separator, sb);

            if (components.HasFlag(IdentifierComponents.FirstName))
            {
                sb.Append(Helpers.GetToken(this.firstNames, this.randomIndex) + separator);
            }
            return sb.ToString().TrimEnd(separator);

        }

        private void GetTheNounPart(IdentifierComponents components, string separator, StringBuilder sb)
        {
            if (components.HasFlag(IdentifierComponents.Animal))
            {
                sb.Append(Helpers.GetToken(this.animals, this.randomIndex) + separator);
            }
            else if (components.HasFlag(IdentifierComponents.Noun))
            {
                sb.Append(Helpers.GetToken(this.nouns, this.randomIndex) + separator);
            }
            else if (components.HasFlag(IdentifierComponents.Profession))
            {
                sb.Append(Helpers.GetToken(this.professions, this.randomIndex) + separator);
            }
        }
    }
}