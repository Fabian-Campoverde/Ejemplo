using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi.Datos;
using WebApi.Model;
using WebApi.Model.DTO;
using WebApi.Repositorio.IRepositorio;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberProductController : ControllerBase
    {
        private readonly ILogger<NumberProductController> logger;
        private readonly iProductRepositorio productRepo;
        private readonly iNumberProductRepositorio numberRepo;
        private readonly IMapper mapper;
        protected ApiResponse response;
         public NumberProductController(ILogger<NumberProductController> _logger, iProductRepositorio _productRepo, 
                                                                        iNumberProductRepositorio _numberRepo, IMapper mapp)
        {
            logger = _logger;   
            productRepo = _productRepo;
            numberRepo = _numberRepo;
            mapper = mapp;
            response= new ApiResponse();
        }


        [HttpGet]
        public async Task< ActionResult< ApiResponse>> GetNumberProducts()
        {
            try
            {
                logger.LogInformation("Obtener Numeros de Productos");
                IEnumerable<NumberProduct> numberproductList = await numberRepo.obtenerTodos();
                response.Resultado = mapper.Map<IEnumerable<NumberProductDTO>>(numberproductList);
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Errors= new List<string>() { ex.ToString()};
                
                
            }
            return response;


            
                
        }
        [HttpGet("id:int",Name ="GetNumberProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< ActionResult< ApiResponse>> GetNumberProduct(int id) 
        {
            try
            {
                if (id == 0)
                {
                    logger.LogError("Error con el numero de producto " + id);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsExitoso = false;
                    return BadRequest(response);
                }
                //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
                var prod = await numberRepo.Obtener(x => x.ProductNo == id);

                if (prod == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsExitoso = false;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<NumberProductDTO>(prod);
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Errors = new List<string>() { ex.ToString() };

            }
            return response;

            
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<ApiResponse>> addProduct([FromBody] NumberProductCreateDTO createnumberproduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

                if (await numberRepo.Obtener(p => p.ProductNo == createnumberproduct.ProductNo) != null)
                {
                    ModelState.AddModelError("NumberExists", "El N° de producto " + createnumberproduct.ProductNo + " existe");
                    return BadRequest(ModelState);
                }

                if (await productRepo.Obtener(p => p.Id == createnumberproduct.ProductId) == null)
                {
                    ModelState.AddModelError("IdNotExists", "El N° de producto padre " + createnumberproduct.ProductNo + " no existe");
                    return BadRequest(ModelState);
                }

                if (createnumberproduct == null)
                {
                    return BadRequest(createnumberproduct);

                }
                NumberProduct modelo = mapper.Map<NumberProduct>(createnumberproduct);
                modelo.FechaCreacion=DateTime.Now;
                await numberRepo.Crear(modelo);
                response.Resultado = modelo;
                response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProductId", new { id = modelo.ProductNo }, response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Errors = new List<string>() { ex.ToString() };

            }
            return response;

            //product.Id= ProductStore.lista.OrderByDescending(p => p.Id).FirstOrDefault().Id +1;
            //Product modelo = new Product()
            //{             
            //    Name = product.Name,
            //    Description = product.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = product.Category,
            //    Proveedor = product.Proveedor,
            //    Precio = product.Precio,
            //    Stock = product.Stock,
            //    imgUrl = product.imgUrl,
            //    Tipo = ""



            //};

           
            
            //product.Id = db.Products.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            //ProductStore.lista.Add(product);
           
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< IActionResult> DeleteNumberProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                //var product = ProductStore.lista.FirstOrDefault(p=>p.Id==id);
                var numberproduct = await numberRepo.Obtener(p => p.ProductNo == id);
                if (numberproduct == null)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                //ProductStore.lista.Remove(product);
                await numberRepo.Remover(numberproduct);
                response.StatusCode = HttpStatusCode.NoContent;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Errors = new List<string>() { ex.ToString() };

            }
            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateNumberProduct(int id, [FromBody] NumberProductUpdateDTO updatenumberproduct)
        {


            if(updatenumberproduct == null || id!= updatenumberproduct.ProductNo)
            {
                response.IsExitoso = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await productRepo.Obtener(n=> n.Id== updatenumberproduct.ProductId)==null)
            {
                ModelState.AddModelError("LlaveForanea", "El N° de producto padre " + updatenumberproduct.ProductId + " no exists");
                return BadRequest(ModelState);
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            //prod.Name=product.Name;
            //prod.Description=product.Description;
            //prod.Category=product.Category;
            //Product modelo = new Product()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = product.Category,
            //    Proveedor = product.Proveedor,
            //    Precio = product.Precio,
            //    Stock = product.Stock,
            //    imgUrl = product.imgUrl,
            //    Tipo = ""

            //};
            NumberProduct modelo= mapper.Map<NumberProduct>(updatenumberproduct);
           await numberRepo.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;

            return Ok(response);
        }

        
    }

}
