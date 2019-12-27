using Flights_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject_OutOfProject
{
    public class Center_UnitTestProject_OutOfProject
    {
       public LoggedInAirlineFacade AirlineFacade;
       public LoggedInCustomerFacade CustomerFacade; 
       public LoggedInAdministratorFacade AdministratorFacade;
       public AnonymousUserFacade AnonymousUserFacade;

        public LoginToken<AirlineCompany> AirLineToken;  
        public LoginToken<Customer> CustomerToken;  
        public LoginToken<Administrator> AdministratorToken;  

        public Center_UnitTestProject_OutOfProject()
        {
            LoginService L = new LoginService();
            FlyingCenterSystem f = FlyingCenterSystem.GetInstance();

            AdministratorFacade = new LoggedInAdministratorFacade();
            CustomerFacade = new LoggedInCustomerFacade();
            AnonymousUserFacade = new AnonymousUserFacade();
            AirlineFacade = new LoggedInAirlineFacade();

            Customer cust = AdministratorFacade.GetAllCustomers().ToList()[0];
            AirlineCompany airline = AdministratorFacade.GetAllAirlineCompanies().ToList()[0];

            L.TryAdminLogin(flightCenterConfig.ADMIN_NAME, flightCenterConfig.ADMIN_PASSWORD, out AdministratorToken);
            L.TryCustomerLogin(cust.UserName, cust.Password, out CustomerToken);
            L.TryAirLineLogin(airline.UserName, airline.Password, out AirLineToken);
        }
    }
}
