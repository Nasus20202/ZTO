using System;

namespace Training.DomainClasses
{
    public class Pet : IEquatable<Pet>
    {
        public bool Equals(Pet other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Pet)obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }

        public static bool operator ==(Pet left, Pet right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pet left, Pet right)
        {
            return !Equals(left, right);
        }

        public Sex sex;
        public string name { get; set; }
        public int yearOfBirth { get; set; }
        public float price { get; set; }
        public Species species { get; set; }

        public static ICriteria<Pet> IsASpeciesOf(Species species)
        {
            return new SpeciesCriteria(species);
        }

        public static ICriteria<Pet> IsBornAfter(int year)
        {
            return new BornAfterCriteria(year);
        }

        public static ICriteria<Pet> IsFemale()
        {
            return new SexCriteria(Sex.Female);
        }

        public static ICriteria<Pet> IsMale()
        {
            return new SexCriteria(Sex.Male);
        }

        public class SexCriteria(Sex sex) : ICriteria<Pet>
        {
            public bool IsSatisfiedBy(Pet pet)
            {
                return pet.sex == sex;
            }
        }

        public class SpeciesCriteria(Species species) : ICriteria<Pet>
        {
            public bool IsSatisfiedBy(Pet item)
            {
                return item.species == species;
            }
        }

        public class BornAfterCriteria(int year) : ICriteria<Pet>
        {
            public bool IsSatisfiedBy(Pet pet)
            {
                return pet.yearOfBirth > year;
            }
        }
    }
}
