using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Alustetaan tietokantayhteys

        // Perinteinen tapa alapuolella
        //NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency injektion tapa 
        private NorthwindOriginalContext db;

        public CustomersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // Haetaan kaikki asiakkaat listaan
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Haetaan yhden asiakkaan listaan
        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    //return BadRequest("Asiakas id:llä " + id + " ei löydy asiakasta."); - perinteinen
                    return NotFound($"Asiakas id:llä {id} ei löydy asiakasta."); // STRING INTERPOLATION -tapa verrattuna perinteiseen
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }


        // Uuden asiakkaan lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }



        }


        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null) // Jos id:llä löytyy asiakas
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok($"Asiakas {asiakas.CompanyName} poistettiin.");
                }

                return NotFound($"Asiakas id:llä {id} ei löytynyt asiakasta.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }


        // Asiakkaan muokkaaminen 
        [HttpPut("{id}")]

        public ActionResult EditCustomer(string id, [FromBody] Customer customer)
        {
            var asiakas = db.Customers.Find(id);
            if (asiakas != null)
            {

                asiakas.CompanyName = customer.CompanyName;
                asiakas.ContactName = customer.ContactName;
                asiakas.Address = customer.Address;
                asiakas.City = customer.City;
                asiakas.Region = customer.Region;
                asiakas.PostalCode = customer.PostalCode;
                asiakas.Country = customer.Country;
                asiakas.Phone = customer.Phone;
                asiakas.Fax = customer.Fax;

                db.SaveChanges();
                return Ok("Muokattu asiakasta " + asiakas.CompanyName);
            }

            return NotFound("Asiakasta ei löytynyt id:llä " + id);
        }


        //Hakee nimen osalla
        [HttpGet("companyname/{cname}")]

        public ActionResult GetByName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname)); // <--- toimiva ja hyvä linq kysely nykyisin
                // var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <--- sama mutta "vanhaa tyylii"
                // var cust = db.Customers.Where(c => c.CompanyName == cname); <--- perfect match eli koko hakusana pitää osaa kirjottaa
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
