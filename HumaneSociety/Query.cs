using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {

        internal static List<USState> GetStates()
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();


            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();


            return client;
        }

        internal static List<Client> GetClients()
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string userName, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = userName;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = zipCode;
                newAddress.USStateId = stateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {

             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();


            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = clientAddress.Zipcode;
                newAddress.USStateId = clientAddress.USStateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }

        internal static List<Animal> SearchForAnimalByMultipleTraits(Dictionary<int, string> searchParameterDictionary)
        {

            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            string dictionaryValue = searchParameterDictionary.Values.ElementAt(0);
            int dictionaryKey = searchParameterDictionary.Keys.ElementAt(0);


            switch (dictionaryKey)
            {
                case 1:
                    var categorySearchResult = db.Animals.Where(a=>a.Category.Name == dictionaryValue).ToList();
                    return categorySearchResult;
                case 2:
                    var nameSearchResult = db.Animals.Where(a => a.Name == dictionaryValue).ToList();
                    return nameSearchResult;
                case 3:
                    var ageSearchResult = db.Animals.Where(a => a.Age == Int32.Parse(dictionaryValue)).ToList();
                    return ageSearchResult;
                case 4:
                    var demeanorSearchResult = db.Animals.Where(a => a.Demeanor == dictionaryValue).ToList();
                    return demeanorSearchResult;
                case 5:
                    var kidFriendlySearchResult = db.Animals.Where(a => a.KidFriendly.ToString() == dictionaryValue).ToList();
                    return kidFriendlySearchResult;
                case 6:
                    var petFriendlySearchResult = db.Animals.Where(a => a.PetFriendly.ToString() == dictionaryValue).ToList();
                    return petFriendlySearchResult;
                case 7:
                    var weightSearchResult = db.Animals.Where(a => a.Weight == Int32.Parse(dictionaryValue)).ToList();
                    return weightSearchResult;
                case 9:
                    var idSearchResult = db.Animals.Where(a => a.AnimalId == Int32.Parse(dictionaryValue)).ToList();
                    return idSearchResult;
                default:
                    List<Animal> defaultReturn = new List<Animal>();
                    return defaultReturn;
            }
            
            
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if(employeeFromDb == null)
            {
                throw new NullReferenceException();            
            }
            else
            {
                return employeeFromDb;
            }            
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();


            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();


            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();


            return employeeWithUserName == null;
        }

        internal static void AddUserNameAndPassword(Employee employee)
        {
            HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        // CUSTOMER CLASS QUERY METHODS //
        public static Animal GetAnimalByID(int id)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal animal = db.Animals.Where(c => c.AnimalId == id).Single();
            return animal;
        }


        //Still some doubts on this one -- need to know a way to check/debug
        public static void Adopt(Animal animal, Client client)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Adoption adoption = db.Adoptions.Where(c => c.AnimalId == animal.AnimalId && c.ClientId == client.ClientId).Single();

            adoption.AdoptionFee += 75;
            if(adoption.PaymentCollected == true)
            {
                adoption.Animal.AdoptionStatus = "Adopted";
                adoption.ApprovalStatus = "True";

            }

            db.SubmitChanges();
        }
        

        // USER EMPLOYEE CLASS //

        //DONE
        public static List<Adoption> GetPendingAdoptions()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var pendingAdoptions =  db.Adoptions.Where(m => m.ApprovalStatus.ToLower() == "pending").ToList();
            return pendingAdoptions;
        }

        public static void UpdateAdoption(bool b, Adoption adoption)
        {
            throw new Exception();
        }

        public static List<AnimalShot> GetShots(Animal animal)
        {
            throw new Exception();
        }

        public static void UpdateShot(string word, Animal animal)
        {
            throw new Exception();
        }

        public static void EnterAnimalUpdate(Animal animal, Dictionary<int, string> updates)
        {
            throw new Exception();
        }

        public static void RemoveAnimal(Animal animal)
        {

            throw new Exception();

        }

        //DONE
        public static int GetCategoryId()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            List<string> types = new List<string>() { "What is the Animal Type:", "1. Dog", "2. Cat", "3. Lizard", "4. Bird", "5. Rodent", "Enter the corresponding number." };
            UserInterface.DisplayUserOptions(types);
            int animalTypeId = Int32.Parse(UserInterface.GetUserInput());
            var category = db.Categories.Where(c => c.CategoryId == animalTypeId).Single();
            return category.CategoryId;
        }

        //DONE
        public static int GetDietPlanId()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            List<string> plans = new List<string>() { "Choose a Diet Plan:", "1. Canine", "2. Feline", "3. Avian", "4. Reptile", "5. Mammal", "Enter the corresponding number." };
            UserInterface.DisplayUserOptions(plans);
            int dietNameId = Int32.Parse(UserInterface.GetUserInput());
            var dietPlan = db.DietPlans.Where(m => m.DietPlanId == dietNameId).Single();
            return dietPlan.DietPlanId;
        }

        //Done - Animal Added
        public static void AddAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        public static void AddUsernameAndPassword(Employee employee)
        {
            throw new Exception();
        }

        // ADMIN CLASS //

        public static void RunEmployeeQueries(Employee employee, string word)
        {
            throw new Exception();
        }

        // USERINTERFACE CLASS //
        public static Room GetRoom(int id)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var room = db.Rooms.Where(r => r.AnimalId == id).Single();
            return room;
        }

        //Query Made for check housing method
        public static List<Room> GetAnimalHousing()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var bookedRooms = db.Rooms.Where(c => c.AnimalId != null).ToList();
            return bookedRooms;
        }

        //Query Made to get Available rooms
        public static List<Room> GetAvailableRooms()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var availableRooms = db.Rooms.Where(c => c.AnimalId == null).ToList();
            return availableRooms;
        }

        //Query to Assign or update room
        public static void UpdateRoom(int number, int id)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Room roomFromDb = db.Rooms.Where(r => r.RoomNumber == number).Single();
            if(roomFromDb.AnimalId != null)
            {
                if(UserInterface.GetBitData("Already Occupied! Do you still want to update?"))
                {
                    roomFromDb.AnimalId = id;
                }
            }
            else
            {
                roomFromDb.AnimalId = id;
            }
            db.SubmitChanges();
        }

        //Queries for Categorization UserStory
        //Getting the Catgories in a list
        public static List<Category> GetAllCategories()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var allCategories = db.Categories.ToList();
            return allCategories;
        }

        //Create a New category
        public static void AddCategory(Category category)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Categories.InsertOnSubmit(category);
            db.SubmitChanges();

        }

        //Check to make sure category doesn't exist already
        public static bool CheckCategoryName(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            try
            {
                Category categoryFromDb = db.Categories.Where(c => c.Name == name).Single();
                return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }
    }
}