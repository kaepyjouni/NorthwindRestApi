using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // Dependency injektion tapa 
        private NorthwindOriginalContext db;

        public EmployeesController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // Haetaan kaikki työntekijät listaan
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var tyotekijat = db.Employees.ToList();
                return Ok(tyotekijat);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Haetaan yksi työntekijä listaan
        [HttpGet("{id}")]
        public ActionResult GetOneEmployeeById(int id)
        {
            try
            {
                var tyotekija = db.Employees.Find(id);
                if (tyotekija != null)
                {
                    return Ok(tyotekija);
                }
                else
                {
                    //return BadRequest("Tuote id:llä " + id + " ei löydy tuotetta."); - perinteinen
                    return NotFound($"Työntekijä id:llä {id} ei löydy työntekijää."); // STRING INTERPOLATION -tapa verrattuna perinteiseen
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }


        // Uuden työntekijän lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Employee emp)
        {
            try
            {
                if (emp.Photo == null)
                {
                    emp.Photo = new byte[0]; // Tyhjä byte lisäys
                }

                db.Employees.Add(emp);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä {emp.LastName}");
                //return CreatedAtAction($"Lisättiin uusi työntekijä {emp.LastName}", new { id = emp.EmployeeId }, emp);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }



        }


        // Työntekijän poisto
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tyotekija = db.Employees.Find(id);

                if (tyotekija != null) // Jos id:llä löytyy työntekijä
                {
                    db.Employees.Remove(tyotekija);
                    db.SaveChanges();
                    return Ok($"Työntekijä {tyotekija.FirstName} {tyotekija.LastName} poistettiin.");
                }

                return NotFound($"Työntekijä id:llä {id} ei löytynyt työntekijää.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Työntekijän muokkaaminen 
        [HttpPut("{id}")]

        public ActionResult EditEmployee(int id, [FromBody] Employee employee)
        {
            var tyotekija = db.Employees.Find(id);
            if (tyotekija != null)
            {

                tyotekija.FirstName = employee.FirstName;
                tyotekija.LastName = employee.LastName;
                tyotekija.Address = employee.Address;
                tyotekija.City = employee.City;
                tyotekija.Country = employee.Country;
                tyotekija.Title = employee.Title;

                db.SaveChanges();
                return Ok("Muokattu työntekijää " + tyotekija.FirstName + tyotekija.LastName);
            }

            return NotFound("Työntekijää ei löytynyt id:llä " + id);
        }


        // Haku työntekijän nimen osalla
        [HttpGet("employeename/{ename}")]

        public ActionResult GetByName(string ename)
        {
            try
            {
                var tyotekija = db.Employees.Where(t => t.FirstName.Contains(ename)); // <--- toimiva ja hyvä linq kysely nykyisin

                return Ok(tyotekija);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
