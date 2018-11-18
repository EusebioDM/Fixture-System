using System;
using System.Collections.Generic;
using EirinDuran.DataAccess.Entities;
using EirinDuran.Domain.Fixture;
using EirinDuran.GenericEntityRepository;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EirinDuran.DataAccess
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly EntityRepository<Comment, CommentEntity> repo;

        public CommentRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<CommentEntity> factory = new EntityFactory<CommentEntity>(() => new CommentEntity());
            Func<DbContext, DbSet<CommentEntity>> getDbSet = c => ((Context)c).Comments;
            repo = new EntityRepository<Comment, CommentEntity>(factory,getDbSet, contextFactory);
        }

        public Comment Get(string id) => repo.Get(id);

        public IEnumerable<Comment> GetAll() => repo.GetAll();

        public void Add(Comment model) => repo.Add(model);

        public void Update(Comment model) => repo.Update(model);

        public void Delete(string id) => repo.Delete(id);
    }
}