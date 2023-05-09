using Az_FnHttpAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Az_FnHttpAPI.Services
{
    /// <summary>
    /// TEntity is an entity class that will be included in teh ResponseObject
    /// The 'in' an always input parameter TPk will be an input parameter that will be mostly the PrimaryKey expression e.g. DeptNo, EmpNo
    /// 'where TEntity : EntityBase' a Generic Constraint, means TEntuty will always be a class type
    /// TEntity will always be a class of Type EntityBase
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface IServices<TEntity, in TPk> where TEntity : EntityBase
    {
        Task<ResponseObject<TEntity>> GetAsync();
        Task<ResponseObject<TEntity>> GetAsync(TPk id);
        Task<ResponseObject<TEntity>> CreateAsync(TEntity entity);
        Task<ResponseObject<TEntity>> UpdateAsync(TPk id, TEntity entity);
        Task<ResponseObject<TEntity>> DeleteAsync(TPk id);
    }
}
