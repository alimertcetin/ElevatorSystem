using System;
namespace AutamationSystem.HumanLogic
{
    public class Person
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }

        public int Age { get; private set; }

        public DateTime BirthDay { get; private set; }

        public int TargetFloorIndex; //where this person goes?

        public Person()
        {
            // if we wouldn't create this constructor
            // we couldn't create new person without parameters
        }

        public Person(string name, string surname, DateTime birthDay, int targetFloor)
        {
            this.Name = name;
            this.Surname = surname;
            this.BirthDay = birthDay;
            this.Age = DateTime.Now.Year - birthDay.Year;
            this.TargetFloorIndex = targetFloor;
        }

        public static Person CreteRandomPerson(int floorCount)
        {
            //Usage 1
            //string exampleString = Guid.NewGuid().ToString().Split('-')[0]; //You can directly get index while chaning
            string[] stringArr = Guid.NewGuid().ToString().Split('-'); //method chaning

            int randomFloor = new Random().Next(0, floorCount);
            Person person = new Person(stringArr[0], stringArr[1], GetRandomDate(), randomFloor);
            //Usage 2
            //Guid guid = Guid.NewGuid();
            //string guidString = guid.ToString();
            //string[] stringArray = guidString.Split('-');
            //string firstName = stringArray[0];
            return person;
        }

        private static DateTime GetRandomDate(int maxYear = 2005, int minYear = 0)
        {
            var random = new Random();
            var startDate = DateTime.Now.AddYears(-maxYear);
            var endDate = DateTime.Now.AddYears(-minYear);
            var range = Convert.ToInt32(endDate.Subtract(startDate).TotalDays);
            return startDate.AddDays(random.Next(range));
        }

        public override string ToString()
        {
            return $"Name : {this.Name}{Environment.NewLine}" +
                $"Surname : {this.Surname}{Environment.NewLine}" +
                $"Age : {Age}{Environment.NewLine}" +
                $"Target Floor : {this.TargetFloorIndex}";
        }
    }
}