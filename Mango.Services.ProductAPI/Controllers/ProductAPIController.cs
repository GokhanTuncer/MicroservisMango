﻿using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        public ProductAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }
        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductID == id);
                _response.Result= _mapper.Map<ProductDTO>(obj);
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Post([FromBody] ProductDTO couponDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(couponDTO);
                _db.Products.Add(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Put([FromBody] ProductDTO couponDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(couponDTO);
                _db.Products.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductID == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
