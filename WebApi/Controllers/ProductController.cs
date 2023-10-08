using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Datos;
using WebApi.Model;
using WebApi.Model.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public ActionResult< IEnumerable<ProductDTO>> GetProducts()
        {
            return  Ok(ProductStore.lista);
                
        }
        [HttpGet("id:int",Name ="GetProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult< ProductDTO> GetProduct(int id) 
        { 
            if (id == 0)
            {
                return BadRequest();
            }
            var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                return NotFound();
            }
            return Ok(prod);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> addProduct([FromBody] ProductDTO product)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (ProductStore.lista.FirstOrDefault(p=> p.Name==product.Name)!=null)
            {
                ModelState.AddModelError("NameExists","The name "+ product.Name+ " exists");
                return BadRequest(ModelState);
            }

            if (product == null)
            {
                return BadRequest(product);

            }

            if (product.Id > 0)
            {
                StatusCode(StatusCodes.Status500InternalServerError);
            }
            product.Id= ProductStore.lista.OrderByDescending(p => p.Id).FirstOrDefault().Id +1;
            ProductStore.lista.Add(product);
            return CreatedAtRoute("GetProductId", new {id=product.Id}, product);
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var product = ProductStore.lista.FirstOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            ProductStore.lista.Remove(product);
            return NoContent();

        }
    }

}
