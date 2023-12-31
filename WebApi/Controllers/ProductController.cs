﻿using AutoMapper;
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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly iProductRepositorio productRepo;
        private readonly IMapper mapper;
        protected ApiResponse response;
         public ProductController(ILogger<ProductController> _logger, iProductRepositorio _productRepo, IMapper mapp)
        {
            logger = _logger;   
            productRepo = _productRepo;
            mapper = mapp;
            response= new ApiResponse();
        }


        [HttpGet]
        public async Task< ActionResult< ApiResponse>> GetProducts()
        {
            try
            {
                logger.LogInformation("Obtener Productos");
                IEnumerable<Product> productList = await productRepo.obtenerTodos();
                response.Resultado = mapper.Map<IEnumerable<ProductDTO>>(productList);
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
        [HttpGet("id:int",Name ="GetProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< ActionResult< ApiResponse>> GetProduct(int id) 
        {
            try
            {
                if (id == 0)
                {
                    logger.LogError("Error con el producto " + id);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsExitoso = false;
                    return BadRequest(response);
                }
                //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
                var prod = await productRepo.Obtener(x => x.Id == id);

                if (prod == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsExitoso = false;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<ProductDTO>(prod);
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
        public async Task< ActionResult<ApiResponse>> addProduct([FromBody] ProductCreateDTO createproduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

                if (await productRepo.Obtener(p => p.Name.ToLower() == createproduct.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("NameExists", "The name " + createproduct.Name + " exists");
                    return BadRequest(ModelState);
                }

                if (createproduct == null)
                {
                    return BadRequest(createproduct);

                }
                Product modelo = mapper.Map<Product>(createproduct);
                modelo.FechaCreacion=DateTime.Now;
                await productRepo.Crear(modelo);
                response.Resultado = modelo;
                response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProductId", new { id = modelo.Id }, response);
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
        public async Task< IActionResult> DeleteProduct(int id)
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
                var product = await productRepo.Obtener(p => p.Id == id);
                if (product == null)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                //ProductStore.lista.Remove(product);
                await productRepo.Remover(product);
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
        public async Task< IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO updateproduct)
        {


            if(updateproduct == null || id!= updateproduct.Id)
            {
                response.IsExitoso = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
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
            Product modelo= mapper.Map<Product>(updateproduct);
           await productRepo.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;

            return Ok(response);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateParcialProduct(int id, JsonPatchDocument<ProductUpdateDTO> product)
        {
            if (product == null || id ==0)
            {
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            

            var prod= await productRepo.Obtener(p => p.Id == id,tracked:false);

            ProductUpdateDTO productDTO= mapper.Map<ProductUpdateDTO>(prod);
            //ProductUpdateDTO productDTO = new ProductUpdateDTO()
            //{
            //    Id = prod.Id,
            //    Name = prod.Name,
            //    Description = prod.Description,
            //    Category = prod.Category,
            //    Proveedor = prod.Proveedor,
            //    Precio = prod.Precio,
            //    Stock = prod.Stock,
            //    imgUrl = prod.imgUrl
            //};
            if (prod == null) 
            {
                return BadRequest();
            }
            product.ApplyTo(productDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Product modelo = new Product()
            //{
            //    Id = productDTO.Id,
            //    Name = productDTO.Name,
            //    Description = productDTO.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = productDTO.Category,
            //    Proveedor = productDTO.Proveedor,
            //    Precio = productDTO.Precio,
            //    Stock = productDTO.Stock,
            //    imgUrl = productDTO.imgUrl,
            //    Tipo = ""

            //};
            Product modelo = mapper.Map<Product>(productDTO);
            await productRepo.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;
            return Ok(response);
        }


    }

}
