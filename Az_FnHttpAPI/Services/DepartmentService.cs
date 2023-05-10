using Az_FnHttpAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Az_FnHttpAPI.Services
{
    public class DepartmentService : IServices<Department, int>
    {

        CompanyContext _context;

        ResponseObject<Department> _response;

        /// <summary>
        /// COnstructor Injection
        /// </summary>
        /// <param name="context"></param>
        public DepartmentService(CompanyContext context)
        {
            //_context = new CompanyContext();
            _context = context;
            _response = new ResponseObject<Department>();
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.CreateAsync(Department entity)
        {
            // Adding to the recordset
            var result = await _context.Departments.AddAsync(entity);
            // Commiting to Database
            await _context.SaveChangesAsync();
            _response.Record = result.Entity;
            _response.Message = "Record Added Successfully";
            _response.StatucCode = 201;
            return _response;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.DeleteAsync(int id)
        {
           
           var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                _response.Message = "Record not Found for delete";
            else
            {
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync();
                _response.Message = "Record deleted";

            }
            return _response;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.GetAsync()
        {
            _response.Records = await _context.Departments.ToListAsync();
            _response.Message = "Records received Successfully";
            _response.StatucCode = 200;
            return _response;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.GetAsync(int id)
        {
            _response.Record = await _context.Departments.FindAsync(id);
            _response.Message = "Record is read Successfully";
            _response.StatucCode = 200;
            return _response;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.UpdateAsync(int id, Department entity)
        {
            
                _context.Entry<Department>(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _response.Message = "Record updated";

            
            return _response;
        }
    }
}
