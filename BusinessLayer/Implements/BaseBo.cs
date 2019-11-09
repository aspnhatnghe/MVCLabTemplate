using Business.Interfaces;
using System;
using System.Collections.Generic;
using Repositories;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Entities;
using System.Linq.Expressions;

namespace Business.Implements
{
    public class BaseBo<TModel, TEntity> : IBaseBo<TModel, TEntity>
        where TModel : class
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IServiceProvider _serviceProvider;

        public BaseBo(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public IUnitOfWork NewDbContext()
        {
            var newContext = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope()
                .ServiceProvider.GetRequiredService<MyDbContext>();
            return new UnitOfWork(newContext);
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            try
            {

                // Get entity from data context.
                using (var unitOfWork = NewDbContext())
                {
                    // Get repository.
                    return GetAll(unitOfWork);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual IEnumerable<TModel> GetAll(IUnitOfWork unitOfWork)
        {
            try
            {
                var models = new List<TModel>();

                // Get entity from data context.
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get all entity in datacontext.
                var entities = repository.GetAll();

                // Map model list to models list.
                models = _mapper.Map<List<TModel>>(entities);

                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TModel> Get(Func<TEntity, bool> filter)
        {
            try
            {
                // Get entity from data context.
                using (var unitOfWork = NewDbContext())
                {
                    // Get repository.
                    return Get(unitOfWork, filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<TModel> Get(IUnitOfWork unitOfWork, Func<TEntity, bool> filter)
        {
            try
            {
                var models = new List<TModel>();

                // Get entity from data context.

                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get all entity in datacontext.
                var entities = repository.GetManyQueryable(filter);

                // Map model list to models list.
                models = _mapper.Map<List<TModel>>(entities);

                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<TModel> GetIgnoreNull(Func<TEntity, bool> filter)
        {
            try
            {
                var models = new List<TModel>();

                // Get entity from data context.
                using (var unitOfWork = NewDbContext())
                {
                    // Get repository.
                    var repository = unitOfWork.Repository<TEntity>();

                    // Get all entity in datacontext.
                    var entities = repository.GetMany(filter);
                    if (entities.Any())
                    {
                        // Map model list to models list.
                        models = _mapper.Map<List<TModel>>(entities);
                    }
                }

                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TModel GetFirstIgnoreNull(Func<TEntity, bool> filter)
        {
            try
            {

                // Get entity from data context.
                using (var unitOfWork = NewDbContext())
                {
                    // Get repository.
                    return GetFirstIgnoreNull(unitOfWork, filter);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TModel GetFirstIgnoreNull(IUnitOfWork unitOfWork, Func<TEntity, bool> filter)
        {
            try
            {
                TModel models = null;

                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get all entity in datacontext.
                var entities = repository.GetManyQueryable(filter);
                if (entities.Any())
                {
                    models = _mapper.Map<TModel>(entities.First());
                }

                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TModel> GetTop(Func<TEntity, bool> filter, int count, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            try
            {
                var models = new List<TModel>();

                // Get entity from data context.
                using (var unitOfWork = NewDbContext())
                {
                    // Get repository.
                    var repository = unitOfWork.Repository<TEntity>();

                    // Get all entity in datacontext.
                    var entities = orderBy(repository.GetManyQueryable(filter)).Take(count);

                    models = _mapper.Map<List<TModel>>(entities);
                    // Map model list to models list.

                }

                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<TEntity> GetTopEntities(IUnitOfWork unitOfWork, Func<TEntity, bool> filter, int count, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            try
            {
                var models = new List<TModel>();

                // Get entity from data context.

                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get all entity in datacontext.
                var entities = orderBy(repository.GetManyQueryable(filter)).Take(count);

                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual TModel GetById(object id)
        {
            // Get entity from data context.
            using (var unitOfWork = NewDbContext())
            {
                // Get repository.
                return GetById(unitOfWork, id);
            }
        }
        public virtual TModel GetById(IUnitOfWork unitOfWork, object id)
        {
            // Get entity from data context.

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            var item = repository.GetById(id);

            return _mapper.Map<TModel>(item);

        }
        public virtual TModel GetFirst(Func<TEntity, bool> filter)
        {
            // Get entity from data context.
            using (var unitOfWork = NewDbContext())
            {
                // Get repository.
                return GetFirst(unitOfWork, filter);
            }
        }

        public virtual TModel GetFirst(IUnitOfWork unitOfWork, Func<TEntity, bool> filter)
        {
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            var item = repository.GetFirst(filter);

            return _mapper.Map<TModel>(item);
        }

        public TModel Insert(TModel item)
        {
            using (var unitOfWork = NewDbContext())
            {
                return Insert(item, unitOfWork);
            }
        }

        public virtual TModel Insert(TModel item, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Mapping to TEntity
                var entity = _mapper.Map<TEntity>(item);

                // Insert TEntity
                var entityResult = repository.Insert(entity);

                // Save change to datacontext.
                bool success = unitOfWork.Save();

                if (success == true)
                {
                    // Return TModel
                    return _mapper.Map<TModel>(entityResult);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TEntity InsertEntity(TEntity item)
        {
            using (var unitOfWork = NewDbContext())
            {
                return InsertEntity(item, unitOfWork);

            }
        }
        public virtual TEntity InsertEntity(TEntity item, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Insert TEntity

                var entityResult = repository.Insert(item);

                // Save change to datacontext.
                unitOfWork.Save();

                // Return TModel
                return entityResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<TEntity> InsertEntities(IEnumerable<TEntity> items)
        {
            using (var unitOfWork = NewDbContext())
            {
                return InsertEntities(items, unitOfWork);
            }
        }
        public virtual IEnumerable<TEntity> InsertEntities(IEnumerable<TEntity> items, IUnitOfWork unitOfWork)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.Insert(items);
        }
        public TModel Update(TModel item, object id)
        {
            using (var unitOfWork = NewDbContext())
            {
                return Update(item, id, unitOfWork);
            }
        }

        public virtual TModel Update(TModel item, object id, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get Entity
                var entity = repository.Find(id);
                if (entity != null)
                {
                    // Mapping to TEntity
                    entity = _mapper.Map(item, entity);
                }

                // Update TEntity
                var entityResult = repository.Update(entity);

                // Save change to datacontext.
                unitOfWork.Save();

                // Return TModel
                return _mapper.Map<TModel>(entityResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TModel UpdateEntity(TEntity item, object id)
        {
            using (var unitOfWork = NewDbContext())
            {
                return UpdateEntity(item, id, unitOfWork);

            }
        }
        public virtual TModel UpdateEntity(TEntity item, object id, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Get Entity
                var entity = repository.Find(id);

                if (entity != null)
                {
                    // Mapping to TEntity
                    entity = _mapper.Map(item, entity);
                }

                // Update TEntity
                var entityResult = repository.Update(entity);

                // Save change to datacontext.
                unitOfWork.Save();

                // Return TModel
                return _mapper.Map<TModel>(entityResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(object id)
        {
            using (var unitOfWork = NewDbContext())
            {
                return Delete(id, unitOfWork);
            }
        }

        public virtual bool Delete(object id, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Delete model.
                repository.Delete(id);

                // Save change to datacontext
                return unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool DeleteEntities(IEnumerable<TEntity> entitiesToDelete)
        {
            using (var unitOfWork = NewDbContext())
            {
                return DeleteEntities(entitiesToDelete, unitOfWork);
            }
        }
        public virtual bool DeleteEntities(IEnumerable<TEntity> entitiesToDelete, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Delete model.
                repository.DeleteRange(entitiesToDelete);

                // Save change to datacontext
                return unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteEntity(TModel entity)
        {
            using (var unitOfWork = NewDbContext())
            {
                return DeleteEntity(entity, unitOfWork);
            }
        }

        public virtual bool DeleteEntity(TModel item, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                var entity = _mapper.Map<TEntity>(item);

                // Delete model.
                repository.Delete(entity);

                // Save change to datacontext
                unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteByEntity(TEntity entity)
        {
            using (var unitOfWork = NewDbContext())
            {
                return DeleteByEntity(entity, unitOfWork);
            }
        }

        public virtual bool DeleteByEntity(TEntity entity, IUnitOfWork unitOfWork)
        {
            try
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Delete model.
                repository.Delete(entity);

                // Save change to datacontext
                unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Exists(object id)
        {
            bool exist;

            // Get entity from data context.
            using (var unitOfWork = NewDbContext())
            {
                // Get repository.
                var repository = unitOfWork.Repository<TEntity>();

                // Check exist.
                exist = repository.Exists(id);
            }

            return exist;
        }

        public int ExecuteSqlCommand(string query, params object[] @params)
        {
            using (var unitOfWork = NewDbContext())
            {
                var repository = unitOfWork.Repository<TEntity>();
                return repository.ExecuteSqlCommand(query, @params);
            }
        }

        public int ExecuteSqlCommand(IUnitOfWork unitOfWork, string query, params object[] @params)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.ExecuteSqlCommand(query, @params);
        }

        public List<T> ExecuteStoreProcedure<T>(string query, params object[] @params) where T : class
        {
            using (var unitOfWork = NewDbContext())
            {
                var repository = unitOfWork.Repository<TEntity>();
                return repository.FromSql<T>(query, @params);
            }
        }

        public IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.GetEntities();
        }

        public IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.GetEntities(filter);
        }

        public IQueryable<TEntity> GetEntitiesWithInclude(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.GetEntitiesWithInclude(filter, includes);
        }
        public IQueryable<TEntity> GetEntitiesWithInclude(IUnitOfWork unitOfWork, params Expression<Func<TEntity, object>>[] includes)
        {
            var repository = unitOfWork.Repository<TEntity>();
            return repository.GetEntitiesWithInclude(includes);
        }
        public IQueryable<TEntity> GetEntitiesWithInclude(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var unitOfWork = NewDbContext())
            {
                var repository = unitOfWork.Repository<TEntity>();
                return repository.GetEntitiesWithInclude(filter, includes);
            }
        }
        public TModel GetFirstWithInclude(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var unitOfWork = NewDbContext())
            {
                var repository = unitOfWork.Repository<TEntity>();
                var item = repository.GetEntitiesWithInclude(filter, includes).First();
                return _mapper.Map<TModel>(item);
            }
        }
        public TModel GetFirstWithIncludeIgnoreNull(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var unitOfWork = NewDbContext())
            {
                var repository = unitOfWork.Repository<TEntity>();
                var item = repository.GetEntitiesWithInclude(filter, includes);
                if (item.Any())
                {
                    return _mapper.Map<TModel>(item.First());
                }
            }
            return null;
        }
    }
}
