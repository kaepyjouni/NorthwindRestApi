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
        NorthwindOriginalContext db = new NorthwindOriginalContext();

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

    }
}
