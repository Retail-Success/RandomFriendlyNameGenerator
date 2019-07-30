﻿using System;
using RandomFriendlyNameGenerator.Data;
using RandomFriendlyNameGenerator.RandomIndex;

namespace RandomFriendlyNameGenerator
{
    /// <summary>
    /// Generates human-like names based on random first names and last names combinations from all countries.
    /// </summary>
    public class PersonNames
    {
        private readonly IGenerateRandomIndex randomIndex = new RandomBasedGenerator();

        public string Get(NameGender gender = NameGender.Any, NameComponents components = NameComponents.FirstNameLastName, string separator = " ", bool forceSingleLetter = false)
        {
            char? forcedSingleLetter = Helpers.GetForcedSingleCharacter(forceSingleLetter, this.randomIndex);

            string firstName = this.GetFirstName(components, this.DetermineGender(gender), forcedSingleLetter);
            string lastName = this.GetLastName(components, forcedSingleLetter);
            string middleName = this.GetMiddleName(components, this.DetermineGender(gender), forcedSingleLetter);
            switch (components)
            {
                case NameComponents.FirstNameLastName:
                    return $"{firstName}{separator}{lastName}";
                case NameComponents.FirstNameMiddleNameLastName:
                    return $"{firstName}{separator}{middleName}{separator}{lastName}";
                case NameComponents.FirstNameOnly:
                    return firstName;
                case NameComponents.LastNameOnly:
                    return lastName;
                case NameComponents.LastNameFirstName:
                    return $"{lastName}{separator}{firstName}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(components), components, null);
            }


        }

        private string GetMiddleName(NameComponents components, NameGender gender, char? forcedSingleLetter)
        {
            if (components == NameComponents.FirstNameMiddleNameLastName)
            {
                return this.GetName(gender, forcedSingleLetter);
            }

            return "";
        }

        private NameGender DetermineGender(NameGender gender)
        {
            if (gender != NameGender.Any)
            {
                return gender;
            }
            else
            {
                var diceRoll = this.randomIndex.Get(2);
                if (diceRoll == 0)
                {
                    return NameGender.Female;
                }
                else
                {
                    return NameGender.Male;
                }
            }
        }

        private string GetFirstName(NameComponents components, NameGender gender, char? forcedSingleLetter)
        {
            if (components != NameComponents.LastNameOnly)
            {
                return this.GetName(gender, forcedSingleLetter);
            }

            return "";
        }

        private string GetName(NameGender gender, char? forcedSingleLetter)
        {
            switch (gender)
            {
                case NameGender.Any:
                    return $"{Helpers.GetToken(FirstNames.Values, this.randomIndex, forcedSingleLetter)}";
                case NameGender.Female:
                    return $"{Helpers.GetToken(FemaleFirstNames.Values, this.randomIndex, forcedSingleLetter)}";
                case NameGender.Male:
                    return $"{Helpers.GetToken(MaleFirstNames.Values, this.randomIndex, forcedSingleLetter)}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        private string GetLastName(NameComponents components, char? forcedSingleLetter)
        {
            if (components != NameComponents.FirstNameOnly)
            {
                return Helpers.GetToken(LastNames.Values, this.randomIndex, forcedSingleLetter);
            }

            return "";
        }
    }
}