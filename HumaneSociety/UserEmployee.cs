﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class UserEmployee : User
    {
        Employee employee;
        
        public override void LogIn()
        {
            if (CheckIfNewUser())
            {
                CreateNewEmployee();
                LogInPreExistingUser();
            }
            else
            {
                Console.Clear();
                LogInPreExistingUser();
            }
            RunUserMenus();
        }
        protected override void RunUserMenus()
        {
            Console.Clear();
            List<string> options = new List<string>() { "What would you like to do? (select number of choice)", "1. Add animal", "2. Remove Anmial", "3. Check Animal Status",  "4. Approve Adoption" , "5. Check All Categories", "6. Make New Category",  "7. Check Housing", "8. Manage Housing", "9. Make New Diet-Plan", "10. Check All DietPlans", "11. Modify Existing Diet-Plan" };
            UserInterface.DisplayUserOptions(options);
            string input = UserInterface.GetUserInput();
            RunUserInput(input);
        }
        private void RunUserInput(string input)
        {
            switch (input)
            {
                case "1":
                    AddAnimal();
                    RunUserMenus();
                    return;
                case "2":
                    RemoveAnimal();
                    RunUserMenus();
                    return;
                case "3":
                    CheckAnimalStatus();
                    RunUserMenus();
                    return;
                case "4":
                    CheckAdoptions();
                    RunUserMenus();
                    return;
                case "5":
                    CheckCurrentCategories();
                    RunUserMenus();
                    return;
                case "6":
                    CreateCategory();
                    RunUserMenus();
                    return;
                case "7":
                    CheckHousing();
                    RunUserMenus();
                    return;
                case "8":
                    ManageHousing();
                    RunUserMenus();
                    return;

                case "9":
                    CreateNewDietPlan();
                    RunUserMenus();
                    return;
                case "10":
                    CheckCurrentDietPlans();
                    RunUserMenus();
                    return;
                case "11":
                    ModDietPlan();
                    RunUserMenus();
                    return;
                default:
                    UserInterface.DisplayUserOptions("Input not accepted please try again");
                    RunUserMenus();
                    return;
            }
        }

        //Displays all the current Categories
        private void CheckCurrentCategories()
        {
            Console.Clear();
            List<Category> currentCategories = Query.GetAllCategories();
            foreach(Category category in currentCategories)
            {
                UserInterface.DisplayUserOptions($"Category ID - {category.CategoryId} is for Animal Type - {category.Name}");
            }
            Console.ReadLine();
        }


        //Creates new category depending upon the availability check
        private void CreateCategory()
        {
            
            string newType = UserInterface.GetStringData("type", "the new Animal");
            if(!Query.CheckCategoryName(newType.Trim()))
            {
                Category category = new Category();
                category.Name = newType.Trim();
                Query.AddCategory(category);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                UserInterface.DisplayUserOptions("The Animal Category already Exists. Please Check all Categories");
                Console.ReadLine();
                Console.ResetColor();
                Console.Clear();
            }
        }
        //Method created to check and keep track of animal housing as per User Story
        private void CheckHousing()
        {
            Console.Clear();
            List<Room> roomsInfo;
            List<Room> roomsAvailable;
            roomsInfo = Query.GetAnimalHousing();
            roomsAvailable = Query.GetAvailableRooms();
            foreach(Room room in roomsInfo)
            {
                UserInterface.DisplayUserOptions($"Room No - {room.RoomNumber} - {room.Animal.Name} ");
            }
            UserInterface.DisplayUserOptions("Available Rooms");
            foreach(Room room in roomsAvailable)
            {
                UserInterface.DisplayUserOptions($"Room No - {room.RoomNumber} - Available");
            }
            Console.ReadLine();
        }

        //Manages the animal housing - assign/updates rooms
        private void ManageHousing()
        {
            CheckHousing();
            if(UserInterface.GetBitData("Would you like to Assign a room to an Animal"))
            {
                UserInterface.DisplayUserOptions("Enter the available room number you want to assign");
                int roomNumber = UserInterface.GetIntegerData();
                AssignRoom(roomNumber);
                CheckHousing();
            }
            else
            {
                return;
            }
        }
        //Assign a room to an animal
        private void AssignRoom(int number)
        {
            Console.Clear();
            UserInterface.DisplayUserOptions("Enter the Animal's ID you want to put in the room.");
            int animalID = UserInterface.GetIntegerData();
            Query.UpdateRoom(number, animalID);
            UserInterface.DisplayUserOptions($"Room Number {number} assigned");
        }
        private void CheckAdoptions()
        {
            Console.Clear();
            List<string> adoptionInfo = new List<string>();
            int counter = 1;
            var adoptions = Query.GetPendingAdoptions();
            if(adoptions.Count > 0)
            {
                foreach(Adoption adoption in adoptions)
                {
                    adoptionInfo.Add($"{counter}. {adoption.Client.FirstName} {adoption.Client.LastName}, {adoption.Animal.Name} {adoption.Animal.Category.Name}");
                    counter++;
                }
                UserInterface.DisplayUserOptions(adoptionInfo);
                UserInterface.DisplayUserOptions("Enter the number of the adoption you would like to approve");
                int input = UserInterface.GetIntegerData();
                ApproveAdoption(adoptions[input - 1]);
            }
            else
            {
                UserInterface.DisplayUserOptions("No Pending Adoptions");
                UserInterface.GetUserInput();
            }

        }
        
        public void ApproveTransaction(Adoption adoption)
        {
            adoption.PaymentCollected = true;
        }
        private void ApproveAdoption(Adoption adoption)
        {
            UserInterface.DisplayAnimalInfo(adoption.Animal);
            UserInterface.DisplayClientInfo(adoption.Client);
            UserInterface.DisplayUserOptions("Would you approve this adoption?");
            
            if ((bool)UserInterface.GetBitData())
            {
                ApproveTransaction(adoption);
                Query.UpdateAdoption(true, adoption);
                UserInterface.DisplayUserOptions("Adoption Approved");
                UserInterface.GetUserInput();
            }
            else
            {
                Query.UpdateAdoption(false, adoption);
            }
            
        }

        private void CheckAnimalStatus()
        {
            Console.Clear();
            var searchParameterDictionary = UserInterface.GetAnimalCriteria();
            var animals = Query.SearchForAnimalByMultipleTraits(searchParameterDictionary).ToList();
            if(animals.Count > 1)
            {
                UserInterface.DisplayUserOptions("Several animals found");
                UserInterface.DisplayAnimals(animals);
                UserInterface.DisplayUserOptions("Enter the ID of the animal you would like to check");
                int ID = UserInterface.GetIntegerData();
                CheckAnimalStatus(ID);
                return;
            }
            if(animals.Count == 1)
            {
                Console.Clear();
                UserInterface.DisplayUserOptions("***ANIMAL INFOMATION***");
                UserInterface.DisplayAnimals(animals);
                Console.ReadLine();
            }
            if(animals.Count == 0)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            RunCheckMenu(animals[0]);
        }

        private void RunCheckMenu(Animal animal)
        {
            bool isFinished = false;
            Console.Clear();
            while(!isFinished){
                animal =  Query.GetUpdatedAnimal(animal);
                List<string> options = new List<string>() { "Animal found:", animal.Name, animal.Category.Name, "Would you like to:", "1. Get Info", "2. Update Info", "3. Check shots", "4. Return" };
                UserInterface.DisplayUserOptions(options);
                int input = UserInterface.GetIntegerData();
                if (input == 4)
                {
                    isFinished = true;
                    continue;
                }
                RunCheckMenuInput(input, animal);
            }
        }

        private void RunCheckMenuInput(int input, Animal animal)
        {
            
            switch (input)
            {
                case 1:
                    UserInterface.DisplayAnimalInfo(animal);
                    Console.Clear();
                    return;
                case 2:
                    UpdateAnimal(animal);
                    Console.Clear();
                    return;
                case 3:
                    CheckShots(animal);
                    Console.Clear();
                    return;
                default:
                    UserInterface.DisplayUserOptions("Input not accepted please select a menu choice");
                    return;
            }
        }

        private void CheckShots(Animal animal)
        {
            List<string> shotInfo = new List<string>();
            var shots = Query.GetShots(animal);
            foreach(AnimalShot shot in shots.ToList())
            {
                shotInfo.Add($"{shot.Shot.Name} Date: {shot.DateReceived}");
            }
            if(shotInfo.Count > 0)
            {
                UserInterface.DisplayUserOptions(shotInfo);
                if(UserInterface.GetBitData("Would you like to Update shots?"))
                {
                    Query.UpdateShot("booster", animal);
                }
            }
            else
            {
                if (UserInterface.GetBitData("Would you like to Update shots?"))
                {
                    Query.UpdateShot("booster", animal);
                }
            }
            
        }

        private void UpdateAnimal(Animal animal, Dictionary<int, string> updates = null)
        {
            if(updates == null)
            {
                updates = new Dictionary<int, string>();
            }
            string input;
            List<string> options = new List<string>() { "Select Updates: (Enter number and choose finished when finished)", "1. Category", "2. Name", "3. Age", "4. Gender", "5. Demeanor", "6. Kid friendly", "7. Pet friendly", "8. Weight", "9. ID", "10. Finished" };
            do
            {
                Console.Clear();
                UserInterface.DisplayUserOptions(options);
                input = UserInterface.GetUserInput();
                updates = UserInterface.EnterSearchCriteria(updates, input);
            }
            while (input != "10");
            Query.EnterAnimalUpdate(animal, updates);
            UserInterface.DisplayUserOptions("Animal Updated Successfully");
            UserInterface.GetUserInput();
        }

        private void CheckAnimalStatus(int iD)
        {
            Console.Clear();
            var animals = SearchForAnimal(iD).ToList();
            if (animals.Count == 0)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            RunCheckMenu(animals[0]);
        }

        private IQueryable<Animal> SearchForAnimal(int iD)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var animals = (from animal in context.Animals where animal.AnimalId == iD select animal);
            return animals;
        }       

        private void RemoveAnimal()
        {
            var searchParameterDictionary = UserInterface.GetAnimalCriteria();
            var animals = Query.SearchForAnimalByMultipleTraits(searchParameterDictionary).ToList();
            if (animals.Count > 1)
            {
                UserInterface.DisplayUserOptions("Several animals found please refine your search.");
                UserInterface.DisplayAnimals(animals);
                UserInterface.DisplayUserOptions("Press enter to continue");
                Console.ReadLine();
                return;
            }
            else if (animals.Count < 1)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            var animal = animals[0];
            List<string> options = new List<string>() { "Animal found:", animal.Name, animal.Category.Name, "would you like to delete?" };
            if ((bool)UserInterface.GetBitData(options))
            {
                Query.RemoveAnimal(animal);
            }
        }
        private void AddAnimal()
        {
            Console.Clear();
            Animal animal = new Animal();
            animal.CategoryId = Query.GetCategoryId();
            animal.Name = UserInterface.GetStringData("name", "the animal's");
            animal.Age = UserInterface.GetIntegerData("age", "the animal's");
            animal.Demeanor = UserInterface.GetStringData("demeanor", "the animal's");
            animal.KidFriendly = UserInterface.GetBitData("the animal", "child friendly");
            animal.PetFriendly = UserInterface.GetBitData("the animal", "pet friendly");
            animal.Weight = UserInterface.GetIntegerData("the animal", "the weight of the");
            animal.Gender = UserInterface.GetStringData("gender", "the animal's");
            animal.DietPlanId = Query.GetDietPlanId();
            Query.AddAnimal(animal);
        }
        protected override void LogInPreExistingUser()
        {
            List<string> options = new List<string>() { "Please log in", "Enter your username (CaSe SeNsItIvE)" };
            UserInterface.DisplayUserOptions(options);
            userName = UserInterface.GetUserInput();
            UserInterface.DisplayUserOptions("Enter your password (CaSe SeNsItIvE)");
            string password = UserInterface.GetUserInput();
            try
            {
                Console.Clear();
                employee = Query.EmployeeLogin(userName, password);
                UserInterface.DisplayUserOptions("Login successfull. Welcome.");
            }
            catch
            {
                Console.Clear();
                UserInterface.DisplayUserOptions("Employee not found, please try again, create a new user or contact your administrator");
                LogIn();
            }
            
        }
        private void CreateNewEmployee()
        {
            Console.Clear();
            string email = UserInterface.GetStringData("email", "your");
            int employeeNumber = int.Parse(UserInterface.GetStringData("employee number", "your"));
            try
            {
                employee = Query.RetrieveEmployeeUser(email, employeeNumber);
            }
            catch
            {
                UserInterface.DisplayUserOptions("Employee not found please contact your administrator");
                PointOfEntry.Run();
            }
            if (employee.Password != null)
            {
                UserInterface.DisplayUserOptions("User already in use please log in or contact your administrator");
                LogIn();
                return;
            }
            else
            {
                UpdateEmployeeInfo();
            }
        }

        private void UpdateEmployeeInfo()
        {
            GetUserName();
            GetPassword();
            Query.AddUserNameAndPassword(employee);
        }

        private void GetPassword()
        {
            UserInterface.DisplayUserOptions("Please enter your password: (CaSe SeNsItIvE)");
            employee.Password = UserInterface.GetUserInput();
        }

        private void GetUserName()
        {
            Console.Clear();
            string username = UserInterface.GetStringData("username", "your");
            if (Query.CheckEmployeeUserNameExist(username))
            {
                UserInterface.DisplayUserOptions("Username already in use please try another username.");
                GetUserName();
            }
            else
            {
                employee.UserName = username;
                UserInterface.DisplayUserOptions("Username successful");
            }
        }

        public static void CreateNewDietPlan()
        {
            string newPlanName = UserInterface.GetStringData("name", "the new Diet-Plan");
            string foodType = UserInterface.GetStringData("type", "the food");
            int foodAmount = UserInterface.GetIntegerData("in cups", "the amount of food");
            if (!Query.CheckDietPlanName(newPlanName.Trim()))
            {
                DietPlan dietPlan = new DietPlan();
                dietPlan.Name = newPlanName.Trim();
                dietPlan.FoodType = foodType.Trim();
                dietPlan.FoodAmountInCups = foodAmount;
                Query.AddDietPlan(dietPlan);
            }
            else
            {
                Console.WriteLine("That Diet-Plan already Exists. Please check all diet-plans");
                Console.ReadLine();
                Console.Clear();
            }
        }


        private void CheckCurrentDietPlans()
        {
            Console.Clear();
            List<DietPlan> currentDietPlans = Query.GetAllDietPlans();
            foreach(DietPlan plan in currentDietPlans)
            {
                UserInterface.DisplayUserOptions($"DietPlan - Name of Plan is {plan.Name}. Type of Food is {plan.FoodType}.  This plan administers {plan.FoodAmountInCups} cups of food.");
            }
            Console.ReadLine();
        }



        public static void ModDietPlan()
        {
            Console.Clear();

            Console.WriteLine("Choose The Name Of The Plan You Wish To Modify");
           
            DietPlan ModThisPlan = Query.FindDietPlan(UserInterface.GetUserInput());

            Console.WriteLine("Choose a feild to modify: 1. Name  2. Food Type  3. Amount, in Cups");
            var fieldToMod = Convert.ToInt32(Console.ReadLine());

            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            
            var dietPlantFromDb = db.DietPlans.Where(d => d.Name == ModThisPlan.Name).Single();

            if (fieldToMod == 1)
            {
                string newName = UserInterface.GetStringData("name", "the new");
                
                dietPlantFromDb.Name = newName;
            }
            else if (fieldToMod == 2)
            {
                string newFoodType = UserInterface.GetStringData("type", "the new");
               
                dietPlantFromDb.FoodType = newFoodType;
            }
            else if (fieldToMod == 3)
            {
                int newFoodAmount = UserInterface.GetIntegerData("amount", "the new food");
                
                dietPlantFromDb.FoodAmountInCups = newFoodAmount;
            }
            else
            {
                Console.WriteLine("Not A Valid Field To Modify");
                Console.ReadLine();
            }

            db.SubmitChanges();

        }
    }
}
